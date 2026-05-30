using UnityEngine;

namespace InGame.Enemy
{
    public class EnemyAttack : MonoBehaviour, ILipAttachTarget
    {
        [Header("Parameters")]
        [SerializeField] private int _damageAmount;
        [SerializeField] private float _nockbackPower;
        [SerializeField] private MagneticType _selfMagneticType;

        [Header("References")]
        [SerializeField] private Collider2D _col;

        public Vector2 Position => transform.position;
        public MagneticType MagneticType => _selfMagneticType;

        private float Rotation => transform.localEulerAngles.z;
        private const DamageType _DamageType = DamageType.Needle;

        public void OnHitAttackCollider(Collider2D col)
        {
            if (col.TryGetComponent(out IDamageTarget target))
            {
                Vector2 pos = Position;
                Vector2 nockback = (target.Position - pos).normalized * _nockbackPower;
                target.OnDamaged(_damageAmount, nockback);
            }
        }

        public void OnAttached(ILip lip) => lip.OnDamaged(_damageAmount, _nockbackPower, _DamageType);

        public void OnDetached(ILip lip) { }

        public void AddForce(Vector2 force) { }
        public void AddForceImpulse(Vector2 force) { }

        public void AddTorque(float torque) { }
        public void AddTorqueImpulse(float torque) { }

        public Vector2 GetAttachPoint(Vector2 pos) => _col.ClosestPoint(pos);
        public float GetAttachRotation(Vector2 pos) => CalculateUtilities.DirectionToAngle((Position - pos).normalized);

        public Vector2 GetInverseTransformPoint(Vector2 pos) => transform.InverseTransformPoint(pos);
        public Vector2 GetTransformPoint(Vector2 pos) => transform.TransformPoint(pos);

        public float GetInverseTransformRotation(float rot) => Mathf.DeltaAngle(Rotation, rot);
        public float GetTransformRotation(float rot) => Rotation + rot;
    }
}