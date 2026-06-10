using UnityEngine;

/// <summary>
/// 星形スプライトをランタイムで生成してキャッシュするユーティリティ
/// ButtonParticleEffect から自動で使われます（手動での操作不要）
/// </summary>
public static class GOD_StarSprite
{
    private static Sprite _cache;

    public static Sprite Get()
    {
        if (_cache != null) return _cache;
        _cache = CreateStar(64, 5, 0.42f);
        return _cache;
    }

    private static Sprite CreateStar(int size, int points, float innerRatio)
    {
        Texture2D tex = new Texture2D(size, size, TextureFormat.RGBA32, false);
        tex.filterMode = FilterMode.Bilinear;

        Vector2 center = new Vector2(size * 0.5f, size * 0.5f);
        float outerRadius = size * 0.5f - 1f;
        float innerRadius = outerRadius * innerRatio;

        // 星の頂点
        Vector2[] verts = new Vector2[points * 2];
        for (int i = 0; i < points * 2; i++)
        {
            float angle = i * Mathf.PI / points - Mathf.PI * 0.5f;
            float r = (i % 2 == 0) ? outerRadius : innerRadius;
            verts[i] = center + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * r;
        }

        // ピクセル塗り
        for (int y = 0; y < size; y++)
            for (int x = 0; x < size; x++)
                tex.SetPixel(x, y, InsidePoly(new Vector2(x, y), verts) ? Color.white : Color.clear);

        tex.Apply();
        return Sprite.Create(tex,
            new Rect(0, 0, size, size),
            new Vector2(0.5f, 0.5f));
    }

    private static bool InsidePoly(Vector2 p, Vector2[] poly)
    {
        bool inside = false;
        int n = poly.Length, j = n - 1;
        for (int i = 0; i < n; j = i++)
        {
            if ((poly[i].y > p.y) != (poly[j].y > p.y) &&
                p.x < (poly[j].x - poly[i].x) * (p.y - poly[i].y)
                      / (poly[j].y - poly[i].y) + poly[i].x)
                inside = !inside;
        }
        return inside;
    }
}