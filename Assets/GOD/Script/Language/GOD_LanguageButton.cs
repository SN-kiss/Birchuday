using UnityEngine;

public class GOD_LanguageButton : MonoBehaviour
{
    [SerializeField] private GOD_Language targetLanguage;

    public void OnClick()
    {
        if (GOD_LanguageManager.Instance == null)
        {
            Debug.LogWarning("GOD_LanguageManager.Instance is null");
            return;
        }
        GOD_LanguageManager.Instance.SetLanguage(targetLanguage);
    }
}