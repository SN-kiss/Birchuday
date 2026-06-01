using UnityEngine;

namespace InGame
{
    public class Rail : MonoBehaviour
    {
        [SerializeField] private Vector2[] _points;

        public Vector2 Offset => transform.position;
        public int Length => _points?.Length ?? 0;

        public Vector2 GetPoint(int index)
        {
            if (_points == null) return Offset;

            int length = Length;
            if (length == 0) return Offset;

            return _points[OriginalCalculateUtils.Loop(0, Length, index)] + Offset;
        }

        public Vector2 GetPointInSection(int index, float time) => Vector2.Lerp(GetPoint(index), GetPoint(index + 1), time);

        public float GetSectionDistance(int index) => (GetPoint(index + 1) - GetPoint(index)).magnitude;

        private void OnDrawGizmos()
        {
            if (_points == null) return;

            int length = Length;
            if (length <= 1) return;

            for (int i = 0; i < length; i++)
            {
                OriginalGizmoUtils.DrawArrow(GetPoint(i), GetPoint(i + 1), Color.cyan);
            }
        }
    }
}