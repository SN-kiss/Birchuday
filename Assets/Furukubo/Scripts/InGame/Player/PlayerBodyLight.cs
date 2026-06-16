using UnityEngine;

namespace InGame.Player
{
    /// <summary>
    /// Furukubo
    /// </summary>
    public class PlayerBodyLight : MonoBehaviour
    {
        [Header("Parameters")]
        [SerializeField] float _keepAngle;

        [Header("References")]
        [SerializeField] private Transform _trLight;

        private void Update()
        {
            _trLight.eulerAngles = new Vector3(0f, 0f, 0f);
        }
    }
}