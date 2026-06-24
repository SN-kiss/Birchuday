using UnityEngine;
using UnityEngine.SceneManagement;

public class GOD_TransitionManager : MonoBehaviour
{
    public static GOD_TransitionManager Instance { get; private set; }

    [SerializeField] private Animator animator;
    [SerializeField] private string _nextSceneName; // Inspector用デフォルト値

    private string nextSceneName; // 実行時に使う

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void GoToScene(string sceneName)
    {
        nextSceneName = sceneName;
        animator.SetTrigger("Start");
    }

    // テスト用：InspectorのボタンやAnimation Eventから_nextSceneNameで遷移
    public void GoToDefaultScene()
    {
        GoToScene(_nextSceneName);
    }

    // Animation Event：画面が完全に隠れたフレームに設定
    private void OnTransitionMidpoint()
    {
        if (string.IsNullOrEmpty(nextSceneName)) return; // ← 追加
        SceneManager.LoadScene(nextSceneName);
    }
}