using UnityEngine;

namespace InGame
{
    /// <summary>
    /// Furukubo
    /// </summary>
    public interface ILipAttractTarget
    {
        Vector2 LipPosition { get; }
        MagneticType MagneticType { get; }
        void OnAttracted(Vector2 force);
    }
}