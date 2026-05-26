using UnityEngine;

/// <summary>
/// Furukubo
/// </summary>
public class GameManager : MonoBehaviour
{
    public const int Fps = 60;

    public static GameManager Instance => s_instance;

    private static GameManager s_instance;

    private void Awake()
    {
        if (s_instance == null)
        {
            s_instance = this;

            DontDestroyOnLoad(gameObject);

            Application.targetFrameRate = Fps;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (s_instance == this)
        {
            s_instance = null;
        }
    }
}
