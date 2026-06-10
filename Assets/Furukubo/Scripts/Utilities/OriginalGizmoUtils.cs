using UnityEngine;

/// <summary>
/// Furukubo
/// </summary>
public static class OriginalGizmoUtils
{
    private const float _ArrowHeadAngle = 30f;
    private const float _ArrowHeadLength = 0.25f;

    public static void DrawArrow(Vector2 start, Vector2 end) => DrawArrow(start, end, Color.white);

    public static void DrawArrow(Vector2 start, Vector2 end, Color color)
    {
        Vector2 endToStart = (start - end).normalized;
        float endToStartAngle = OriginalCalculateUtils.DirectionToAngle(endToStart);
        Vector2 head1Offset = OriginalCalculateUtils.AngleToDirection(endToStartAngle + _ArrowHeadAngle) * _ArrowHeadLength;
        Vector2 head2Offset = OriginalCalculateUtils.AngleToDirection(endToStartAngle - _ArrowHeadAngle) * _ArrowHeadLength;

        Vector2 head1 = end + head1Offset;
        Vector2 head2 = end + head2Offset;

        Debug.DrawLine(start, (head1 + head2) * 0.5f, color);
        Debug.DrawLine(end, head1, color);
        Debug.DrawLine(end, head2, color);
        Debug.DrawLine(head1, head2, color);
    }

    public static void DrawStar(Vector2 pos, int vert, float outRadius, float inRadius) => DrawStar(pos, vert, outRadius, inRadius, Color.white);

    public static void DrawStar(Vector2 pos, int vert, float outRadius, float inRadius, Color color)
    {
        Vector2 oldPoint = Vector2.zero;

        int disit = vert * 2;
        float interval = 360f / disit;
        float halfInterval = interval * 0.5f;

        for (int i = 0; i <= disit; i++)
        {
            if(i == 0)
            {
                float angle = interval * i + halfInterval;
                oldPoint = pos + OriginalCalculateUtils.AngleToDirection(angle) * outRadius;
            }
            else
            {
                float radius = i % 2 == 0 ? outRadius : inRadius;
                float angle = interval * i + halfInterval;
                Vector2 point = pos + OriginalCalculateUtils.AngleToDirection(angle) * radius;
                Debug.DrawLine(oldPoint, point, color);
                oldPoint = point;
            }
        }
    }
}