using UnityEngine;

namespace InGame
{
    /// <summary>
    /// Furukubo
    /// </summary>
    public interface ILip
    {
        void OnLipDamage(int damageAmount, float nockbackPower, LipDamageType type);
    }
}