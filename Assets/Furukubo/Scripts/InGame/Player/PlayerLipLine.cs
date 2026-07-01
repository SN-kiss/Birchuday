using UnityEngine;

namespace InGame.Player
{
    /// <summary>
    /// Furukubo (Refactoring of Mock_Line)
    /// </summary>
    public class PlayerLipLine : MonoBehaviour
    {
        [Header("Parameters")]
        [SerializeField] private int _lineSegments;
        [SerializeField] private float _handleLengthCoef;

        [Header("References")]
        [SerializeField] private Rigidbody2D _bodyRb;
        [SerializeField] private Rigidbody2D _lipRb;
        [SerializeField] private Transform _connectPointTrBody;
        [SerializeField] private Transform _connectPointTrLip;
        [SerializeField] private LineRenderer _line;

        private void Update()
        {
            if(_line == null) return;

            _line.positionCount = _lineSegments + 1;

            Vector3 pos = transform.position;

            Vector2 body = _connectPointTrBody.position - pos;
            Vector2 lip = _connectPointTrLip.position - pos;

            Vector2 bodyDir = OriginalCalculateUtils.AngleToDirection(_bodyRb.rotation);
            Vector2 lipDir = OriginalCalculateUtils.AngleToDirection(_lipRb.rotation);

            float length = (body - lip).magnitude * _handleLengthCoef;

            _line.SetPositions(GetPoints(body, body + bodyDir * length, lip - lipDir * length, lip, _lineSegments));
        }

        private Vector3 GetPointThree(Vector3 start, Vector3 handle, Vector3 end, float time)
        {
            return Vector3.Lerp(Vector3.Lerp(start, handle, time), Vector3.Lerp(handle, end, time), time);
        }

        private Vector3 GetPointFour(Vector3 start, Vector3 handle1, Vector3 handle2, Vector3 end, float time)
        {
            Vector3 a = GetPointThree(start, handle1, handle2, time);
            Vector3 b = GetPointThree(handle1, handle2, end, time);

            return Vector3.Lerp(a, b, time);
        }

        private Vector3[] GetPoints(Vector3 start, Vector3 handle1, Vector3 handle2, Vector3 end, int disits)
        {
            if (disits <= 0) return new Vector3[0];

            Vector3[] points = new Vector3[disits + 1];

            for (int i = 0; i <= disits; i++)
            {
                float t = (float)i / disits;

                points[i] = GetPointFour(start, handle1, handle2, end, t);
            }

            return points;
        }

        private void OnDrawGizmos()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                if (_connectPointTrBody == null || _connectPointTrLip == null) return;

                Vector3 pos = transform.position;

                _line.positionCount = 2;
                _line.SetPosition(0, _connectPointTrBody.position - pos);
                _line.SetPosition(1, _connectPointTrLip.position - pos);
            }
#endif
        }
    }
}