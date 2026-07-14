using UnityEngine;
using UnityEngine.EventSystems;

public class GOD_ButtonScale : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    private Vector3 defaultScale;

    [SerializeField] private float scale = 1.2f;

    private void Awake()
    {
        defaultScale = transform.localScale;
    }

    public void OnSelect(BaseEventData eventData)
    {
        Debug.Log("Selected!");
        transform.localScale = defaultScale * scale;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        transform.localScale = defaultScale;
    }
}
