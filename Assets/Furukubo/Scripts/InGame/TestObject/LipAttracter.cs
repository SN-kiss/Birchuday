using InGame.Player;
using System.Collections.Generic;
using UnityEngine;

namespace InGame
{
    /// <summary>
    /// Furukubo (Refactoring of SunctionZone)
    /// </summary>
    public class LipAttracter : MonoBehaviour
    {
        [SerializeField] LipConnecter _connecter;

        /// <summary>
        /// Dammy
        /// </summary>
        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.TryGetComponent(out PlayerLip lip))
            {
                Vector2 lipPos = lip.Position;
                Vector2 closestPos = _connecter.GetClosestPoint(lipPos);

                Vector2 between = closestPos - lipPos;

                lip.OnAttracted(between * 5f, closestPos, _connecter);
            }
        }
    }
}