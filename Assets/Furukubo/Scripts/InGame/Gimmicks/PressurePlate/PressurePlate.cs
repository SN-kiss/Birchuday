using UnityEngine;
using UnityEngine.Events;

namespace InGame.Gimmick
{
    /// <summary>
    /// Furukubo
    /// </summary>
    public class PressurePlate : MonoBehaviour
    {
        [Header("Parameters")]
        [SerializeField] private float _plateReleasingPoint;
        [SerializeField] private float _platePressingPoint;

        [Header("References")]
        [SerializeField] private UnityEvent _onPressing;
        [SerializeField] private UnityEvent _onReleasing;
        [SerializeField] private Transform _textureTr;

        private bool _isPressing;

        private void Update()
        {
            _textureTr.localPosition = Vector3.up * (_isPressing ? _platePressingPoint : _plateReleasingPoint);
        }

        private void FixedUpdate()
        {
            if (_isPressing)
            {
                _onPressing?.Invoke();
            }
            else
            {
                _onReleasing?.Invoke();
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            _isPressing = true;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            _isPressing = false;
        }
    }
}