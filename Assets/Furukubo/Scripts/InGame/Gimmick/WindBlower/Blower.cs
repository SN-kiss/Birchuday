using System.Collections.Generic;
using UnityEngine;

namespace InGame
{
    public class Blower : MonoBehaviour
    {
        [Header("Parameters")]
        [SerializeField, Min(0)] private float _blowPowerMax;
        [SerializeField, Min(0)] private float _length;
        [SerializeField, Min(0)] private int _rayDisits;

        List<Collider2D> hitColsInFrame = new List<Collider2D>();

        private float Range => transform.localScale.x;
        private float Angle => transform.localEulerAngles.z;

        private void FixedUpdate()
        {
            if (_rayDisits <= 0) return;

            hitColsInFrame.Clear();

            Vector2 center = transform.position;
            Vector2 rotation = CalculateUtilities.AngleToDirection(Angle - 90f);
            int rayDisitsHalf = _rayDisits / 2;
            float interval = Range / _rayDisits;

            Vector2 dir = BlowDirection();

            for (int i = -rayDisitsHalf; i < rayDisitsHalf + 1; i++)
            {
                Vector2 startOffset = rotation * interval * i;
                Vector2 startPos = center + startOffset;

                RaycastHit2D hit = Physics2D.Raycast(startPos, dir, _length);

                Collider2D col = hit.collider;

                if (col == null) continue;
                if(hitColsInFrame.Contains(col))continue;

                if (col.TryGetComponent(out IBlowTarget target))
                {
                    target.AddForce(dir * BlowPower(target.Position));
                    hitColsInFrame.Add(col);
                }
            }
        }

        private void OnDrawGizmos()
        {
            if(_rayDisits <= 0) return;

            Vector2 center = transform.position;
            Vector2 rotation = CalculateUtilities.AngleToDirection(Angle - 90f);
            int rayDisitsHalf = _rayDisits / 2;
            float interval = Range / _rayDisits;

            Vector2 endOffset = BlowDirection() * _length;

            for (int i = -rayDisitsHalf; i < rayDisitsHalf + 1; i++)
            {
                Vector2 startOffset = rotation * interval * i;
                Vector2 startPos = center + startOffset;
                Vector2 endPos = startPos + endOffset;

                Debug.DrawLine(startPos, endPos, Color.cyan);
            }

            float radius = Range * 0.5f;
            Debug.DrawLine(center - rotation * radius, center + rotation * radius, Color.cyan);
        }

        private Vector2 BlowDirection() => CalculateUtilities.AngleToDirection(Angle);

        private float BlowPower(Vector2 targetPos)
        {
            Vector2 pos = transform.position;

            float distance = Mathf.Clamp(Mathf.Abs(Vector2.Dot(targetPos - pos, BlowDirection())), 1f, float.MaxValue);

            Debug.Log(distance);

            return _blowPowerMax / distance;
        }
    }
}