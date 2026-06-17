using System.Collections;
using UnityEngine;

namespace InGame
{
    public class Fade : MonoBehaviour
    {
        [SerializeField] private string _stateName;
        [SerializeField] private Animator _targetAnim;

        public IEnumerator WaitForEndOfAnimationCoroutine()
        {
            _targetAnim.gameObject.SetActive(true);
            _targetAnim.Play(_stateName);
            yield return new WaitUntil(() => _targetAnim.GetCurrentAnimatorStateInfo(0).IsName(_stateName));
            yield return new WaitUntil(() => 1f <= _targetAnim.GetCurrentAnimatorStateInfo(0).normalizedTime);
        }
}
}