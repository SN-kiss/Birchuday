using UnityEngine;

namespace InGame
{
    /// <summary>
    /// Furukubo (Refactoring of Mock_ObjectA)
    /// </summary>
    public class LipAttacherNormal : MonoBehaviour, ILipAttacher
    {
        [Header("References")]
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private Collider2D _col;

        public Vector2 Position => _rb.position;
        public float Rotation => _rb.rotation;

        public void OnAttached() => Debug.Log("Attached");
        public void OnDetached() => Debug.Log("Detached");

        public void AddForce(Vector2 force) => _rb.AddForce(force);
        public Vector2 GetClosestPoint(Vector2 pos) => _col.ClosestPoint(pos);
        public Vector2 GetInverseTransformPoint(Vector2 pos) => transform.InverseTransformPoint(pos);
        public Vector2 GetTransformPoint(Vector2 pos) => transform.TransformPoint(pos);
    }
}