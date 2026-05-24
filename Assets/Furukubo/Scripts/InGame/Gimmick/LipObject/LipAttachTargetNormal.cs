using UnityEngine;

namespace InGame
{
    /// <summary>
    /// Furukubo (Refactoring of Mock_ObjectA)
    /// </summary>
    public class LipAttachTargetNormal : MonoBehaviour, ILipAttachTarget
    {
        [Header("Parameters")]
        [SerializeField] private MagneticType _selfMagneticType;

        [Header("References")]
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private Collider2D _col;

        public Vector2 Position => _rb.position;
        public MagneticType MagneticType => _selfMagneticType;

        public void OnAttached(ILip attacher) => Debug.Log("Attached");
        public void OnDetached(ILip lip) => Debug.Log("Detached");

        public void AddForce(Vector2 force) => _rb.AddForce(force);
        public void AddImpulse(Vector2 force) => _rb.AddForce(force, ForceMode2D.Impulse);

        public void AddTorque(float torque) => _rb.AddTorque(torque);
        public void AddTorqueImpulse(float torque) => _rb.AddTorque(torque, ForceMode2D.Impulse);

        public Vector2 GetAttachPoint(Vector2 pos) => _col.ClosestPoint(pos);
        public float GetAttachRotation(Vector2 pos) => CalculateUtilities.DirectionToAngle((_rb.position - pos).normalized);

        public Vector2 GetInverseTransformPoint(Vector2 pos) => transform.InverseTransformPoint(pos);
        public Vector2 GetTransformPoint(Vector2 pos) => transform.TransformPoint(pos);

        public float GetInverseTransformRotation(float rot) => Mathf.DeltaAngle(_rb.rotation, rot);
        public float GetTransformRotation(float rot) => _rb.rotation + rot;
    }
}