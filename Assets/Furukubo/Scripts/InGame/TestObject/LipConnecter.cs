using UnityEngine;

namespace InGame
{
    /// <summary>
    /// Furukubo (Refactoring of Mock_ObjectA)
    /// </summary>
    public class LipConnecter : MonoBehaviour, ILipConnecter
    {
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private Collider2D _col;

        public float Rotation => _rb.rotation;
        public float Weight => _rb.mass;

        public void AddForce(Vector2 force) => _rb.AddForce(force);

        public Vector2 GetClosestPoint(Vector2 pos) => _col.ClosestPoint(pos);

        public Vector2 GetInverseTransformPoint(Vector2 pos)
        {
            return transform.InverseTransformPoint(pos);
        }

        public Vector2 GetTransformPoint(Vector2 pos)
        {
            return transform.TransformPoint(pos);
        }
    }
}