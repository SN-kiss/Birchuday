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

        public void Kiss(ILip lipA, ILip lipB)
        {

        }

        public void OnAttached(ILip attacher) => Debug.Log("Attached");
        public void OnDetached(ILip lip) => Debug.Log("Detached");

        public void AddForce(Vector2 force) => _rb.AddForce(force);
        public Vector2 GetAttachPoint(Vector2 pos) => Vector2.zero;
        public float GetAttachRotation(Vector2 pos) => CalculateUtilities.DirectionToAngle((_rb.position - pos).normalized);

        public Vector2 GetInverseTransformPoint(Vector2 pos) => transform.InverseTransformPoint(pos);
        public Vector2 GetTransformPoint(Vector2 inversPos) => transform.TransformPoint(inversPos);

        public float GetInverseTransformRotation(float rot) => Mathf.DeltaAngle(_rb.rotation, rot);
        public float GetTransformRotation(float inverseRot) => _rb.rotation + inverseRot;
    }
}