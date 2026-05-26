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
        [SerializeField] private MagneticType _selfMagneticType;

        [Header("References")]
        [SerializeField] private Collider2D _col;

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.TryGetComponent(out ILipAttractTarget target))
            {
                if (!MagnetJudgement.IsAttachable(_selfMagneticType, target.MagneticType)) return;

                Vector2 lipPos = target.Position;
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
    }
}