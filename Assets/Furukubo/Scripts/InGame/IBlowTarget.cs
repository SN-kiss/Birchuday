using UnityEngine;

namespace InGame
{
    public interface IBlowTarget
    {
        Vector2 Position { get; }
        void AddForce(Vector2 force);
    }
}