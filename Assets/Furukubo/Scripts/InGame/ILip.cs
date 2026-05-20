using UnityEngine;

namespace InGame
{
    /// <summary>
    /// Furukubo
    /// </summary>
    public interface ILip
    {
        Vector2 Position { get; }
        void OnAttracted(Vector2 force);
        void OnLipDamage(int damageAmount, float nockbackPower, LipDamageType type);
    }

    public interface IKissLip
    {
        Vector2 Position { get; }
        bool IsKissableNow { get; }
        void OnKissAttach(ILipAttachTarget target);
        void OnKissDetach();
    }
}