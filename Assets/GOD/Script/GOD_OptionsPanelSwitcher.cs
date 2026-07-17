using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

//GOD
//OptionのSettingsとCreditsを切り替えるやつ

public class GOD_OptionsPanelSwitcher : MonoBehaviour
{
    [System.Serializable]
    private class Tab
    {
        public GameObject button; // 例: Button_Setings, Button_Credits
        public GameObject panel;  // 例: Settings, Credits

        [HideInInspector] public RectTransform buttonRect;
        [HideInInspector] public Vector2 buttonBasePos;
        [HideInInspector] public RectTransform panelRect;
        [HideInInspector] public Vector2 panelBasePos;
        [HideInInspector] public CanvasGroup panelGroup;
    }

    [SerializeField] private Tab[] tabs;
    [SerializeField] private GameObject defaultButton; // シーン開始時に選択させたいボタン
    [SerializeField] private float unselectedOffsetX = 20f; // 非選択時に右へずらす量
    [SerializeField] private float moveDuration = 0.15f;    // 移動アニメの時間（0で即時）

    private GameObject _lastSelected;

    private void Awake()
    {
        foreach (var tab in tabs)
        {
            tab.buttonRect = tab.button.GetComponent<RectTransform>();
            tab.buttonBasePos = tab.buttonRect.anchoredPosition;

            if (tab.panel != null)
            {
                tab.panel.SetActive(true); // 常に表示。操作可否はCanvasGroupで制御する

                tab.panelRect = tab.panel.GetComponent<RectTransform>();
                tab.panelBasePos = tab.panelRect.anchoredPosition;

                tab.panelGroup = tab.panel.GetComponent<CanvasGroup>();
                if (tab.panelGroup == null)
                    tab.panelGroup = tab.panel.AddComponent<CanvasGroup>();
            }
        }
    }

    private void OnEnable()
    {
        _lastSelected = null;

        if (defaultButton != null)
        {
            EventSystem.current.SetSelectedGameObject(defaultButton);
        }

        ApplySelection(defaultButton, immediate: true);
    }

    private void Update()
    {
        GameObject current = EventSystem.current.currentSelectedGameObject;

        if (current == _lastSelected) return;
        _lastSelected = current;

        ApplySelection(current, immediate: false);
    }

    private void ApplySelection(GameObject selected, bool immediate)
    {
        foreach (var tab in tabs)
        {
            bool isSelected = IsTabSelected(tab, selected);

            if (tab.panelGroup != null)
            {
                tab.panelGroup.interactable = isSelected;
                tab.panelGroup.blocksRaycasts = isSelected;
            }

            Vector2 buttonTarget = isSelected
                ? tab.buttonBasePos
                : tab.buttonBasePos + new Vector2(unselectedOffsetX, 0f);

            if (immediate || moveDuration <= 0f)
            {
                tab.buttonRect.anchoredPosition = buttonTarget;
            }
            else
            {
                StartCoroutine(MoveTo(tab.buttonRect, buttonTarget, moveDuration));
            }

            if (tab.panelRect != null)
            {
                Vector2 panelTarget = isSelected
                    ? tab.panelBasePos
                    : tab.panelBasePos + new Vector2(unselectedOffsetX, 0f);

                if (immediate || moveDuration <= 0f)
                {
                    tab.panelRect.anchoredPosition = panelTarget;
                }
                else
                {
                    StartCoroutine(MoveTo(tab.panelRect, panelTarget, moveDuration));
                }
            }
        }

        UpdateSiblingOrder(selected);
    }

    /// <summary>
    /// 最前面から順に「選択中ボタン→選択中パネル→非選択ボタン→非選択パネル」になるよう並べ替える。
    /// SetAsLastSibling()は呼んだ順に手前へ来るので、後ろにしたいものから先に呼ぶ。
    /// </summary>
    private void UpdateSiblingOrder(GameObject selected)
    {
        // 背面: 非選択パネル → 非選択ボタン
        foreach (var tab in tabs)
        {
            if (IsTabSelected(tab, selected)) continue;

            if (tab.panelRect != null) tab.panelRect.SetAsLastSibling();
            tab.buttonRect.SetAsLastSibling();
        }

        // 前面: 選択中パネル → 選択中ボタン
        foreach (var tab in tabs)
        {
            if (!IsTabSelected(tab, selected)) continue;

            if (tab.panelRect != null) tab.panelRect.SetAsLastSibling();
            tab.buttonRect.SetAsLastSibling();
        }
    }

    private bool IsTabSelected(Tab tab, GameObject selected)
    {
        return selected == tab.button
            || (tab.panel != null && selected != null && selected.transform.IsChildOf(tab.panel.transform));
    }

    private IEnumerator MoveTo(RectTransform rect, Vector2 target, float duration)
    {
        Vector2 start = rect.anchoredPosition;
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            rect.anchoredPosition = Vector2.Lerp(start, target, t / duration);
            yield return null;
        }
        rect.anchoredPosition = target;
    }
}