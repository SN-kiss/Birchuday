using UnityEngine;
using UnityEngine.Events;

namespace InGame.Gimmick
{
    public class PressurePlate : MonoBehaviour
    {
        [SerializeField] private float _plateDownOffset;
        [SerializeField] private Transform _textureTr;

        public event UnityAction OnPressed;
        public event UnityAction OnPressing;
        public event UnityAction OnReleased;
        public event UnityAction OnReleasing;

        private bool _curPressing;
        private bool _oldPressing;

        private void FixedUpdate()
        {
            if (_curPressing == _oldPressing)
            {
                if (_curPressing)
                {
                    Debug.Log("Pressing");
                    OnPressing?.Invoke();
                    _textureTr.localPosition = Vector3.down * _plateDownOffset;
                }
                else
                {
                    Debug.Log("Releasing");
                    OnReleasing?.Invoke();
                    _textureTr.localPosition = Vector3.zero;
                }
            }
            else
            {
                _oldPressing = _curPressing;

                if (_curPressing)
                {
                    Debug.Log("Pressed");
                    OnPressed?.Invoke();
                    _textureTr.localPosition = Vector3.down * _plateDownOffset;
                }
                else
                {
                    Debug.Log("Released");
                    OnReleased?.Invoke();
                    _textureTr.localPosition = Vector3.zero;
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            _curPressing = true;
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            _curPressing = true;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            _curPressing = false;
        }
    }
}