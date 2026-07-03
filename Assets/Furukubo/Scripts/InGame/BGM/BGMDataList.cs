using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Furukubo
/// </summary>
[CreateAssetMenu(menuName = "ScriptableObjects/BGM Data List")]
public class BGMDataList : ScriptableObject
{
    [SerializeField] private List<BGMData> _list;

    public BGMData[] GetBGMs() => _list.ToArray();
}

[Serializable]
public struct BGMData
{
    [SerializeField] private BGMType _type;
    [SerializeField] private AudioClip _clip;

    public BGMType Type => _type;
    public AudioClip Clip => _clip;
}