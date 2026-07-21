using System;
using UnityEngine;

public enum GOD_Language { EN, JP }

public class GOD_LanguageManager : MonoBehaviour
{
    public static GOD_LanguageManager Instance { get; private set; }
    public GOD_Language CurrentLanguage { get; private set; } = GOD_Language.EN;
    public event Action<GOD_Language> OnLanguageChanged;

    private const string PrefKey = "GOD_Language";

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        CurrentLanguage = (GOD_Language)PlayerPrefs.GetInt(PrefKey, 0);
    }

    public void SetLanguage(GOD_Language lang)
    {
        if (CurrentLanguage == lang) return;
        CurrentLanguage = lang;
        PlayerPrefs.SetInt(PrefKey, (int)lang);
        PlayerPrefs.Save();
        OnLanguageChanged?.Invoke(lang);
    }
}