using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class GOD_ControllerAdjustableSlider : Slider, ISubmitHandler
{
    [Header("編集モード")]
    [SerializeField] private float stepSize = 0.05f;
    [Header("非編集時に左で戻る先（例: Settingsボタン）")]
    [SerializeField] private Selectable leftNavigationTarget;
    [Header("見た目（任意）")]
    [SerializeField] private GameObject editingIndicator;

    private bool _isEditing;

    public void OnSubmit(BaseEventData eventData)
    {
        // スライダーが選択されている状態でAが押された＝編集モードのトグル
        SetEditing(!_isEditing);

        // 編集モードを抜けた時は元のボタンに選択を戻す
        if (!_isEditing && leftNavigationTarget != null)
        {
            leftNavigationTarget.Select();
        }
    }

    // 外部（BGMボタン側）からも呼べるように公開メソッド化
    public void SetEditing(bool editing)
    {
        _isEditing = editing;
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
                    eventData.Use();
                    return;
            }
        }
        base.OnMove(eventData);
    }
}