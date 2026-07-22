using UnityEngine;

namespace InGame.Enemy
{
    public class EnemySpearLip : MonoBehaviour, ILipAttractTarget
    {
        [SerializeField, Range(-1f, 1f)] private float _attractableRangeThreshoud;
        [SerializeField] private float _attractedCancelTime;
        [SerializeField] private float _attractedCoolTime;
        [SerializeField] private float _attractedPowerCoef;
        [SerializeField] private int _damageAmount;
        [SerializeField] private float _nockbackPower;
        [SerializeField] private MagneticType _seltMagneticType;
        [SerializeField] private Rigidbody2D _lipRb;
        [SerializeField] private Rigidbody2D _bodyRb;
        [SerializeField] private Transform _lipDefaultPointTr;

        private float _attractedCancelTimeCounter;
        private float _attractCoolTimeCount;

        public MagneticType MagneticType => _seltMagneticType;
        private Vector2 LipPositionDefault => _lipDefaultPointTr.position;

        public Vector2 LipPosition
        {
            get => _lipRb.position;
            private set => _lipRb.position = value;
        }

        private bool LipKinematic
        {
            get => _lipRb.bodyType == RigidbodyType2D.Kinematic;
            set => _lipRb.bodyType = value ? RigidbodyType2D.Kinematic : RigidbodyType2D.Dynamic;
        }

        public float LipRotation
        {
            get => _lipRb.rotation;
            private set => _lipRb.rotation = value;
        }

        private Vector2 LipVelocity
        {
            get => _lipRb.linearVelocity;
            set => _lipRb.linearVelocity = value;
        }

        private void FixedUpdate()
        {
            if (0f < _attractCoolTimeCount) _attractCoolTimeCount -= Time.fixedDeltaTime;

            if (LipKinematic)
            {
                FollowBodyUpdate(Time.fixedDeltaTime);
            }
            else
            {
                AttractedUpdate(Time.fixedDeltaTime);
            }
        }

        private void SetAttractCoolTime() => _attractCoolTimeCount = _attractedCoolTime;

        public void OnHitAttackCollider(Collider2D col)
        {
            if (col.TryGetComponent(out IDamageTarget target))
            {
                Vector2 pos = LipPosition;
                Vector2 nockback = (target.Position - pos).normalized * _nockbackPower;
                target.OnDamaged(_damageAmount, nockback, col.ClosestPoint(LipPosition));
                CancelAttracted();
            }
        }

        private void FollowBody()
        {
            LipVelocity = Vector2.zero;
            LipKinematic = true;
            LipPosition = LipPositionDefault;
            LipRotation = _bodyRb.rotation;
        }

        private void FollowBodyUpdate(float dt)
        {
            LipPosition = LipPositionDefault;
            LipRotation = _bodyRb.rotation;
        }

        public void OnAttracted(Vector2 force)
        {
            if (0f < _attractCoolTimeCount) return;

            float dot = Vector2.Dot(OriginalCalculateUtils.AngleToDirection(_lipRb.rotation), force.normalized);

            if (_attractableRangeThreshoud < dot)
            {
                _attractedCancelTimeCounter = 0f;

                LipKinematic = false;
                _lipRb.AddForce(force * _attractedPowerCoef);

                if (LipVelocity.sqrMagnitude != 0f)
                {
                    LipRotation = OriginalCalculateUtils.DirectionToAngle(_lipRb.linearVelocity);
                }
            }
        }

        private void AttractedUpdate(float dt)
        {
            _attractedCancelTimeCounter += dt;

            if (_attractedCancelTime <= _attractedCancelTimeCounter)
            {
                _attractedCancelTimeCounter = 0f;
                CancelAttracted();
            }
        }

        private void CancelAttracted()
        {
            SetAttractCoolTime();
            FollowBody();
        }
    }
}