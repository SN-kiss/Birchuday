using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GOD_SubmitToSelect : MonoBehaviour, ISubmitHandler
{
    [SerializeField] private Selectable target;

    public void OnSubmit(BaseEventData eventData)
    {
        if (target == null) return;

        target.Select();

        // targetが編集可能スライダーなら、選択と同時に編集モードもONにする
        if (target is GOD_ControllerAdjustableSlider slider)
        {
            slider.SetEditing(true);
        }
    }
}