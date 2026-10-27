using UnityEngine;
using UnityEngine.SceneManagement;

public class GOD_GoNextScenes : MonoBehaviour
{
    [SerializeField] private string sceneName = "";

    public void OnGameStart()
    {
        SceneManager.LoadScene(sceneName);
    }
}
