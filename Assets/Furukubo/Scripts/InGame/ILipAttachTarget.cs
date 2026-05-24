using UnityEngine;

namespace InGame
{
    /// <summary>
    /// Furukubo
    /// </summary>
    public interface ILipAttachTarget
    {
        public Vector2 Position { get; }

        public void OnAttached(ILip lip);
        public void OnDetached(ILip lip);

        public void AddForce(Vector2 force);
        public void AddImpulse(Vector2 force);

        public void AddTorque(float torque);
        public void AddTorqueImpulse(float torque);

        public float GetAttachRotation(Vector2 pos);
        public Vector2 GetAttachPoint(Vector2 pos);

        public float GetInverseTransformRotation(float rot);//Mathf.DeltaAngle(_lipRb.rotation, rot);
        public float GetTransformRotation(float rot);

        public Vector2 GetInverseTransformPoint(Vector2 pos);
        public Vector2 GetTransformPoint(Vector2 pos);
    }
}