using UnityEngine;

namespace InGame
{
    /// <summary>
    /// Furukubo
    /// </summary>
    public interface IDamageTarget
    {
        Vector2 Position { get; }
        void OnDamaged(int damage, Vector2 nockback);
    }
}