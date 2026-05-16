using UnityEngine;

namespace InGame.Enemy
{
    public class DebugEnemy : MonoBehaviour
    {
        [Header("Parameters")]
        [SerializeField] private int _damageAmount;
        [SerializeField] private float _nockbackPower;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out IDamageTarget target))
            {
                target.OnDamaged(_damageAmount);
                target.OnNockBack((target.Position - (Vector2)transform.position).normalized * _nockbackPower);
            }
        }
    }
}