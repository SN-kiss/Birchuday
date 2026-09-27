using UnityEngine;

namespace InGame
{
    /// <summary>
    /// Furukubo (Refactoring of Mock_ObjectA)
    /// </summary>
    public class LipAttachTargetNormal : MonoBehaviour, ILipAttachTarget
    {
        [Header("LipAttachTargetNormal----------------")]
        [Header("Parameters")]
        [SerializeField] private MagneticType _selfMagneticType;

        [Header("References")]
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private Collider2D _col;

        public virtual Vector2 Position => _rb.position;
        public MagneticType MagneticType => _selfMagneticType;

        protected Rigidbody2D Rb => _rb;
        protected Collider2D Col => _col;

        private int _attachingCount;

        public virtual void OnAttached(ILip lip)
        {
            _attachingCount = Mathf.Clamp(_attachingCount + 1, 0, int.MaxValue);

            Rb.bodyType = RigidbodyType2D.Dynamic;
        }

        public virtual void OnDetached(ILip lip)
        {
            _attachingCount = Mathf.Clamp(_attachingCount - 1, 0, int.MaxValue);

            if(_attachingCount <= 0)
            {
                Rb.bodyType = RigidbodyType2D.Kinematic;
                Rb.linearVelocity = Vector2.zero;
                Rb.angularVelocity = 0f;
            }
        }

        public virtual void AddForce(Vector2 force) => _rb.AddForce(force);
        public virtual void AddForceImpulse(Vector2 force) => _rb.AddForce(force, ForceMode2D.Impulse);

        public virtual void AddTorque(float torque) => _rb.AddTorque(torque);
        public virtual void AddTorqueImpulse(float torque) => _rb.AddTorque(torque, ForceMode2D.Impulse);

        public virtual Vector2 GetAttachPoint(Vector2 pos) => _col.ClosestPoint(pos);
        public virtual float GetAttachRotation(Vector2 pos) => OriginalCalculateUtils.DirectionToAngle((_col.ClosestPoint(pos) - pos).normalized);

        public virtual Vector2 GetInverseTransformPoint(Vector2 pos) => transform.InverseTransformPoint(pos);
        public virtual Vector2 GetTransformPoint(Vector2 pos) => transform.TransformPoint(pos);

        public virtual float GetInverseTransformRotation(float rot) => Mathf.DeltaAngle(_rb.rotation, rot);
        public virtual float GetTransformRotation(float rot) => _rb.rotation + rot;
    }
}