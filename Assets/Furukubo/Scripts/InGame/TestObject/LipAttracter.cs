using InGame.Player;
using UnityEngine;

namespace InGame
{
    /// <summary>
    /// Furukubo (Refactoring of SunctionZone)
    /// </summary>
    public class LipAttracter : MonoBehaviour
    {
        [SerializeField] LipConnecter _connecter;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out PlayerLipControler lip))
            {
                if (_connecter == null) return;
                lip.AttractedStateEnter(_connecter);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.TryGetComponent(out PlayerLipControler lip))
            {
                lip.Detach();
            }
        }
    }
}