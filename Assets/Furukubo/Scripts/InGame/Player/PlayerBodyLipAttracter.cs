using UnityEngine;

namespace InGame.Player
{
    /// <summary>
    /// Furukubo
    /// </summary>
    public class PlayerBodyLipAttracter : MonoBehaviour
    {
        [Header("Parameters")]
        [SerializeField] private float _attractPowerBase;
        [SerializeField] private float _attractPower;
        [SerializeField, Range(-1f, 1f)] private float _attractRangeThrehoud;
        [SerializeField] private MagneticType _selfMagneticType;

        [Header("References")]
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private Collider2D _col;
        [SerializeField] private Collider2D _ingnoreCol;

        private void OnTriggerStay2D(Collider2D col)
        {
            if (col == _ingnoreCol) return;

            if (col.TryGetComponent(out ILipAttractTarget target))
            {
                if (!MagnetJudgement.IsAttachable(_selfMagneticType, target.MagneticType)) return;

                Vector2 lipPos = target.LipPosition;
                Vector2 closestPos = GetClosestPoint(lipPos);
                Vector2 between = closestPos - lipPos;

                float dirSimilarity = Vector2.Dot(between.normalized, OriginalCalculateUtils.AngleToDirection(_rb.rotation));

                if (_attractRangeThrehoud <= dirSimilarity)
                {
                    float power = _attractPowerBase + (_attractPower / Mathf.Clamp(between.sqrMagnitude, 1f, float.MaxValue));
                    target.OnAttracted(between.normalized * power);
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