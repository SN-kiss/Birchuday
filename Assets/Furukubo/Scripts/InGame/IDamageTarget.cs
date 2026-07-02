using UnityEngine;

namespace InGame
{
    /// <summary>
    /// Furukubo
    /// </summary>
    public interface IDamageTarget
    {
        MagneticType MagneticType { get; }
        Vector2 Position { get; }
        void OnDamaged(int damage, Vector2 nockback);
        void OnDetach();
    }
}