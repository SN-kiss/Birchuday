using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


[RequireComponent(typeof(Slider))]
public class GOD_ControllerAdjustableSlider : Slider, ISubmitHandler
{
    [Header("編集モード")]
    [SerializeField] private float stepSize = 0.05f; // 1回の入力での増減量（Sliderのvalue単位）

    [Header("非編集時に左で戻る先（例: Settingsボタン）")]
    [SerializeField] private Selectable leftNavigationTarget;

    [Header("見た目（任意）")]
    [SerializeField] private GameObject editingIndicator; // 編集中だけ表示したい枠やアイコンがあれば割り当て

    private bool _isEditing;

    public void OnSubmit(BaseEventData eventData)
    {
        _isEditing = !_isEditing;

        if (editingIndicator != null)
        {
            editingIndicator.SetActive(_isEditing);
        }
    }

    public override void OnMove(AxisEventData eventData)
    {
        if (_isEditing)
        {
            switch (eventData.moveDir)
            {
                case MoveDirection.Left:
                    value -= stepSize;
                    eventData.Use();
                    return;

                case MoveDirection.Right:
                    value += stepSize;
                    eventData.Use();
                    return;

                case MoveDirection.Up:
                case MoveDirection.Down:
                    // 編集中はタブ間・スライダー間の移動をさせない
                    eventData.Use();
                    return;
            }
        }
        else
        {
            switch (eventData.moveDir)
            {
                case MoveDirection.Left:
                    if (leftNavigationTarget != null)
                    {
                        leftNavigationTarget.Select();
                    }
                    eventData.Use();
                    return;

                case MoveDirection.Right:
                    // 非編集時に右へは何もしない
                    eventData.Use();
                    return;
            }
        }

        // 非編集時の上下だけはExplicit Navigationに任せる
        base.OnMove(eventData);
    }
}