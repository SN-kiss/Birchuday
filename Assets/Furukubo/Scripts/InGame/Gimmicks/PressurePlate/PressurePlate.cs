using UnityEngine;
using UnityEngine.Events;

namespace InGame.Gimmick
{
    /// <summary>
    /// Furukubo
    /// </summary>
    public class PressurePlate : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private SpriteRenderer _sr;
        [SerializeField] private Sprite _sprOn;
        [SerializeField] private Sprite _sprOff;
        [SerializeField] private UnityEvent _onPressed;
        [SerializeField] private UnityEvent _onPressing;
        [SerializeField] private UnityEvent _onReleasing;

        private bool _isPressing;

        private void Update()
        {
            _sr.sprite = _isPressing ? _sprOn : _sprOff;
        }

        private void FixedUpdate()
        {
            if (_isPressing)
            {
                Debug.Log("Pressingggggggggggggggggggggggggggggggggg");
                _onPressing?.Invoke();
            }
            else
            {
                Debug.Log("Releasinggggggggggggggggggggggggggggggggg");
                _onReleasing?.Invoke();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_isPressing) return;

            _isPressing = true;

            _onPressed?.Invoke();
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