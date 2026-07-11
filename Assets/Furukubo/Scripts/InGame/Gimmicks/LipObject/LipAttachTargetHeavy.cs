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
        [Header("Parameters")]
        [SerializeField] private int _attachedCountNeedToMoveMin;

        [Header("References")]
        [SerializeField] private GameObject _objLampRed;
        [SerializeField] private GameObject _objLampBlue;

        private bool IsMovable => _attachedCountNeedToMoveMin <= _attachedCount;

        private int _attachedCount;

        public override void OnAttached(ILip lip)
        {
            _attachedCount = Mathf.Clamp(_attachedCount + 1, 0, _attachedCountNeedToMoveMin);

            Rb.bodyType =
                _attachedCountNeedToMoveMin <= _attachedCount
                ? RigidbodyType2D.Dynamic
                : RigidbodyType2D.Kinematic;

            if (lip.MagneticType == MagneticType.North)
            {
                if (_objLampRed == null) return;
                _objLampRed.SetActive(true);
            }
            else if (lip.MagneticType == MagneticType.South)
            {
                if (_objLampBlue == null) return;
                _objLampBlue.SetActive(true);
            }
        }

        public override void OnDetached(ILip lip)
        {
            _attachedCount = Mathf.Clamp(_attachedCount - 1, 0, _attachedCountNeedToMoveMin);

            Rb.linearVelocity = Vector2.zero;
            Rb.angularVelocity = 0f;

            Rb.bodyType = 
                _attachedCountNeedToMoveMin <= _attachedCount 
                ? RigidbodyType2D.Dynamic 
                : RigidbodyType2D.Kinematic;

            if(lip.MagneticType == MagneticType.North)
            {
                if (_objLampRed == null) return;
                _objLampRed.SetActive(false);
            }
            else if(lip.MagneticType == MagneticType.South)
            {
                if (_objLampBlue == null) return;
                _objLampBlue.SetActive(false);
            }
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
    }
}