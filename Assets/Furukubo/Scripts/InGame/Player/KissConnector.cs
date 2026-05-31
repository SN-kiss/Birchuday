using UnityEngine;

namespace InGame
{
    /// <summary>
    /// Furukubo
    /// </summary>
    public class KissConnector : MonoBehaviour, ILipAttachTarget
    {
        [Header("Parameters")]
        [SerializeField] private float _lipOffsetRadius;

        [Header("References")]
        [SerializeField] private Rigidbody2D _rb;

        public Vector2 Position => _rb.position;
        public MagneticType MagneticType => MagneticType.Both;

        private ILip _lipA;
        private ILip _lipB;

        public void Kiss(ILip lipA, ILip lipB)
        {
            _lipA = lipA;
            _lipB = lipB;

            Vector2 center = (lipA.LipPosition + lipB.LipPosition) * 0.5f;
            transform.position = center;
            _rb.position = center;

            float intverseAngleLipA = lipA.LipRotation;

            Vector2 inversePosLipB = OriginalCalculateUtils.AngleToDirection(intverseAngleLipA) * _lipOffsetRadius;

            lipA.OnAttachFromOther(this, -inversePosLipB, intverseAngleLipA);
            lipB.OnAttachFromOther(this, inversePosLipB, intverseAngleLipA + 180f);

            Debug.Log($"Kiss started : {_lipA} <=> {_lipB}");
        }

        public void OnAttached(ILip attacher) { }

        public void OnDetached(ILip lip)
        {
            if (lip == _lipA)
            {
                _lipB.OnDetachFromOther();
            }
            else if (lip == _lipB)
            {
                _lipA.OnDetachFromOther();
            }

            Debug.Log($"Kiss finished : {_lipA} <=> {_lipB}");

            Destroy(gameObject);
        }

        public void AddForce(Vector2 force) => _rb.AddForce(force);
        public void AddForceImpulse(Vector2 force) => _rb.AddForce(force, ForceMode2D.Impulse);

        public void AddTorque(float torque) => _rb.AddTorque(torque);
        public void AddTorqueImpulse(float torque) => _rb.AddTorque(torque, ForceMode2D.Impulse);

        public Vector2 GetAttachPoint(Vector2 pos) => Vector2.zero;
        public float GetAttachRotation(Vector2 pos) => 0f;

        public Vector2 GetInverseTransformPoint(Vector2 pos) => transform.InverseTransformPoint(pos);
        public Vector2 GetTransformPoint(Vector2 inversPos) => transform.TransformPoint(inversPos);

        public float GetInverseTransformRotation(float rot) => Mathf.DeltaAngle(_rb.rotation, rot);
        public float GetTransformRotation(float inverseRot) => _rb.rotation + inverseRot;
    }
}