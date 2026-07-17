using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class GOD_SubmitToSelect : MonoBehaviour, ISubmitHandler
{
    [SerializeField] private Selectable target;

    public void OnSubmit(BaseEventData eventData)
    {
        if (target != null)
        {
            target.Select();
        }
    }
}