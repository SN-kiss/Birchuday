using UnityEngine;
using UnityEngine.Events;

namespace InGame.Gimmick
{
    public class PressurePlate : MonoBehaviour
    {
        [SerializeField] private float _plateDownOffset;
        [SerializeField] private Transform _textureTr;

        public event UnityAction OnPressedEvent;
        public event UnityAction OnPressingEvent;
        public event UnityAction OnReleasedEvent;
        public event UnityAction OnReleasingEvent;

        private bool _pressed;

        private void Update()
        {
            if (!_pressed)
            {
                OnPressingEvent?.Invoke();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            _pressed = true;
            _textureTr.localPosition = Vector3.down * _plateDownOffset;
            OnPressedEvent?.Invoke();
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            _pressed = true;
            _textureTr.localPosition = Vector3.down * _plateDownOffset;
            OnPressingEvent?.Invoke();
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            _pressed = false;
            _textureTr.localPosition = Vector3.zero;
            OnReleasedEvent?.Invoke();
        }
    }
}