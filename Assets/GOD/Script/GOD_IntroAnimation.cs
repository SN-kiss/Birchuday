using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroAnimation : MonoBehaviour
{
    private static HashSet<string> _playedScenes = new HashSet<string>();

    private void Start()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        if (_playedScenes.Contains(sceneName)) { gameObject.SetActive(false); return; }
        _playedScenes.Add(sceneName);
        // アニメーション再生
    }
}