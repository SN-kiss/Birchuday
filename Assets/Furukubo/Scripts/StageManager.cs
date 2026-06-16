using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.AdaptivePerformance.Provider.AdaptivePerformanceSubsystemDescriptor;

namespace InGame
{
    /// <summary>
    /// Furukubo
    /// </summary>
    public class StageManager : MonoBehaviour
    {
        [SerializeField] private UnityEvent _onStageClear;
        [SerializeField] private UnityEvent _onMiss;

        [SerializeField] private GameObject _msgMissPlayerNorth;
        [SerializeField] private GameObject _msgMissPlayerSouth;
        [SerializeField] private GameObject _msgStageClear;
        [SerializeField] private Animator _animFlash;
        [SerializeField] private string _flashAnimStateName;

        private bool _isStageClear;
        private bool _isMiss;

        public void OnClearStage()
        {
            if (_isStageClear || _isMiss) return;

            _isStageClear = true;

            _onStageClear?.Invoke();

            StartCoroutine(ClearStageCoroutine());
            
            IEnumerator ClearStageCoroutine()
            {
                _animFlash.gameObject.SetActive(true);
                _animFlash.Play(_flashAnimStateName);

                yield return new WaitUntil(() => _animFlash.GetCurrentAnimatorStateInfo(0).IsName(_flashAnimStateName));

                yield return new WaitUntil(() => 1f <= _animFlash.GetCurrentAnimatorStateInfo(0).normalizedTime);

                _msgStageClear.SetActive(true);
            }
        }

        public void OnMissPlayerNorth()
        {
            if (_isStageClear || _isMiss) return;

            _isMiss = true;

            _onMiss?.Invoke();

            _msgMissPlayerNorth.gameObject.SetActive(true);
        }

        public void OnMissPlayerSouth()
        {
            if (_isStageClear || _isMiss) return;

            _isMiss = true;

            _onMiss?.Invoke();

            _msgMissPlayerSouth.gameObject.SetActive(true);
        }
    }
}