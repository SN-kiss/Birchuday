using UnityEngine;

namespace InGame.Gimmick
{
    /// <summary>
    /// Furukubo
    /// </summary>
    public class Door : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private float _openSpeed;
        [SerializeField] private float _openRadius;

        [Header("References")]
        [SerializeField] private AnimationCurve _openCurve;
        [SerializeField] private Transform _doorADefaultPointTr;
        [SerializeField] private Transform _doorBDefaultPointTr;
        [SerializeField] private Rigidbody2D _DoorARb;
        [SerializeField] private Rigidbody2D _DoorBRb;

        private float _time;

        public void OnOpenUpdate()
        {
            if (1f <= _time) return;
            SetDoors(Mathf.Clamp01(_time + _openSpeed * Time.fixedDeltaTime));
        }

        public void OnCloseUpdate()
        {
            if (_time <= 0f) return;
            SetDoors(Mathf.Clamp01(_time - _openSpeed * Time.fixedDeltaTime));
        }

        private void SetDoors(float time)
        {
            _time = time;

            float curvedTime = _openCurve?.Evaluate(time) ?? 0f;

            Vector2 dir = CalculateUtilities.AngleToDirection(transform.localEulerAngles.z);

            Vector2 aDefPos = _doorADefaultPointTr.position;
            Vector2 aPos = Vector2.Lerp(aDefPos, aDefPos + dir * _openRadius, curvedTime);
            _DoorARb.MovePosition(aPos);

            Vector2 bDefPos = _doorBDefaultPointTr.position;
            Vector2 bPos = Vector2.Lerp(bDefPos, bDefPos - dir * _openRadius, curvedTime);
            _DoorBRb.MovePosition(bPos);
        }
    }
}