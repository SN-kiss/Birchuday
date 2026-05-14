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

        private void FixedUpdate()
        {
            if (!_pressed)
            {
                Debug.Log("Releasing");
                OnReleasingEvent?.Invoke();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log("Pressed");
            _pressed = true;
            _textureTr.localPosition = Vector3.down * _plateDownOffset;
            OnPressedEvent?.Invoke();
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            Debug.Log("Pressing");
            _pressed = true;
            _textureTr.localPosition = Vector3.down * _plateDownOffset;
            OnPressingEvent?.Invoke();
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            Debug.Log("Released");
            _pressed = false;
            _textureTr.localPosition = Vector3.zero;
            OnReleasedEvent?.Invoke();
        }
    }
}