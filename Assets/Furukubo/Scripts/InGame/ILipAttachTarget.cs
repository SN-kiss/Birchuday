using InGame.Player;
using UnityEngine;

namespace InGame
{
    /// <summary>
    /// Furukubo
    /// </summary>
    public interface ILipAttachTarget
    {
        public Vector2 Position { get; }
        public float Rotation { get; }

        public void OnAttached(PlayerLip attacher);
        public void OnDetached();
        public void AddForce(Vector2 force);
        public Vector2 GetClosestPoint(Vector2 pos);
        public Vector2 GetInverseTransformPoint(Vector2 pos);
        public Vector2 GetTransformPoint(Vector2 pos);
    }
}