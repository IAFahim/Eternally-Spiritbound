using _Root.Scripts.Game.Inputs.Runtime;
using _Root.Scripts.Game.MainGameObjectProviders.Runtime;
using _Root.Scripts.Game.MainProviders.Runtime;
using _Root.Scripts.Game.Utils.Runtime;
using Pancake;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace _Root.Scripts.Game.Movements.Runtime.Character_Controller
{
    /// <summary>
    /// A floating-capsule oriented physics based character controller. Based on the approach devised by Toyful Games for Very Very Valet.
    /// </summary>
    public class PhysicsBasedCharacterController : MovementProviderComponent, IMainCameraProvider
    {
        [SerializeField] private Optional<Transform> parent;
        private Rigidbody _rb;
        private Vector3 _gravitationalForce;
        private readonly Vector3 _rayDir = Vector3.down;
        private Vector3 _previousVelocity = Vector3.zero;
        private ParticleSystem.EmissionModule _emission;

        [Header("Other:")] [SerializeField] private bool adjustInputsToCameraAngle = false;

        [SerializeField] private LayerMask terrainLayer;

        [SerializeField] private ParticleSystem dustParticleSystem;

        private bool _shouldMaintainHeight = true;

        [Header("Height Spring:")]
        // rideHeight: desired distance to ground (Note, this is distance from the original raycast position (currently centre of transform)). 
        [SerializeField]
        private float rideHeight = 2f;

        // rayToGroundLength: max distance of raycast to ground (Note, this should be greater than the rideHeight).
        [SerializeField] private float rayToGroundLength = 3f;
        [SerializeField] public float rideSpringStrength = 200f; // rideSpringStrength: strength of spring. (?)
        [SerializeField] private float rideSpringDamper = 10f; // rideSpringDampener: dampener of spring. (?)
        [SerializeField] private Oscillator squashAndStretchOcillator;


        private enum ELookDirectionOptions
        {
            Velocity,
            Acceleration,
            MoveInput
        };

        private Quaternion
            _uprightTargetRot = Quaternion.identity; // Adjust y value to match the desired direction to face.

        private Quaternion _lastTargetRot;
        private Vector3 _platformInitRot;
        private bool _didLastRayHit;

        [Header("Upright Spring:")] [SerializeField]
        private ELookDirectionOptions characterELookDirection = ELookDirectionOptions.Velocity;

        [SerializeField] private float uprightSpringStrength = 40f;

        [SerializeField] private float uprightSpringDamper = 5f;


        private Vector3 _moveInput;
        private readonly float _speedFactor = 1f;
        private readonly float _maxAccelForceFactor = 1f;
        private Vector3 _mGoalVel = Vector3.zero;

        [Header("Movement:")] [SerializeField] private float maxSpeed = 8f;

        [SerializeField] private float acceleration = 400f;
        [SerializeField] private float maxAccelForce = 300f;
        [SerializeField] private float leanFactor = 0.2f;
        [SerializeField] private AnimationCurve accelerationFactorFromDot;
        [SerializeField] private AnimationCurve maxAccelerationForceFactorFromDot;
        [SerializeField] private Vector3 moveForceScale = new(1f, 0f, 1f);


        private Vector3 _jumpInput;
        private float _timeSinceJumpPressed = 0f;
        private float _timeSinceUngrounded = 0f;
        private float _timeSinceJump = 0f;
        private bool _jumpReady = true;
        private bool _isJumping = false;

        [Header("Jump:")] [SerializeField] private float jumpForceFactor = 10f;

        [SerializeField] private float riseGravityFactor = 5f;

        [SerializeField] private float fallGravityFactor = 10f; // typically > 1f (i.e. 5f).

        [SerializeField] private float lowJumpFactor = 2.5f;

        [SerializeField]
        private float jumpBuffer = 0.15f; // Note, jumpBuffer shouldn't really exceed the time of the jump.

        [SerializeField] private float coyoteTime = 0.25f;

        /// <summary>
        /// Prepare frequently used variables.
        /// </summary>
        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _gravitationalForce = Physics.gravity * _rb.mass;

            if (dustParticleSystem)
            {
                _emission = dustParticleSystem.emission; // Stores the module in a local variable
                _emission.enabled = false; // Applies the new value directly to the Particle System
            }
        }

        /// <summary>
        /// Use the result of a Raycast to determine if the capsules distance from the ground is sufficiently close to the desired ride height such that the character can be considered 'grounded'.
        /// </summary>
        /// <param name="rayHitGround">Whether the Raycast hit anything.</param>
        /// <param name="rayHit">Information about the ray.</param>
        /// <returns>Whether the player is considered grounded.</returns>
        private bool CheckIfGrounded(bool rayHitGround, RaycastHit rayHit)
        {
            bool grounded;
            if (rayHitGround == true)
            {
                grounded = rayHit.distance <=
                           rideHeight *
                           1.3f; // 1.3f allows for greater leniancy (as the value will oscillate about the rideHeight).
            }
            else
            {
                grounded = false;
            }

            return grounded;
        }

        /// <summary>
        /// Gets the look desired direction for the character to look.
        /// The method for determining the look direction is depending on the lookDirectionOption.
        /// </summary>
        /// <param name="eLookDirectionOption">The factor which determines the look direction: velocity, acceleration or moveInput.</param>
        /// <returns>The desired look direction.</returns>
        private Vector3 GetLookDirection(ELookDirectionOptions eLookDirectionOption)
        {
            Vector3 lookDirection = Vector3.zero;
            if (eLookDirectionOption is ELookDirectionOptions.Velocity or ELookDirectionOptions.Acceleration)
            {
                Vector3 velocity = _rb.linearVelocity;
                velocity.y = 0f;
                if (eLookDirectionOption == ELookDirectionOptions.Velocity) lookDirection = velocity;
                else
                {
                    Vector3 deltaVelocity = velocity - _previousVelocity;
                    _previousVelocity = velocity;
                    lookDirection = deltaVelocity / Time.fixedDeltaTime;
                }
            }
            else if (eLookDirectionOption == ELookDirectionOptions.MoveInput) lookDirection = _moveInput;

            return lookDirection;
        }

        private bool _prevGrounded = false;

        /// <summary>
        /// Determines and plays the appropriate character sounds, particle effects, then calls the appropriate methods to move and float the character.
        /// </summary>
        private void FixedUpdate()
        {
            _moveInput = new Vector3(MoveDirection.x, 0, MoveDirection.y);

            if (adjustInputsToCameraAngle && _mainCamera) _moveInput = AdjustInputToFaceCamera(_moveInput);

            (bool rayHitGround, RaycastHit rayHit) = RaycastToGround();

            bool grounded = CheckIfGrounded(rayHitGround, rayHit);
            if (grounded == true)
            {
                if (dustParticleSystem)
                {
                    if (_emission.enabled == false)
                    {
                        _emission.enabled =
                            true; // Applies the new value directly to the Particle System                  
                    }
                }

                _timeSinceUngrounded = 0f;

                if (_timeSinceJump > 0.2f)
                {
                    _isJumping = false;
                }
            }
            else
            {
                if (dustParticleSystem)
                {
                    if (_emission.enabled == true)
                    {
                        _emission.enabled = false; // Applies the new value directly to the Particle System
                    }
                }

                _timeSinceUngrounded += Time.fixedDeltaTime;
            }

            CharacterMove(_moveInput, rayHit);
            CharacterJump(_jumpInput, grounded, rayHit);

            if (rayHitGround && _shouldMaintainHeight)
            {
                MaintainHeight(rayHit);
            }

            Vector3 lookDirection = GetLookDirection(characterELookDirection);
            MaintainUpright(lookDirection, rayHit);

            _prevGrounded = grounded;
        }

        /// <summary>
        /// Perfom raycast towards the ground.
        /// </summary>
        /// <returns>Whether the ray hit the ground, and information about the ray.</returns>
        private (bool, RaycastHit) RaycastToGround()
        {
            Ray rayToGround = new Ray(transform.position, _rayDir);
            bool rayHitGround = Physics.Raycast(rayToGround, out var rayHit, rayToGroundLength, terrainLayer.value);
            //Debug.DrawRay(transform.position, _rayDir * _rayToGroundLength, Color.blue);
            return (rayHitGround, rayHit);
        }

        /// <summary>
        /// Determines the relative velocity of the character to the ground beneath,
        /// Calculates and applies the oscillator force to bring the character towards the desired ride height.
        /// Additionally applies the oscillator force to the squash and stretch oscillator, and any object beneath.
        /// </summary>
        /// <param name="rayHit">Information about the RaycastToGround.</param>
        private void MaintainHeight(RaycastHit rayHit)
        {
            Vector3 vel = _rb.linearVelocity;
            Vector3 otherVel = Vector3.zero;
            Rigidbody hitBody = rayHit.rigidbody;
            if (hitBody != null)
            {
                otherVel = hitBody.linearVelocity;
            }

            float rayDirVel = Vector3.Dot(_rayDir, vel);
            float otherDirVel = Vector3.Dot(_rayDir, otherVel);

            float relVel = rayDirVel - otherDirVel;
            float currHeight = rayHit.distance - rideHeight;
            float springForce = (currHeight * rideSpringStrength) - (relVel * rideSpringDamper);
            Vector3 maintainHeightForce = -_gravitationalForce + springForce * Vector3.down;
            Vector3 oscillationForce = springForce * Vector3.down;
            _rb.AddForce(maintainHeightForce);
            squashAndStretchOcillator.ApplyForce(oscillationForce);
            //Debug.DrawLine(transform.position, transform.position + (_rayDir * springForce), Color.yellow);

            // Apply force to objects beneath
            if (hitBody != null) hitBody.AddForceAtPosition(-maintainHeightForce, rayHit.point);
        }

        /// <summary>
        /// Determines the desired y rotation for the character, with account for platform rotation.
        /// </summary>
        /// <param name="yLookAt">The input look rotation.</param>
        /// <param name="rayHit">The rayHit towards the platform.</param>
        private void CalculateTargetRotation(Vector3 yLookAt, RaycastHit rayHit = new RaycastHit())
        {
            if (_didLastRayHit)
            {
                _lastTargetRot = _uprightTargetRot;
                _platformInitRot = parent ? parent.Value.rotation.eulerAngles : Vector3.zero;
            }

            _didLastRayHit = rayHit.rigidbody == null;

            if (yLookAt != Vector3.zero)
            {
                _uprightTargetRot = Quaternion.LookRotation(yLookAt, Vector3.up);
                _lastTargetRot = _uprightTargetRot;
                _platformInitRot = parent ? parent.Value.rotation.eulerAngles : Vector3.zero;
            }
            else
            {
                if (!parent) return;
                Vector3 platformRot = parent.Value.rotation.eulerAngles;
                Vector3 deltaPlatformRot = platformRot - _platformInitRot;
                float yAngle = _lastTargetRot.eulerAngles.y + deltaPlatformRot.y;
                _uprightTargetRot = Quaternion.Euler(new Vector3(0f, yAngle, 0f));
            }
        }

        /// <summary>
        /// Adds torque to the character to keep the character upright, acting as a torsional oscillator (i.e. vertically flipped pendulum).
        /// </summary>
        /// <param name="yLookAt">The input look rotation.</param>
        /// <param name="rayHit">The rayHit towards the platform.</param>
        private void MaintainUpright(Vector3 yLookAt, RaycastHit rayHit = new RaycastHit())
        {
            CalculateTargetRotation(yLookAt, rayHit);

            Quaternion currentRot = transform.rotation;
            Quaternion toGoal = MathsUtils.ShortestRotation(_uprightTargetRot, currentRot);

            toGoal.ToAngleAxis(out var rotDegrees, out var rotAxis);
            rotAxis.Normalize();

            float rotRadians = rotDegrees * Mathf.Deg2Rad;

            _rb.AddTorque(
                (rotAxis * (rotRadians * uprightSpringStrength)) - (_rb.angularVelocity * uprightSpringDamper));
        }

        public bool stopMove = false;


        /// <summary>
        /// Reads the player jump input.
        /// </summary>
        /// <param name="context">The jump input's context.</param>
        public void JumpInputAction(InputAction.CallbackContext context)
        {
            float jumpContext = context.ReadValue<float>();
            _jumpInput = new Vector3(0, jumpContext, 0);

            if (context.started) // button down
            {
                _timeSinceJumpPressed = 0f;
            }
        }

        /// <summary>
        /// Adjusts the input, so that the movement matches input regardless of camera rotation.
        /// </summary>
        /// <param name="moveInput">The player movement input.</param>
        /// <returns>The camera corrected movement input.</returns>
        private Vector3 AdjustInputToFaceCamera(Vector3 moveInput)
        {
            float facing = _mainCamera.Value.transform.eulerAngles.y;
            return (Quaternion.Euler(0, facing, 0) * moveInput);
        }


        /// <summary>
        /// Apply forces to move the character up to a maximum acceleration, with consideration to acceleration graphs.
        /// </summary>
        /// <param name="moveInput">The player movement input.</param>
        /// <param name="rayHit">The rayHit towards the platform.</param>
        private void CharacterMove(Vector3 moveInput, RaycastHit rayHit)
        {
            Vector3 unitGoal = moveInput;
            Vector3 unitVel = _mGoalVel.normalized;
            float velDot = Vector3.Dot(unitGoal, unitVel);
            float accel = acceleration * accelerationFactorFromDot.Evaluate(velDot);
            Vector3 goalVel = unitGoal * (maxSpeed * _speedFactor);
            _mGoalVel = Vector3.MoveTowards(_mGoalVel,
                goalVel,
                accel * Time.fixedDeltaTime);
            Vector3 neededAccel = (_mGoalVel - _rb.linearVelocity) / Time.fixedDeltaTime;
            float maxAccel = maxAccelForce * maxAccelerationForceFactorFromDot.Evaluate(velDot) * _maxAccelForceFactor;
            neededAccel = Vector3.ClampMagnitude(neededAccel, maxAccel);
            _rb.AddForceAtPosition(Vector3.Scale(neededAccel * _rb.mass, moveForceScale),
                transform.position +
                new Vector3(0f, transform.localScale.y * leanFactor,
                    0f)); // Using AddForceAtPosition in order to both move the player and cause the play to lean in the direction of input.
        }

        /// <summary>
        /// Apply force to cause the character to perform a single jump, including coyote time and a jump input buffer.
        /// </summary>
        /// <param name="jumpInput">The player jump input.</param>
        /// <param name="grounded">Whether the player is considered grounded.</param>
        /// <param name="rayHit">The rayHit towards the platform.</param>
        private void CharacterJump(Vector3 jumpInput, bool grounded, RaycastHit rayHit)
        {
            _timeSinceJumpPressed += Time.fixedDeltaTime;
            _timeSinceJump += Time.fixedDeltaTime;
            var linearVelocity = _rb.linearVelocity;
            if (linearVelocity.y < 0)
            {
                _shouldMaintainHeight = true;
                _jumpReady = true;
                if (!grounded)
                {
                    // Increase downforce for a sudden plummet.
                    // Hmm... this feels a bit weird. I want a reactive jump, but I don't want it to dive all the time...
                    _rb.AddForce(_gravitationalForce * (fallGravityFactor - 1f));
                }
            }
            else if (linearVelocity.y > 0)
            {
                if (!grounded)
                {
                    if (_isJumping)
                    {
                        _rb.AddForce(_gravitationalForce * (riseGravityFactor - 1f));
                    }

                    if (jumpInput == Vector3.zero)
                    {
                        // Impede the jump height to achieve a low jump.
                        _rb.AddForce(_gravitationalForce * (lowJumpFactor - 1f));
                    }
                }
            }

            if (_timeSinceJumpPressed < jumpBuffer)
            {
                if (_timeSinceUngrounded < coyoteTime)
                {
                    if (_jumpReady)
                    {
                        _jumpReady = false;
                        _shouldMaintainHeight = false;
                        _isJumping = true;
                        _rb.linearVelocity =
                            new Vector3(linearVelocity.x, 0f,
                                linearVelocity.z); // Cheat fix... (see comment below when adding force to rigidbody).
                        if (rayHit.distance != 0) // i.e. if the ray has hit
                        {
                            var rbPos = _rb.position;
                            _rb.position = new Vector3(rbPos.x, rbPos.y - (rayHit.distance - rideHeight), rbPos.z);
                        }

                        // This does not work very consistently... Jump height is affected by initial y velocity and y position relative to RideHeight... Want to adopt a fancier approach (more like PlayerMovement). A cheat fix to ensure consistency has been issued above...
                        _rb.AddForce(Vector3.up * jumpForceFactor, ForceMode.Impulse);
                        // So as to not activate further jumps, in the case that the player lands before the jump timer surpasses the buffer.
                        _timeSinceJumpPressed = jumpBuffer;
                        _timeSinceJump = 0f;
                    }
                }
            }
        }

        private Optional<Camera> _mainCamera;

        public Camera MainCamera
        {
            get => _mainCamera.Value;
            set => _mainCamera = value;
        }

        public void SetParent(Transform parentTransform) => parent = parentTransform;

        protected override void OnMoveInput(InputAction.CallbackContext context)
        {
            MoveDirection = context.ReadValue<Vector2>();
        }
    }
}