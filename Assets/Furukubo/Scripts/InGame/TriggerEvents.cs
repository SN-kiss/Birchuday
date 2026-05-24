using UnityEngine;
using UnityEngine.Events;

namespace InGame
{
    /// <summary>
    /// Furukubo
    /// </summary>
    public class TriggerEvents : MonoBehaviour
    {
        [SerializeField] private UnityEvent<Collider2D> _onEnter;
        [SerializeField] private UnityEvent<Collider2D> _onStay;
        [SerializeField] private UnityEvent<Collider2D> _onExit;

        private void OnTriggerEnter2D(Collider2D collision) => _onEnter?.Invoke(collision);

        private void OnTriggerStay2D(Collider2D collision) => _onStay?.Invoke(collision);

        private void OnTriggerExit2D(Collider2D collision) => _onExit?.Invoke(collision);
    }
}