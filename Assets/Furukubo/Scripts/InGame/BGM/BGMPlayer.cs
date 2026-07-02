using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Furukubo
/// </summary>
public class BGMPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private BGMDataList _dataList;

    public static BGMPlayer Instance { get; private set; }

    private Dictionary<BGMType, AudioClip> _dictionary;
    private BGMType _current = BGMType.None;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            if(_dataList != null) Init(_dataList.GetBGMs());
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    public void SetAndPlayBGM(BGMType type)
    {
        if (_audioSource == null || _dictionary == null)
        {
            Debug.Log($"Failed to Play BGM : {type}");
            return;
        }

        if (_current == type)
        {
            Debug.Log($"You are trying to play same BGM! : {type}");
            return;
        }

        if (_dictionary.TryGetValue(type, out AudioClip audio))
        {
            if (audio == null) return;
            _current = type;
            _audioSource.clip = audio;
            _audioSource.Play();

            Debug.Log($"Completed to Play BGM : {type}");
        }
        else
        {
            Debug.Log($"Failed to Play BGM : {type}");
        }
    }

    public void PauseBGM()
    {
        if (_audioSource == null) _audioSource.Pause();
    }

    public void UnPauseBGM()
    {
        if (_audioSource == null) _audioSource.UnPause();
    }

    private void Init(BGMData[] bgms)
    {
        _dictionary = new Dictionary<BGMType, AudioClip>();

        foreach (BGMData bgm in bgms)
        {
            if(bgm.Clip != null && !_dictionary.ContainsKey(bgm.Type)) _dictionary.Add(bgm.Type, bgm.Clip);
        }
    }
}

public enum BGMType
{
    None,
    Title,
    InGame,
    Result,
}