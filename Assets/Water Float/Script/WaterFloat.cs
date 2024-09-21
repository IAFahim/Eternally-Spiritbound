using UnityEngine;

public class WaterFloat : MonoBehaviour
{
    public Vector3 MovingDistances = new Vector3(0.002f, 0.001f, 0f); //up and down distance of the wave
    public float speed = 1f; //the speed of up and down

    public Vector3 WaveRotations; //object side rotations
    public float WaveRotationsSpeed = 0.3f; //speed of rotations

    public Vector3 AxisOffsetSpeed; //speed of moving object along an axis

    Transform actualPos; //save the actual transform
    public Vector3 steeringInput;
    public float speedUp = 10;


    void Start()
    {
        actualPos = transform;
    }


    void Update()
    {
        //change axis
        Vector3 mov = new Vector3(
            actualPos.localPosition.x + Mathf.Sin(speed * Time.time) * MovingDistances.x,
            actualPos.localPosition.y + Mathf.Sin(speed * Time.time) * MovingDistances.y,
            actualPos.localPosition.z + Mathf.Sin(speed * Time.time) * MovingDistances.z
        );

        //change rotations
        var leanAmount = Quaternion.Euler(
            actualPos.localRotation.x + WaveRotations.x * Mathf.Sin(Time.time * WaveRotationsSpeed),
            actualPos.localRotation.y + WaveRotations.y * Mathf.Sin(Time.time * WaveRotationsSpeed),
            actualPos.localRotation.z + WaveRotations.z * Mathf.Sin(Time.time * WaveRotationsSpeed)
        );
        
        actualPos.localRotation = Quaternion.Slerp(actualPos.localRotation, leanAmount, Time.deltaTime * speedUp);
        

        //inject the changes
        actualPos.localPosition = mov;

        //offset
        var tran = actualPos.localPosition;

        tran.x += AxisOffsetSpeed.x * Time.deltaTime;
        tran.y += AxisOffsetSpeed.y * Time.deltaTime;
        tran.z += AxisOffsetSpeed.z * Time.deltaTime;

        actualPos.localPosition = tran;
    }
}