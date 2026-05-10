using UnityEngine;

namespace InGame
{
    /// <summary>
    /// Furukubo (Refactoring of ObjectA)
    /// </summary>
    public class TestAttractObject : MonoBehaviour, IAttracter
    {
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private Collider2D _col;
        [SerializeField] private TestAttractArea _area;

        public float Weight => _rb.mass;

        private void Awake()
        {
            _area.Attracter += () => this;
        }

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