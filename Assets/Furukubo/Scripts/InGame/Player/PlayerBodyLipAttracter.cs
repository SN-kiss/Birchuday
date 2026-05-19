using UnityEngine;

namespace InGame.Player
{
    /// <summary>
    /// Furukubo
    /// </summary>
    public class PlayerBodyLipAttracter : MonoBehaviour
    {
        [Header("Parameters")]
        [SerializeField] private float _attractPower;
        [SerializeField, Range(-1f, 1f)] private float _attractRangeThrehoud;

        [Header("References")]
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private Collider2D _col;
        [SerializeField] private Collider2D _ingnoreCol;

        private void OnTriggerStay2D(Collider2D col)
        {
            if (col == _ingnoreCol) return;

            if (col.TryGetComponent(out ILip lip))
            {
                Vector2 lipPos = lip.Position;
                Vector2 closestPos = GetClosestPoint(lipPos);
                Vector2 between = closestPos - lipPos;

                float dirSimilarity = Vector2.Dot(between.normalized, CalculateUtilities.AngleToDirection(_rb.rotation));

                if (_attractRangeThrehoud <= dirSimilarity)
                {
                    lip.OnAttracted(between * _attractPower);
                }
            }
        }

        private Vector2 GetClosestPoint(Vector2 pos)
        {
            if (_col == null) return transform.position;
            return _col.ClosestPoint(pos);
        }
    }
}