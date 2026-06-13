using UnityEngine;

namespace InGame
{
    /// <summary>
    /// Furukubo
    /// </summary>
    public interface ILip
    {
        MagneticType MagneticType { get; }
        Vector2 LipPosition { get; }
        float LipRotation { get; }
        bool IsKissableNow { get; }
        void OnAttachFromTarget(ILipAttachTarget target, Vector2 inversePos, float inverseRot);
        void OnDetachFromTarget();
        void OnKiss();
        void OnDamaged(int damageAmount, float nockbackPower, DamageType type);
        bool TryRecover(int recoveryAmount);
    }
}