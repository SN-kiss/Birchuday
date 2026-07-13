using TMPro;
using UnityEngine;

namespace InGame.Gimmick
{
    /// <summary>
    /// Furukubo
    /// </summary>
    public class LipAttachTargetHeavy : LipAttachTargetNormal
    {
        [Header("LipAttachTargetHeavy----------------")]

        [Header("References")]
        [SerializeField] private SpriteRenderer _sr;
        [SerializeField] private Sprite _sprNone;
        [SerializeField] private Sprite _sprNorth;
        [SerializeField] private Sprite _sprSouth;
        [SerializeField] private Sprite _sprBoth;

        private bool IsMovable => _northAttaching && _southAttaching;

        private bool _northAttaching;
        private bool _southAttaching;

        public override void OnAttached(ILip lip)
        {
            if (lip.MagneticType == MagneticType.North)
            {
                _northAttaching = true;
            }
            else if (lip.MagneticType == MagneticType.South)
            {
                _southAttaching = true;
            }

            Rb.bodyType = IsMovable ? RigidbodyType2D.Dynamic : RigidbodyType2D.Kinematic;
            if(!IsMovable) Rb.linearVelocity = Vector2.zero;

            ChangeSprite();
        }

        public override void OnDetached(ILip lip)
        {
            if (lip.MagneticType == MagneticType.North)
            {
                _northAttaching = false;
            }
            else if (lip.MagneticType == MagneticType.South)
            {
                _southAttaching = false;
            }

            Rb.bodyType = IsMovable ? RigidbodyType2D.Dynamic : RigidbodyType2D.Kinematic;
            if (!IsMovable) Rb.linearVelocity = Vector2.zero;

            ChangeSprite();
        }

        public override void AddForce(Vector2 force)
        {
            if (IsMovable) Rb.AddForce(force);
        }

        public override void AddForceImpulse(Vector2 force)
        {
            if (IsMovable) Rb.AddForce(force, ForceMode2D.Impulse);
        }

        public override void AddTorque(float torque)
        {
            if (IsMovable) Rb.AddTorque(torque);
        }

        public override void AddTorqueImpulse(float torque)
        {
            if (IsMovable) Rb.AddTorque(torque, ForceMode2D.Impulse);
        }

        private void ChangeSprite()
        {
            if (_sr == null) return;

            switch (_northAttaching, _southAttaching)
            {
                case (true, true):
                    _sr.sprite = _sprBoth;
                    break;
                case (true, false):
                    _sr.sprite = _sprNorth;
                    break;
                case (false, true):
                    _sr.sprite = _sprSouth;
                    break;
                case (false, false):
                    _sr.sprite = _sprNone;
                    break;
            }
        }
    }
}