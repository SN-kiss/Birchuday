using UnityEngine;

namespace InGame
{
    /// <summary>
    /// Furukubo
    /// </summary>
    public interface IAttracter
    {
        float Weight { get; }
        void AddForce(Vector2 force);
        Vector2 GetClosestPoint(Vector2 pos);
        Vector2 GetInverseTransformPoint(Vector2 pos);
        Vector2 GetTransformPoint(Vector2 pos);
    }
}