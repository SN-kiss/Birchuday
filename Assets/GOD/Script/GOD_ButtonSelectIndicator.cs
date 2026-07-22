using UnityEngine;
using UnityEngine.EventSystems;

public class GOD_ButtonSelectIndicator : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    [SerializeField] private GameObject targetObject; // 表示したいオブジェクト

    private void Awake()
    {
        // 初期状態は非表示
        if (targetObject != null)
            targetObject.SetActive(false);
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (targetObject != null)
            targetObject.SetActive(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (targetObject != null)
            targetObject.SetActive(false);
    }
}