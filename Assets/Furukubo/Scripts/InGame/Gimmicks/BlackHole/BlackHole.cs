using UnityEngine;

namespace InGame.Gimmick
{
    public class BlackHole : MonoBehaviour
    {
        [SerializeField] float _inhalePowerBase;

        public void OnHitInhaleColiider(Collider2D col)
        {
            if (col.TryGetComponent(out IBlackHoleTarget target))
            {
                Vector2 v = (Vector2)transform.position - target.Position;
                float power = _inhalePowerBase / Mathf.Clamp(v.sqrMagnitude, 1f, float.MaxValue);
                target.AddForce(power * v.normalized);
            }
        }

        public void OnHitAttackColiider(Collider2D col)
        {
            if(col.TryGetComponent(out IDamageTarget target))
            {
                target.OnDamaged(999, Vector2.zero);
            }
        }
    }
}