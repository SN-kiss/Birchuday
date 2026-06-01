using UnityEngine;

namespace InGame
{
    /// <summary>
    /// Furukubo
    /// </summary>
    public class RailTracer : MonoBehaviour
    {
        [Header("Parameters")]
        [SerializeField] private float _speed;

        [Header("References")]
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private Rail _rail;

        private int _index;
        private float _time;

        private void FixedUpdate()
        {
            if (_rail == null) return;

            float sectionDistance = _rail.GetSectionDistance(_index);
            _time += Time.fixedDeltaTime * (sectionDistance == 0f ? _speed : _speed / sectionDistance);

            if (1f <= _time)
            {
                _time = 0f;
                _index++;
            }

            _rb.MovePosition(_rail.GetPointInSection(_index, _time));
        }
    }
}