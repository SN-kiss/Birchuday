using UnityEngine;

namespace InGame
{
    /// <summary>
    /// Furukubo
    /// </summary>
    public interface IDamageTarget
    {
        Vector2 Position { get; }
        void OnDamaged(int damage);
        void OnNockBack(Vector2 force);
    }
}