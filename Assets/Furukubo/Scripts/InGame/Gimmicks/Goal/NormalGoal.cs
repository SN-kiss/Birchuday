using UnityEngine;
using UnityEngine.Events;

namespace InGame
{
    /// <summary>
    /// Furukubo
    /// </summary>
    public class NormalGoal : MonoBehaviour
    {
        [SerializeField] private UnityEvent _onGoalPlayerNorth;
        [SerializeField] private UnityEvent _onGoalPlayerSouth;

        private bool _isGoaledPlayerNorth;
        private bool _isGoaledPlayerSouth;

        public void OnGoalPlayerNorth()
        {
            if (_isGoaledPlayerNorth) return;
            _isGoaledPlayerNorth = true;

            Debug.Log("Player North Goaled!");

            _onGoalPlayerNorth?.Invoke();
        }

        public void OnGoalPlayerSouth()
        {
            if (_isGoaledPlayerSouth) return;
            _isGoaledPlayerSouth = true;

            Debug.Log("Player South Goaled!");

            _onGoalPlayerSouth?.Invoke();
        }
    }
}