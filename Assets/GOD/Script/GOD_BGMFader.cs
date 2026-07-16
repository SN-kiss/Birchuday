using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

namespace InGame.Audio
{
    public class GOD_BGMFader : MonoBehaviour
    {
        [SerializeField] private AudioMixer _audioMixer;
        [SerializeField] private string _volumeParam = "BGMVolume";
        [SerializeField] private float _defaultVolumeDb = 0f;

        private Coroutine _fadeCoroutine;

        private void Awake()
        {
            // シーンに入ったタイミングでBGM音量を元に戻す
            _audioMixer.SetFloat(_volumeParam, _defaultVolumeDb);
        }

        public void FadeOutBGM()
        {
            FadeOutBGM(3f);
        }

        public void FadeOutBGM(float duration)
        {
            if (_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);
            _fadeCoroutine = StartCoroutine(FadeRoutine(duration, -80f));
        }

        public void FadeInBGM(float duration = 1f)
        {
            if (_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);
            _fadeCoroutine = StartCoroutine(FadeRoutine(duration, _defaultVolumeDb));
        }

        private IEnumerator FadeRoutine(float duration, float targetDb)
        {
            _audioMixer.GetFloat(_volumeParam, out float startDb);
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                float currentDb = Mathf.Lerp(startDb, targetDb, t);
                _audioMixer.SetFloat(_volumeParam, currentDb);
                yield return null;
            }

            _audioMixer.SetFloat(_volumeParam, targetDb);
        }
    }
}