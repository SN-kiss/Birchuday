using UnityEngine;
using UnityEngine.Events;

namespace InGame.Gimmick
{
    /// <summary>
    /// Furukubo
    /// </summary>
    public class FinalGoal : MonoBehaviour
    {
        [SerializeField] private UnityEvent _onGoal;
        [SerializeField] private ParticleSystem _ps;

        private bool _isGoaled;

        public void OnGoal()
        {
            if (_isGoaled) return;

            _isGoaled = true;

            Debug.Log("Goaled!!!");

            _ps.Play(true);

            _onGoal?.Invoke();
        }
    }
}