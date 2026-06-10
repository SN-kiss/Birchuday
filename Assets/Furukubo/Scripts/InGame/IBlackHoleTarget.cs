using UnityEngine;

namespace InGame
{
    public interface IBlackHoleTarget
    {
        Vector2 Position { get; }
        void AddForce(Vector2 force);
    }
}