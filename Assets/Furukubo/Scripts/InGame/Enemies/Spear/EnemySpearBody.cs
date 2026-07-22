using InGame;
using UnityEngine;

public class EnemySpearBody : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private int _damageAmount;
    [SerializeField] private float _nockbackPower;
    [SerializeField] private float _angleRange;
    [SerializeField] private float _rotateSpeed;

    [Header("References")]
    [SerializeField] private Rigidbody2D _rbBody;

    public Vector2 Position => transform.position;
    private float _defaultAng;
    private float _time;

    private void Awake()
    {
        _defaultAng = _rbBody.rotation;
    }

    private void FixedUpdate()
    {
        _time += _rotateSpeed * Time.fixedDeltaTime;
        _rbBody.MoveRotation(Mathf.PingPong(_time, _angleRange * 2f) - _angleRange + _defaultAng);
    }

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
