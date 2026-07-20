using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class GOD_AnimationSceneLoader : MonoBehaviour
{
    [Header("次のシーン")]
    [Tooltip("Build Settingsに登録されているシーン名を入力")]
    [SerializeField] private string nextSceneName;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        StartCoroutine(WaitForAnimationAndLoadScene());
    }

    private IEnumerator WaitForAnimationAndLoadScene()
    {
        // ステートが切り替わるフレームを待つ（Awake直後は前フレームの情報が残ることがあるため）
        yield return null;

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // normalizedTimeが1.0以上になったら再生終了（ループアニメには使えない点に注意）
        while (stateInfo.normalizedTime < 1f)
        {
            yield return null;
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        }

        LoadNextScene();
    }

    private void LoadNextScene()
    {
        if (string.IsNullOrEmpty(nextSceneName))
        {
            Debug.LogWarning("[GOD_AnimationSceneLoader] 次のシーン名が設定されていません。");
            return;
        }

        SceneManager.LoadScene(nextSceneName);
    }
}