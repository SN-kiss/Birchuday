using UnityEngine;
using UnityEngine.EventSystems;

//GOD
//OptionのSettingsとCreditsの切り替えのやつ

public class GOD_OptionsPanelSwitcher : MonoBehaviour
{
    [System.Serializable]
    private class Tab
    {
        public GameObject button; // 例: Button_Setings, Button_Credits
        public GameObject panel;  // 例: Settings, Credits
    }

    [SerializeField] private Tab[] tabs;
    [SerializeField] private GameObject defaultButton; // シーン開始時に選択させたいボタン

    private GameObject _lastSelected;

    private void OnEnable()
    {
        _lastSelected = null;

        if (defaultButton != null)
        {
            EventSystem.current.SetSelectedGameObject(defaultButton);
        }

        ApplySelection(defaultButton);
    }

    private void Update()
    {
        GameObject current = EventSystem.current.currentSelectedGameObject;

        if (current == _lastSelected) return;
        _lastSelected = current;

        ApplySelection(current);
    }

    private void ApplySelection(GameObject selected)
    {
        foreach (var tab in tabs)
        {
            if (tab.panel == null) continue;
            tab.panel.SetActive(selected == tab.button);
        }
    }
}