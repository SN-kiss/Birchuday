using System;
using UnityEngine;

namespace InGame
{
    /// <summary>
    /// Furukubo
    /// </summary>
    public class PlayerLeg : MonoBehaviour
    {
        [SerializeField] private Transform _rootTr;
        [SerializeField] private Transform _centerTr;

        private Vector2 _oldCenterPos;

        private void Start()
        {
            if (_centerTr == null) return;

            _oldCenterPos = _centerTr.position;
        }

        private void Update()
        {
            if (_rootTr == null) return;
            if(_centerTr == null) return;

            Vector2 rootPos = _rootTr.position;
            transform.position = rootPos;

            transform.eulerAngles = new Vector3(0f, 0f, OriginalCalculateUtils.DirectionToAngle(_oldCenterPos - rootPos));

            _oldCenterPos = _centerTr.position;
        }
    }
}