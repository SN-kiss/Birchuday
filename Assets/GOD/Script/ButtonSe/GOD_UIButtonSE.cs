using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class GOD_UIButtonSE : MonoBehaviour, ISelectHandler, ISubmitHandler, IPointerClickHandler
{
    private Coroutine _selectSeRoutine;
    private bool _suppressSelect;

    public void OnSelect(BaseEventData eventData)
    {
        _suppressSelect = false;

        if (_selectSeRoutine != null)
            StopCoroutine(_selectSeRoutine);

        _selectSeRoutine = StartCoroutine(PlaySelectDelayed());
    }

    public void OnSubmit(BaseEventData eventData)
    {
        _suppressSelect = true;
        GOD_SEPlayer.Instance?.PlayClick();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _suppressSelect = true;
        GOD_SEPlayer.Instance?.PlayClick();
    }

    private IEnumerator PlaySelectDelayed()
    {
        // “¯ˆêƒtƒŒ[ƒ€“à‚ÅOnSubmit/OnPointerClick‚ª—ˆ‚é‚©‘Ò‚Â
        yield return null;

        if (!_suppressSelect)
        {
            GOD_SEPlayer.Instance?.PlaySelect();
        }
    }
}