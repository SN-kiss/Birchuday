using TMPro;
using UnityEngine;

namespace InGame.Ui
{
    /// <summary>
    /// Furukubo
    /// </summary>
    public class PlayerHeaithUi : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TextMeshProUGUI _amountText;
        [SerializeField] private TextMeshProUGUI _amountMaxText;

        public void SetAmounts(int amount, int amountMax)
        {
            _amountText.text = amount.ToString();
            _amountMaxText.text = amountMax.ToString();
        }
    }
}