using System.Collections;
using UnityEngine;

namespace InGame
{
    public class GOD_Fade : MonoBehaviour
    {
        [SerializeField] private Animation _animation;

        public IEnumerator WaitForEndOfAnimationCoroutine()
        {
            _animation.Play();
            yield return new WaitForSeconds(_animation.clip.length);
        }
    }
}