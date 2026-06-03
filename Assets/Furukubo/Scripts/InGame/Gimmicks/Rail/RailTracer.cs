using UnityEngine;

namespace InGame
{
    /// <summary>
    /// Furukubo
    /// </summary>
    public class RailTracer : MonoBehaviour
    {
        [Header("Parameters")]
        [SerializeField] private float _ratio;
        [SerializeField] private float _speed;

        [Header("References")]
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private Rail _rail;

        private int CurrentIndex
        {
            get
            {
                if (_rail == null) return 0;
                return Mathf.FloorToInt(_ratio);
            }
        }

        private float CurrentTime => Mathf.Repeat(_ratio, 1f);

        private void FixedUpdate()
        {
            if (_rail == null) return;

            float sectionDistance = _rail.GetSectionDistance(CurrentIndex);

            float speed = sectionDistance == 0f ? _speed : (_speed / sectionDistance);
            float addRatio = Time.fixedDeltaTime * speed;

            _ratio = OriginalCalculateUtils.Loop(0f, _rail.SectionCount, _ratio + addRatio);

            _rb.MovePosition(_rail.GetPointInSection(CurrentIndex, CurrentTime));
        }

        private void OnValidate()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                if (_rail == null) return;

                _ratio = OriginalCalculateUtils.Loop(0f, _rail.SectionCount, _ratio);
                transform.position = _rail.GetPointInSection(CurrentIndex, CurrentTime);
            }
#endif
        }
    }
}