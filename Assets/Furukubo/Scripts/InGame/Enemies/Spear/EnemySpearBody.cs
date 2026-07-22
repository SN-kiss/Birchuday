using InGame;
using UnityEngine;

public class EnemySpearBody : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private int _damageAmount;
    [SerializeField] private float _nockbackPower;

    public Vector2 Position => transform.position;

    public void OnHitAttackCollider(Collider2D col)
    {
        if (col.TryGetComponent(out IDamageTarget target))
        {
            Vector2 pos = Position;
            Vector2 nockback = (target.Position - pos).normalized * _nockbackPower;
            target.OnDamaged(_damageAmount, nockback, col.ClosestPoint(Position));
        }
    }
}
