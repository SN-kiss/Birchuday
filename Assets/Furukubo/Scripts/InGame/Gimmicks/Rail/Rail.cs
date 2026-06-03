using UnityEngine;

namespace InGame
{
    /// <summary>
    /// Furukubo
    /// </summary>
    public class Rail : MonoBehaviour
    {
        [Header("Parameters")]
        [SerializeField] private RailPositionMode _positionMode;
        [SerializeField] private Vector2[] _points;

        public Vector2 Offset => _positionMode == RailPositionMode.Local ? transform.position : Vector2.zero;
        public int SectionCount => _points?.Length ?? 0;

        public Vector2 GetPoint(int index)
        {
            if (_points == null) return Offset;

            int length = SectionCount;
            if (length == 0) return Offset;

            return _points[OriginalCalculateUtils.LoopInt(0, SectionCount, index)] + Offset;
        }

        public Vector2 GetPointInSection(int index, float time) => Vector2.Lerp(GetPoint(index), GetPoint(index + 1), time);

        public float GetSectionDistance(int index) => (GetPoint(index + 1) - GetPoint(index)).magnitude;

        private void OnDrawGizmos()
        {
            int length = SectionCount;
            if (length <= 1) return;

            for (int i = 0; i < length; i++)
            {
                OriginalGizmoUtils.DrawArrow(GetPoint(i), GetPoint(i + 1), Color.cyan);
            }
        }

        private enum RailPositionMode
        {
            Local, 
            World
        }
    }
}