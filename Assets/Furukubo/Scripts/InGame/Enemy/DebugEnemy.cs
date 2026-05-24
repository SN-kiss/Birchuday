using InGame.Player;
using UnityEngine;

namespace InGame.Enemy
{
    public class DebugEnemy : MonoBehaviour, ILipAttachTarget
    {
        [Header("Parameters")]
        [SerializeField] private int _damageAmount;
        [SerializeField] private float _nockbackPower;
        [SerializeField] private LipDamageType _damageType;

        [Header("References")]
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private Collider2D _col;
        [SerializeField] private Transform _trTexture;

        public Vector2 Position => _rb.position;

        private void Update()
        {
            _trTexture.localEulerAngles = new Vector3(0, 0, Time.time * 90f);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out IDamageTarget target)) Attack(target);
        }

        public void OnAttached(ILip lip)//change to ILipAttractTarget
        {
            lip.OnLipDamage(1, _nockbackPower, _damageType);
        }

        public void OnDetached(ILip lip) { }

        public void AddForce(Vector2 force) { }
        public void AddImpulse(Vector2 force) { }

        public Vector2 GetAttachPoint(Vector2 pos) => _col.ClosestPoint(pos);
        public float GetAttachRotation(Vector2 pos) => CalculateUtilities.DirectionToAngle((_rb.position - pos).normalized);

        public Vector2 GetInverseTransformPoint(Vector2 pos) => transform.InverseTransformPoint(pos);
        public Vector2 GetTransformPoint(Vector2 pos) => transform.TransformPoint(pos);

        public float GetInverseTransformRotation(float rot) => Mathf.DeltaAngle(_rb.rotation, rot);
        public float GetTransformRotation(float rot) => _rb.rotation + rot;

        private void Attack(IDamageTarget target)
        {
            Vector2 pos = _rb.position;
            Vector2 nockback = (target.Position - pos).normalized * _nockbackPower;
            target.OnDamaged(_damageAmount, nockback);
        }
    }
}