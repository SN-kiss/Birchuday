using InGame.Player;
using TMPro;
using UnityEngine;

namespace InGame.Gimmick
{
    /// <summary>
    /// Furukubo
    /// </summary>
    public class LipAttachTargetHeavy : MonoBehaviour, ILipAttachTarget
    {
        [Header("Parameters")]
        [SerializeField] private int _attachedCountNeedToMoveMin;

        [Header("References")]
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private Collider2D _col;
        [SerializeField] private TextMeshProUGUI _debugAttachedCountText;

        public Vector2 Position => _rb.position;

        private int _attachedCount;

        private void Awake()
        {
            _debugAttachedCountText.text = $"{_attachedCount}/{_attachedCountNeedToMoveMin}";
        }

        public void OnAttached(ILip attacher)
        {
            _attachedCount++;

            _debugAttachedCountText.text = $"{_attachedCount}/{_attachedCountNeedToMoveMin}";

            _rb.bodyType =
                _attachedCountNeedToMoveMin <= _attachedCount
                ? RigidbodyType2D.Dynamic
                : RigidbodyType2D.Kinematic;
        }

        public void OnDetached(ILip lip)
        {
            _attachedCount--;

            _debugAttachedCountText.text = $"{_attachedCount}/{_attachedCountNeedToMoveMin}";

            _rb.linearVelocity = Vector2.zero;

            _rb.bodyType = 
                _attachedCountNeedToMoveMin <= _attachedCount 
                ? RigidbodyType2D.Dynamic 
                : RigidbodyType2D.Kinematic;
        }

        public void AddForce(Vector2 force)
        {
            if (_attachedCountNeedToMoveMin <= _attachedCount)
            {
                _rb.AddForce(force);
            }
        }

        public Vector2 GetAttachPoint(Vector2 pos) => _col.ClosestPoint(pos);
        public float GetAttachRotation(Vector2 pos) => CalculateUtilities.DirectionToAngle((_rb.position - pos).normalized);

        public Vector2 GetInverseTransformPoint(Vector2 pos) => transform.InverseTransformPoint(pos);
        public Vector2 GetTransformPoint(Vector2 pos) => transform.TransformPoint(pos);

        public float GetInverseTransformRotation(float rot) => _rb.rotation - rot;
        public float GetTransformRotation(float rot) => _rb.rotation + rot;
    }
}