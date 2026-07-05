using UnityEngine;
using UnityEngine.SceneManagement;

public class GOD_Bgm : MonoBehaviour
{
    private static GOD_Bgm instance;
    private AudioSource audioSource;

    // BGMÇó¨ÇµÇΩÇ¢ÉVÅ[Éìñº
    [SerializeField] string[] allowedScenes;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        bool playBGM = false;

        foreach (string sceneName in allowedScenes)
        {
            if (scene.name == sceneName)
            {
                playBGM = true;
                break;
            }
        }

        if (playBGM && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
        else if (!playBGM && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}