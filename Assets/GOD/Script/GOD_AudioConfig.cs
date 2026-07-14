using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

//GOD
//Audioミキサーとスライダー繋いでるコード

public class GOD_AudioConfig : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Slider BGMSlider;
    [SerializeField] Slider SESlider;

    private void Start()
    {
        audioMixer.GetFloat("BGM", out float bgmDb);
        BGMSlider.value = DbToLinear(bgmDb);

        audioMixer.GetFloat("SE", out float seDb);
        SESlider.value = DbToLinear(seDb);
    }

    public void SetBGM(float linearValue)
    {
        audioMixer.SetFloat("BGM", LinearToDb(linearValue));
    }

    public void SetSE(float linearValue)
    {
        audioMixer.SetFloat("SE", LinearToDb(linearValue));
    }

    private float LinearToDb(float value)
    {
        value = Mathf.Clamp01(value);
        float db = 20f * Mathf.Log10(value <= 0.0001f ? 0.0001f : value);
        return Mathf.Clamp(db, -80f, 0f);
    }

    private float DbToLinear(float db)
    {
        return Mathf.Pow(10f, db / 20f);
    }
}