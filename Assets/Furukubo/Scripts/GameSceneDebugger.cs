using TMPro;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    [SerializeField] private string _animStateName;

    [Header("References")]
    [SerializeField] private GameObject _objDebugMessageBox;
    [SerializeField] private TextMeshProUGUI _tmpFps;
    [SerializeField] private TextMeshProUGUI _tmpSceneName;
    [SerializeField] private TextMeshProUGUI _tmpPlayCount;
    [SerializeField] private TextMeshProUGUI _tmpClearCount;
    [SerializeField] private TextMeshProUGUI _tmpMiss1Count;
    [SerializeField] private TextMeshProUGUI _tmpMiss2Count;
    [SerializeField] private TextMeshProUGUI _tmpMiss3Count;
    [SerializeField] private Animator _anim;
    [SerializeField] private Image _img;
    [SerializeField] private TextMeshProUGUI _text;

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
        if (IsEnableToTransitionStages())
        {
            SetStageEntryInfo(StageEntryState.First);
            LoadScene(_stage01SceneName);
        }
    }

    public void Debug5()
    {
        if (!IsBoxActive) return;
        if (IsEnableToTransitionStages())
        {
            SetStageEntryInfo(StageEntryState.First);
            LoadScene(_stage02SceneName);
        }
    }

    public void Debug6()
    {
        if (!IsBoxActive) return;

        if (IsEnableToTransitionStages())
        {
            SetStageEntryInfo(StageEntryState.First);
            LoadScene(_stage03SceneName);
        }
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

        if (_text != null) _text.text = $"Welcome to '{sceneName}'!!!";
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

        if (_objDebugMessageBox.activeSelf)
        {
            if (_anim != null) _anim.Play(_animStateName);

            if (_img != null)
            {
                if (Random.Range(0, 5) == 0)
                {
                    _img.color = Color.HSVToRGB(Random.value, 1f, 1f);

                    if (_text != null) _text.text = SuperHappyDebugTimeText();
            
                }
                else
                {
                    _img.color = Color.white;
                    if (_text != null) _text.text = "Normal Debug Time...";
                }
            }
        }
    }

    private bool IsEnableToTransitionStages()
    {
        if (GOD_PlayerData.Instance == null)
        {
            Debug.Log(0);
            return false;
        }

        if(GOD_PlayerData.Instance.Slots == null)
        {
            Debug.Log(1);
            return false;
        }

        if (GOD_PlayerData.Instance.Slots.Length < 2)
        {
            Debug.Log(2);
            return false;
        }

        var slot0 = GOD_PlayerData.Instance.Slots[0];
        if (slot0.CharacterPrefab == null || slot0.Device == null)
        {
            Debug.Log(3);
            return false;
        }

        var slot1 = GOD_PlayerData.Instance.Slots[1];
        if (slot1.CharacterPrefab == null || slot1.Device == null)
        {
            Debug.Log(4);
            return false;
        }

        return true;
    }

    private string SuperHappyDebugTimeText()
    {
        return
            "<color=#FF0000>S</color>" +
            "<color=#FF7700>U</color>" +
            "<color=#FFFF00>P</color>" +
            "<color=#77FF00>E</color>" +
            "<color=#00FF00>R</color>" +
            " " +
            "<color=#00FF77>H</color>" +
            "<color=#00FFFF>A</color>" +
            "<color=#0077FF>P</color>" +
            "<color=#0000FF>P</color>" +
            "<color=#7700FF>Y</color>" +
            " " +
            "<color=#FF00FF>D</color>" +
            "<color=#FF0077>E</color>" +
            "<color=#FF0000>B</color>" +
            "<color=#FF7700>U</color>" +
            "<color=#FFFF00>G</color>" +
            " " +
            "<color=#77FF00>T</color>" +
            "<color=#00FF00>I</color>" +
            "<color=#00FF77>M</color>" +
            "<color=#00FFFF>E</color>" +
            "<color=#0077FF>!</color>" +
            "<color=#0000FF>!</color>" +
            "<color=#7700FF>!</color>";
    }
}