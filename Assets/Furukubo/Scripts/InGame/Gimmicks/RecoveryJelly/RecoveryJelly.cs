using System.Collections.Generic;
using UnityEngine;

namespace InGame.Gimmick
{
    public class RecoveryJelly : MonoBehaviour, ILipAttachTarget
    {
        [Header("References")]
        [SerializeField, Min(0f)] private int _recoveryAmount;
        [SerializeField, Min(1f)] private int _recoveryCountMax;
        [SerializeField, Range(0f, 1f)] private float _scaleMin;

        [Header("References")]
        [SerializeField] private Collider2D _col;
        [SerializeField] private SpringyScaleAnimation _scaleAnim;

        public virtual Vector2 Position => transform.position;
        public MagneticType MagneticType => MagneticType.Both;

        private float Rotation => transform.localEulerAngles.z;

        private List<ILip> _attachingLips;
        private int _recoveryCount;

        private void Start()
        {
            _attachingLips = new List<ILip>();
            _recoveryCount = _recoveryCountMax;
        }

        public virtual void OnAttached(ILip lip)
        {
            _attachingLips?.Add(lip);
            _scaleAnim.OnSpring();
        }

        public virtual void OnDetached(ILip lip)
        {
            if (lip.TryRecover(_recoveryAmount))
            {
                _recoveryCount--;

                if (_recoveryCount <= 0)
                {
                    DetachAll();
                    gameObject.SetActive(false);
                }
                else
                {
                    _attachingLips?.Remove(lip);
                    _scaleAnim.OnSpring();

                    float scale = Mathf.Lerp(_scaleMin, 1f, (float)_recoveryCount / _recoveryCountMax);
                    transform.localScale = new Vector3(scale, scale, 1f);
                }
            }
            else
            {
                _attachingLips?.Remove(lip);
                _scaleAnim.OnSpring();
            }
        }

        public virtual void AddForce(Vector2 force) { }
        public virtual void AddForceImpulse(Vector2 force) { }
        public virtual void AddTorque(float torque) { }
        public virtual void AddTorqueImpulse(float torque) { }

        public virtual Vector2 GetAttachPoint(Vector2 pos) => _col.ClosestPoint(pos);
        public virtual float GetAttachRotation(Vector2 pos) => CalculateUtilities.DirectionToAngle((_col.ClosestPoint(pos) - pos).normalized);

        public virtual Vector2 GetInverseTransformPoint(Vector2 pos) => transform.InverseTransformPoint(pos);
        public virtual Vector2 GetTransformPoint(Vector2 pos) => transform.TransformPoint(pos);

        public virtual float GetInverseTransformRotation(float rot) => Mathf.DeltaAngle(Rotation, rot);
        public virtual float GetTransformRotation(float rot) => Rotation + rot;

        private void DetachAll()
        {
            foreach (var lip in _attachingLips)
            {
                if(lip == null) continue;
                lip.OnDetachFromOther();
            }

            _attachingLips.Clear();
        }
    }
}