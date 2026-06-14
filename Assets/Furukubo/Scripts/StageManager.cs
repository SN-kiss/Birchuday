using UnityEngine;
using UnityEngine.Events;

namespace InGame
{
    /// <summary>
    /// Furukubo
    /// </summary>
    public class StageManager : MonoBehaviour
    {
        [SerializeField] private UnityEvent _onStageClear;
        [SerializeField] private UnityEvent _onMiss;

        private bool _isStageClear;
        private bool _isMiss;

        public void OnClearStage()
        {
            if (_isStageClear || _isMiss) return;

            Debug.Log("Clear Stage!");

            _isStageClear = true;

            _onStageClear?.Invoke();
        }

        public void OnMissStage()
        {
            if (_isStageClear || _isMiss) return;

            Debug.Log("Clear Stage!");

            _isMiss = true;

            _onMiss?.Invoke();
        }
    }
}