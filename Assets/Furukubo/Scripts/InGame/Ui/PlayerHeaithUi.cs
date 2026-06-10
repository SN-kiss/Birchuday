using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InGame.Ui
{
    /// <summary>
    /// Furukubo
    /// </summary>
    public class PlayerHeaithUi : MonoBehaviour
    {
        [Header("Parameters")]
        [SerializeField] private Color _colorActive;
        [SerializeField] private Color _colorPassive;

        [Header("References")]
        [SerializeField] List<Image> _imgHealth;

        public void SetAmounts(int amount, int amountMax)
        {
            if (_imgHealth == null) return;

            for (int i = _imgHealth.Count - 1; 0 <= i; i--)
            {
                Image img = _imgHealth[i];

                if(img == null) continue;

                img.gameObject.SetActive(i <= amountMax - 1);
                img.color = (amount - 1 < i) ? _colorPassive : _colorActive;
            }
        }
    }
}