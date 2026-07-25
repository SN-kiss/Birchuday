using UnityEngine;

namespace InGame.Enemy
{
    public class EnemySpearLip : MonoBehaviour, ILipAttractTarget
    {
        [SerializeField, Range(0f, 1f)] private float _attractableDotThreshoud;
        [SerializeField] private float _attractedCancelTime;
        [SerializeField] private float _attractedCoolTime;
        [SerializeField] private float _attractedPowerCoef;
        [SerializeField] private int _damageAmount;
        [SerializeField] private float _nockbackPower;
        [SerializeField] private float _maxSpeed;
        [SerializeField] private float _maxDistance;
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

        private void Start()
        {
            FollowBody();
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

            Vector2 curDir = OriginalCalculateUtils.AngleToDirection(_lipRb.rotation);
            float dot = Vector2.Dot(curDir, force.normalized);

            if (_attractableDotThreshoud < dot)
            {
                _attractedCancelTimeCounter = 0f;

                LipKinematic = false;

                float ratio = _attractableDotThreshoud == 1f ? 1f : ((dot - _attractableDotThreshoud) / (1f - _attractableDotThreshoud));
                Vector2 lerpedForce = Vector2.Lerp(Vector2.zero, force * _attractedPowerCoef, ratio);

                _lipRb.AddForce(lerpedForce);

                if (LipVelocity.sqrMagnitude != 0f)
                {
                    LipRotation = OriginalCalculateUtils.DirectionToAngle(_lipRb.linearVelocity);
                }
            }

            if (_maxSpeed * _maxSpeed <= LipVelocity.sqrMagnitude)
            {
                LipVelocity = LipVelocity.normalized * _maxSpeed;
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
            else if (_maxDistance * _maxDistance <= ((Vector2)_lipDefaultPointTr.position - LipPosition).sqrMagnitude)
            {
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