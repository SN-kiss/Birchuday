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
        [SerializeField] private Animator _anim;

        private bool _isGoaled;

        public void OnGoal()
        {
            if (_isGoaled) return;

            _isGoaled = true;

            Debug.Log("Goaled!!!");

            _anim.gameObject.SetActive(true);
            _anim.Play("Start");

            _onGoal?.Invoke();
        }
    }
}