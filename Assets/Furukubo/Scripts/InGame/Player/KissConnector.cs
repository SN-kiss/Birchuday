using UnityEngine;

namespace InGame
{
    /// <summary>
    /// Furukubo
    /// </summary>
    public class KissConnector : MonoBehaviour, ILipAttachTarget
    {
        [Header("References")]
        [SerializeField] private Rigidbody2D _rb;

        public Vector2 Position => _rb.position;

        private ILip _lipA;
        private ILip _lipB;

        public void Kiss(ILip lipA, ILip lipB)
        {
            _lipA = lipA;
            _lipB = lipB;

            transform.position = (lipA.Position + lipB.Position) * 0.5f;

            float lipAAngle = lipA.Rotation;

            lipA.OnKissAttach(this, Vector2.zero, lipAAngle);
            lipB.OnKissAttach(this, Vector2.zero, lipAAngle + 180f);

            Debug.Log($"Kiss started : {_lipA} <=> {_lipB}");
        }

        public void OnAttached(ILip attacher) { }

        public void OnDetached(ILip lip)
        {
            if (lip == _lipA)
            {
                _lipB.OnKissDetach();
            }
            else if (lip == _lipB)
            {
                _lipA.OnKissDetach();
            }

            Debug.Log($"Kiss finished : {_lipA} <=> {_lipB}");

            Destroy(gameObject);
        }

        public void AddForce(Vector2 force) => _rb.AddForce(force);
        public Vector2 GetAttachPoint(Vector2 pos) => Vector2.zero;
        public float GetAttachRotation(Vector2 pos) => CalculateUtilities.DirectionToAngle((_rb.position - pos).normalized);

        public Vector2 GetInverseTransformPoint(Vector2 pos) => transform.InverseTransformPoint(pos);
        public Vector2 GetTransformPoint(Vector2 inversPos) => transform.TransformPoint(inversPos);

        public float GetInverseTransformRotation(float rot) => Mathf.DeltaAngle(_rb.rotation, rot);
        public float GetTransformRotation(float inverseRot) => _rb.rotation + inverseRot;
    }
}