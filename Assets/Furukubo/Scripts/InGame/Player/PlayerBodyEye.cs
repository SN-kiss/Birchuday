using UnityEngine;

namespace InGame.Player
{
    public class PlayerBodyEye : MonoBehaviour
    {
        [SerializeField] private float _scaleMin;
        [SerializeField] private float _scaleMax;
        [SerializeField] private float _defaultDistance;
        [SerializeField] private float _distanceCoef;
        [SerializeField] private Transform _trEye;
        [SerializeField] private Transform _trBody;
        [SerializeField] private Transform _trLip;

        private void Update()
        {
            float overDistance = 
                Mathf.Clamp(
                    (_trBody.position - _trLip.position).magnitude - _defaultDistance,
                    0f,
                    float.MaxValue);

            float clamped = Mathf.Clamp(overDistance * _distanceCoef + _scaleMin, _scaleMin, _scaleMax);

            _trEye.localScale = new Vector3(clamped, 1f, 1f);
        }
    }
}