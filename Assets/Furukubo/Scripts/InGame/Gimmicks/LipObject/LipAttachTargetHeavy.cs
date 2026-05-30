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
        [SerializeField] private TextMeshProUGUI _debugAttachedCountText;

        private bool IsMovable => _attachedCountNeedToMoveMin <= _attachedCount;

        private int _attachedCount;

        private void Awake()
        {
            _debugAttachedCountText.text = $"{_attachedCount}/{_attachedCountNeedToMoveMin}";
        }

        public override void OnAttached(ILip attacher)
        {
            _attachedCount = Mathf.Clamp(_attachedCount + 1, 0, _attachedCountNeedToMoveMin);

            Rb.bodyType =
                _attachedCountNeedToMoveMin <= _attachedCount
                ? RigidbodyType2D.Dynamic
                : RigidbodyType2D.Kinematic;

            UpdateAttachedCountText();
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

            UpdateAttachedCountText();
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

        private void UpdateAttachedCountText()
        {
            _debugAttachedCountText.text = $"{_attachedCount}/{_attachedCountNeedToMoveMin}";
        }
    }
}