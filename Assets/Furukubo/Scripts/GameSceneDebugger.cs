using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Furukubo
/// </summary>
public class GameSceneDebugger : MonoBehaviour
{
    [Header("Scene names")]
    [SerializeField] private string _titleSceneName;
    [SerializeField] private string _linkSceneName;
    [SerializeField] private string _stage01SceneName;
    [SerializeField] private string _stage02SceneName;
    [SerializeField] private string _stage03SceneName;
    [SerializeField] private string _resultSceneName;

    [Header("References")]
    [SerializeField] private GameObject _objDebugMessageBox;
    [SerializeField] private TextMeshProUGUI _tmpFps;
    [SerializeField] private TextMeshProUGUI _tmpSceneName;
    [SerializeField] private TextMeshProUGUI _tmpPlayCount;
    [SerializeField] private TextMeshProUGUI _tmpClearCount;
    [SerializeField] private TextMeshProUGUI _tmpMiss1Count;
    [SerializeField] private TextMeshProUGUI _tmpMiss2Count;
    [SerializeField] private TextMeshProUGUI _tmpMiss3Count;

    public static GameSceneDebugger Instance { get; private set; }
    private string _textFps;
    private string _textSceneName;
    private int _playCount;
    private int _clearCount;
    private int _miss1Count;
    private int _miss2Count;
    private int _miss3Count;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            SceneManager.activeSceneChanged += OnSceneChanged;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        float dt = Time.deltaTime;
        float fps = dt == 0f ? 0f : 1f / dt;
        _textFps = fps.ToString();

        _textSceneName = SceneManager.GetActiveScene().name;
    }

    private void LateUpdate()
    {
        if (_objDebugMessageBox == null) return;
        if (!_objDebugMessageBox.activeSelf) return;

        if (_tmpFps != null) _tmpFps.text = _textFps;
        if (_tmpSceneName != null) _tmpSceneName.text = _textSceneName;
        if (_tmpPlayCount != null) _tmpPlayCount.text = _playCount.ToString();
        if (_tmpClearCount != null) _tmpClearCount.text = _clearCount.ToString();
        if (_tmpMiss1Count != null) _tmpMiss1Count.text = _miss1Count.ToString();
        if (_tmpMiss2Count != null) _tmpMiss2Count.text = _miss2Count.ToString();
        if (_tmpMiss3Count != null) _tmpMiss3Count.text = _miss3Count.ToString();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
            SceneManager.activeSceneChanged -= OnSceneChanged;
        }
    }

    private string CurrentSceneName => SceneManager.GetActiveScene().name;
    private bool IsBoxActive => _objDebugMessageBox.activeSelf;

    public void Debug1()
    {
        OpenOrCloseBox();
    }

    public void Debug2()
    {
        if (!IsBoxActive) return;
        SetStageEntryInfo(StageEntryState.First);
        LoadScene(_titleSceneName);
    }

    public void Debug3()
    {
        if (!IsBoxActive) return;
        SetStageEntryInfo(StageEntryState.First);
        LoadScene(_linkSceneName);
    }
    
    public void Debug4()
    {
        if (!IsBoxActive) return;
        if (!IsEnableToTransitionStages()) return;
        SetStageEntryInfo(StageEntryState.First);
        LoadScene(_stage01SceneName);
    }

    public void Debug5()
    {
        if (!IsBoxActive) return;
        if (!IsEnableToTransitionStages()) return;
        SetStageEntryInfo(StageEntryState.First);
        LoadScene(_stage02SceneName);
    }

    public void Debug6()
    {
        if (!IsBoxActive) return;
        if (!IsEnableToTransitionStages()) return;
        SetStageEntryInfo(StageEntryState.First);
        LoadScene(_stage03SceneName);
    }

    public void Debug7()
    {
        if (!IsBoxActive) return;
        LoadScene(_resultSceneName);
    }

    public void Debug8()
    {
        if (!IsBoxActive) return;
        LoadScene(CurrentSceneName);
    }

    public void Debug9()
    {
    }

    public void Debug0()
    {
    }

    public void AddPlayCount() => _playCount++;
    public void AddClearCount() => _clearCount++;

    public void AddMissCount()
    {
        string cur = SceneManager.GetActiveScene().name;

        if (cur == _stage01SceneName)
        {
            _miss1Count++;
        }
        else if (cur == _stage02SceneName)
        {
            _miss2Count++;
        }
        else if (cur == _stage03SceneName)
        {
            _miss3Count++;
        }
    }

    private void LoadScene(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName)) return;
        if (!Application.CanStreamedLevelBeLoaded(sceneName)) return;

        SceneManager.LoadScene(sceneName);
        
        Debug.Log($"<color=cyan><b>Game Scene Debbuger : The scene automatically transitioned to \"{sceneName}\"</b></color>");
    }
    
    private void SetStageEntryInfo(StageEntryState state)
    {
        if(StageEntryInfo.Instance != null) StageEntryInfo.Instance.SetEntryState(state);
    }

    private void OnSceneChanged(Scene from, Scene to)
    {
        if(to.name == _linkSceneName)
        {
            _playCount++;
        }
    }

    private void OpenOrCloseBox()
    {
        if (_objDebugMessageBox == null) return;
        _objDebugMessageBox.SetActive(!_objDebugMessageBox.activeSelf);
    }

    private bool IsEnableToTransitionStages()
    {
        if (GOD_PlayerData.Instance == null) return false;

        if(GOD_PlayerData.Instance.Slots == null) return false;

        if (GOD_PlayerData.Instance.Slots.Length < 2) return false;

        var slot0 = GOD_PlayerData.Instance.Slots[0];
        if (slot0.CharacterPrefab == null || slot0.Device == null) return false;

        var slot1 = GOD_PlayerData.Instance.Slots[1];
        if (slot1.CharacterPrefab == null || slot1.Device == null) return false;

        return true;
    }
}