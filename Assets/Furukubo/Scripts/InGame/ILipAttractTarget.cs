using UnityEngine;

namespace InGame
{
    /// <summary>
    /// Furukubo
    /// </summary>
    public interface ILipAttractTarget
    {
        Vector2 Position { get; }
        void OnAttracted(Vector2 force);
    }

    public interface ILip
    {
        Vector2 Position { get; }
        //bool IsKissableNow { get; }
        //void OnKissAttach(ILipAttachTarget target);
        //void OnKissDetach();
        void OnLipDamage(int damageAmount, float nockbackPower, LipDamageType type);
    }
}