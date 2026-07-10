using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class ResultScene : MonoBehaviour
{
    [SerializeField] private string _nextSceneName;
    [SerializeField] private GameObject _objWhite;
    [SerializeField] private VideoPlayer _videoPlayer;

    private void Start()
    {
        StartCoroutine(VideoPlayCoroutine());
    }

    private IEnumerator VideoPlayCoroutine()
    {
        if(_videoPlayer == null) yield break;

        yield return null;

        _videoPlayer.Prepare();
        
        yield return new WaitUntil(() => _videoPlayer.isPrepared);

        _videoPlayer.Play();

        yield return new WaitUntil(() => _videoPlayer.isPlaying);

        _objWhite.SetActive(false);

        yield return new WaitUntil(() => !_videoPlayer.isPlaying);

        SceneManager.LoadScene(_nextSceneName);
    }
}