using UnityEngine;

namespace InGame
{
    /// <summary>
    /// Furukubo
    /// </summary>
    public interface ILipAttractTarget
    {
        Vector2 Position { get; }
        MagneticType MagneticType { get; }
        void OnAttracted(Vector2 force);
    }

    public interface ILip
    {
        Vector2 Position { get; }
        float Rotation { get; }
        bool IsKissableNow { get; }
        void OnKissAttach(ILipAttachTarget target, Vector2 inversePos, float inverseRot);
        void OnKissDetach();
        void OnLipDamaged(int damageAmount, float nockbackPower, DamageType type);
    }
}