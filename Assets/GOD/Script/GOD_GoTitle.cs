using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class GOD_GoTitle : MonoBehaviour
{
    [SerializeField] private string sceneName = "Title";

    void Update()
    {
        if (Gamepad.current != null && Gamepad.current.buttonEast.wasPressedThisFrame)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
