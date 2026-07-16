using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/DeviceVibrationData", fileName = "NewDeviceVibrationData")]
public class DeviceVibrationData : ScriptableObject
{
    [SerializeField] private float _vibrateTime;
    [SerializeField] private float _lowPower;
    [SerializeField] private float _highPower;

    public float VibrateTime => _vibrateTime;
    public float LowPower => _lowPower;
    public float HighPower => _highPower;
}
