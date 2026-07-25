using InGame.Player;
using UnityEngine;

namespace InGame
{
    /// <summary>
    /// Furukubo (Refactoring of SunctionZone)
    /// </summary>
    public class LipAttracter : MonoBehaviour
    {
        [Header("Parameters")]
        [SerializeField] private float _attractPowerBase;
        [SerializeField] private float _attractPower;
        [SerializeField] private string _wallLayerName;
        [SerializeField] private MagneticType _selfMagneticType;
        [SerializeField] private string[] _ignoreTags;

        [Header("References")]
        [SerializeField] private Collider2D _col;

        private void OnTriggerStay2D(Collider2D col)
        {
            if (_ignoreTags != null)
            {
                foreach (var tag in _ignoreTags)
                {
                    if (col.CompareTag(tag)) return;
                }
            }

            if (IsBindingWall(transform.position, col.transform.position))
            {
                Debug.Log("Is binding wall");
                return;
            }

            if (col.TryGetComponent(out ILipAttractTarget target))
            {
                if (!MagnetJudgement.IsAttachable(_selfMagneticType, target.MagneticType)) return;

                Vector2 lipPos = target.LipPosition;
                Vector2 closestPos = GetClosestPoint(lipPos);
                Vector2 between = closestPos - lipPos;

                float power = _attractPowerBase + (_attractPower / Mathf.Clamp(between.sqrMagnitude, 1f, float.MaxValue));
                target.OnAttracted(between.normalized * power);
            }
        }

        private Vector2 GetClosestPoint(Vector2 pos)
        {
            if(_col == null) return transform.position;
            return _col.ClosestPoint(pos);
        }

        private bool IsBindingWall(Vector2 start, Vector2 end)
        {
            RaycastHit2D[] hits = Physics2D.LinecastAll(start, end, LayerMask.GetMask(_wallLayerName));

            return 0 < hits.Length;
        }
    }
}