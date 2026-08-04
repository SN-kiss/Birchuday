using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GOD_GameStart : MonoBehaviour
{
   public void OnGameStart()
    {
        SceneManager.LoadScene("Link");
    }
}
