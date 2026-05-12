using UnityEngine;
public class Mock_Line : MonoBehaviour
{
    [SerializeField] private Transform bodyTransform;   // ボディ
    [SerializeField] private Transform kissTransform;   // 口パーツ
    [Header("見た目")]
    [SerializeField] private int lineSegments = 20;     // うねうね分割数
    [SerializeField] private float waveAmplitude = 0.1f; // うねりの強さ
    [SerializeField] private float waveSpeed = 3f;
    private LineRenderer line;
    void Start()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = lineSegments;
    }
    void Update()
    {
        // ボディ→口をlineSegments分割で描画（少しうねらせる）
        for (int i = 0; i < lineSegments; i++)
        {
            float t = (float)i / (lineSegments - 1);
            Vector3 pos = Vector3.Lerp(
                bodyTransform.position,
                kissTransform.position, t);
            // 垂直方向に揺れを加える（ゴムっぽさ）
            float wave = Mathf.Sin(t * Mathf.PI + Time.time * waveSpeed)
                         * waveAmplitude * (1 - Mathf.Abs(t - 0.5f) * 2);
            Vector3 perp = Vector3.Cross(
                (kissTransform.position - bodyTransform.position).normalized,
                Vector3.forward);
            pos += perp * wave;
            line.SetPosition(i, pos);
        }
    }
}