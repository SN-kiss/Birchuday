using UnityEngine;
using UnityEngine.SceneManagement;


public class GOD_GoDifficulty : MonoBehaviour
{
    public void OnGameStart()
    {
        SceneManager.LoadScene("Difficulty");
    }
}
