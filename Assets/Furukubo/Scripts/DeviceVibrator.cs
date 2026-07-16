using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Furukubo
/// </summary>
public class DeviceVibrator : MonoBehaviour
{
    public static DeviceVibrator Instance { get; private set; }

    private DeviceVibrationData _data;
    private bool _stop;
    private float _vibrateTimeCount;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            StopVibrate();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (_stop) return;
        if (_data == null) return;

        _vibrateTimeCount -= Time.deltaTime;

        if (_vibrateTimeCount <= 0f)
        {
            StopVibrate();
        }
        else
        {
            float ratio = _data.VibrateTime == 0f ? 1f : Mathf.Clamp01(_vibrateTimeCount / _data.VibrateTime);
            SetVibration(_data.LowPower * ratio, _data.HighPower * ratio);
        }
    }

    private void OnDestroy() => StopVibrate();
    private void OnApplicationQuit() => StopVibrate();

    private void SetVibration(float lowPower, float highPower)
    {
        foreach (var g in Gamepad.all)
        {
            g.SetMotorSpeeds(lowPower, highPower);
        }
    }

    public void StartVibrate(DeviceVibrationData data)
    {
        if (data == null) return;

        _data = data;
        _stop = false;
        _vibrateTimeCount = data.VibrateTime;

        SetVibration(data.LowPower, data.HighPower);

        Debug.Log("Device vibrate started!");
    }

    public void StopVibrate()
    {
        _stop = true;

        SetVibration(0f, 0f);

        Debug.Log("Device vibrate stopped!");
    }
}