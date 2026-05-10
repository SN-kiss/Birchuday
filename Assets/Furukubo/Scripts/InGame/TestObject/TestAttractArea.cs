using InGame.Player;
using System;
using UnityEngine;

namespace InGame
{
    /// <summary>
    /// Furukubo (Refactoring of SunctionZone)
    /// </summary>
    public class TestAttractArea : MonoBehaviour
    {
        public event Func<IAttracter> Attracter;

        private void OnTriggerEnter2D(Collider2D other)
        {
            //dammy
            if (other.TryGetComponent(out PlayerLipControler lip))
            {
                if (Attracter == null) return;
                lip.AttractedStateEnter(Attracter.Invoke());
            }
        }

        /*
        private void OnTriggerExit2D(Collider2D other)
        {
            //dammy
            if (other.TryGetComponent(out PlayerLipControler lip))
            {
                lip.Detach();
            }
        }*/
    }
}