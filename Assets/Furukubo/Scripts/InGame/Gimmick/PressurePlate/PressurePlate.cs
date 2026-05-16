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
        [SerializeField] private UnityEvent _onPressed;
        [SerializeField] private UnityEvent _onPressing;
        [SerializeField] private UnityEvent _onReleased;
        [SerializeField] private UnityEvent _onReleasing;
        [SerializeField] private Transform _textureTr;

        private bool _curPressing;
        private bool _oldPressing;

        private void Update()
        {
            _textureTr.localPosition = Vector3.up * (_curPressing ? _platePressingPoint : _plateReleasingPoint);
        }

        private void FixedUpdate()
        {
            if (_curPressing == _oldPressing)
            {
                if (_curPressing)
                {
                    _onPressing?.Invoke();
                }
                else
                {
                    _onReleasing?.Invoke();
                }
            }
            else
            {
                _oldPressing = _curPressing;

                if (_curPressing)
                {
                    _onPressed?.Invoke();
                }
                else
                {
                    _onReleased?.Invoke();
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log(collision);
            _curPressing = true;
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            Debug.Log(collision);
            _curPressing = true;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            Debug.Log(collision);
            _curPressing = false;
        }
    }
}