using System.Numerics;

namespace Soul.Interactions.Runtime
{
    public interface IFollow
    {
        public bool CanFollow { get; }
        public bool IsFollowing { get; }
        public Vector3 TargetPosition { get; }
        public float StopDistance { get; }
        public float OutsideDistance { get; }
    }
}