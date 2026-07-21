using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class GOD_LocalizedImage : MonoBehaviour
{
    [SerializeField] private Sprite enSprite;
    [SerializeField] private Sprite jpSprite;
    private Image img;

    void Awake() => img = GetComponent<Image>();

    void OnEnable()
    {
        var manager = GOD_LanguageManager.Instance;
        if (manager == null) return; // Ç‹Çæë∂ç›ÇµÇ»Ç¢èÍçáÇÕâΩÇ‡ÇµÇ»Ç¢

        Apply(manager.CurrentLanguage);
        manager.OnLanguageChanged += Apply;
    }

    void OnDisable()
    {
        if (GOD_LanguageManager.Instance != null)
            GOD_LanguageManager.Instance.OnLanguageChanged -= Apply;
    }

    void Apply(GOD_Language lang)
    {
        img.sprite = (lang == GOD_Language.JP) ? jpSprite : enSprite;
    }
}