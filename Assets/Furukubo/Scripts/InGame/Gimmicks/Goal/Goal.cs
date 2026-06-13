using UnityEngine;
using UnityEngine.Events;

namespace InGame.Gimmick
{
    public class Goal : MonoBehaviour
    {
        [SerializeField] private UnityEvent _onGoal;

        private bool _isGoaled;

        public void OnGoal()
        {
            if (_isGoaled) return;

            _isGoaled = true;

            Debug.Log("Goaled!!!");

            _onGoal?.Invoke();
        }
    }
}