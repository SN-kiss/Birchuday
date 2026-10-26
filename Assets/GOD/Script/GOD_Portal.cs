using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Collider2D))]
public class GOD_Portal : MonoBehaviour
{
    [Header("スプライトA(大きくなる+光る)")]
    public Transform spriteA;
    public SpriteRenderer spriteARenderer;
    [Tooltip("シーンに置いた大きさに対する倍率")]
    public float aNearScale = 1.3f;
    public float aGrowDuration = 0.25f;
    public float aShrinkDuration = 0.25f;
    public Color glowColor = new Color(1f, 1f, 0.6f, 1f);
    public GameObject glowObject;
    [Tooltip("離れても光ったままにする")]
    public bool keepGlowAfterExit = true;

    [Header("スプライトB(0から、指定位置からバウンドして拡大)")]
    public Transform spriteB;
    [Tooltip("近づいたときのBの大きさ(離れているときは0)")]
    public Vector3 bShowScale = Vector3.one;
    [Tooltip("Bが大きくなり始める位置(空ならその場で拡大)")]
    public Transform bStartPoint;
    public float bGrowDuration = 0.4f;
    public float bShrinkDuration = 0.4f;
    [Tooltip("拡大カーブ。1を超えるとオーバーシュート(バウンド)")]
    public AnimationCurve bGrowCurve = new AnimationCurve(
        new Keyframe(0f, 0f), new Keyframe(0.6f, 1.15f),
        new Keyframe(0.8f, 0.95f), new Keyframe(1f, 1f));
    [Tooltip("縮小カーブ。マイナスにすると一度大きくなってから縮む")]
    public AnimationCurve bShrinkCurve = new AnimationCurve(
        new Keyframe(0f, 0f), new Keyframe(0.3f, -0.15f), new Keyframe(1f, 1f));

    [Header("プレイヤー判定(タグ名)")]
    public string playerTagNorth = "PlayerNorth";
    public string playerTagSouth = "PlayerSouth";

    [Header("シーン移動")]
    public string sceneName;

    Vector3 aBaseScale;
    Color aBaseColor;
    Vector3 bEndPos;    // Bのシーン上の位置(拡大後の位置)
    Vector3 bStartPos;  // Bが出てくる位置
    Coroutine aRoutine, bRoutine;
    readonly HashSet<GameObject> playersInRange = new HashSet<GameObject>();

    void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;

        if (spriteA) aBaseScale = spriteA.localScale;
        else Debug.LogWarning("GOD_Portal: Sprite A が未設定です", this);

        if (spriteARenderer) aBaseColor = spriteARenderer.color;

        if (spriteB)
        {
            bEndPos = spriteB.localPosition;
            bStartPos = bEndPos;
            if (bStartPoint)
            {
                bStartPos = spriteB.parent
                    ? spriteB.parent.InverseTransformPoint(bStartPoint.position)
                    : bStartPoint.position;
            }
        }
        else Debug.LogWarning("GOD_Portal: Sprite B が未設定です", this);

        ResetState();
    }

    void OnDisable()
    {
        playersInRange.Clear();
        if (aRoutine != null) { StopCoroutine(aRoutine); aRoutine = null; }
        if (bRoutine != null) { StopCoroutine(bRoutine); bRoutine = null; }
        ResetState();
    }

    // 離れているときの状態(Aは元の大きさ、Bは0で非表示)に戻す
    void ResetState()
    {
        if (spriteA) spriteA.localScale = aBaseScale;
        if (spriteARenderer) spriteARenderer.color = aBaseColor;
        if (glowObject) glowObject.SetActive(false);
        if (spriteB)
        {
            spriteB.localScale = Vector3.zero;
            spriteB.localPosition = bStartPos;
        }
    }

    void Update()
    {
        if (playersInRange.Count == 0) return;

        bool pressed = false;

        // Bボタン(右側のボタン = buttonEast)
        if (Gamepad.current != null && Gamepad.current.buttonEast.wasPressedThisFrame)
            pressed = true;

        // キーボード確認用: Xキー
        if (Keyboard.current != null && Keyboard.current.xKey.wasPressedThisFrame)
            pressed = true;

        if (pressed && !string.IsNullOrEmpty(sceneName))
            SceneManager.LoadScene(sceneName);
    }

    GameObject FindPlayerRoot(Collider2D other)
    {
        Transform t = other.transform;
        while (t != null)
        {
            if (t.CompareTag(playerTagNorth) || t.CompareTag(playerTagSouth))
                return t.gameObject;
            t = t.parent;
        }
        return null;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActiveAndEnabled) return;

        GameObject root = FindPlayerRoot(other);
        if (root == null) return;

        bool wasEmpty = playersInRange.Count == 0;
        playersInRange.Add(root);
        if (!wasEmpty) return;

        SetGlow(true);
        StartA(aBaseScale * aNearScale, aGrowDuration);
        StartB(bShowScale, bEndPos, bGrowDuration, bGrowCurve);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        // オブジェクトが無効になるときにも呼ばれるので、そのときは何もしない
        if (!isActiveAndEnabled) return;

        GameObject root = FindPlayerRoot(other);
        if (root == null) return;

        if (StillOverlapping(root)) return;

        playersInRange.Remove(root);
        if (playersInRange.Count > 0) return;

        if (!keepGlowAfterExit) SetGlow(false);
        StartA(aBaseScale, aShrinkDuration);
        StartB(Vector3.zero, bStartPos, bShrinkDuration, bShrinkCurve);
    }

    bool StillOverlapping(GameObject root)
    {
        var col = GetComponent<Collider2D>();
        var results = new List<Collider2D>();
        col.Overlap(ContactFilter2D.noFilter, results);
        foreach (var c in results)
            if (c != null && c.transform.IsChildOf(root.transform)) return true;
        return false;
    }

    void SetGlow(bool on)
    {
        if (spriteARenderer) spriteARenderer.color = on ? glowColor : aBaseColor;
        if (glowObject) glowObject.SetActive(on);
    }

    // ---- スプライトA ----
    void StartA(Vector3 to, float duration)
    {
        if (!spriteA || !isActiveAndEnabled) return;
        if (aRoutine != null) StopCoroutine(aRoutine);
        aRoutine = StartCoroutine(ARoutine(to, duration));
    }

    IEnumerator ARoutine(Vector3 to, float duration)
    {
        Vector3 from = spriteA.localScale;
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float n = Mathf.Clamp01(t / duration);
            spriteA.localScale = Vector3.Lerp(from, to, Mathf.SmoothStep(0f, 1f, n));
            yield return null;
        }
        spriteA.localScale = to;
    }

    // ---- スプライトB ----
    void StartB(Vector3 toScale, Vector3 toPos, float duration, AnimationCurve curve)
    {
        if (!spriteB || !isActiveAndEnabled) return;
        if (bRoutine != null) StopCoroutine(bRoutine);
        bRoutine = StartCoroutine(BRoutine(toScale, toPos, duration, curve));
    }

    IEnumerator BRoutine(Vector3 toScale, Vector3 toPos, float duration, AnimationCurve curve)
    {
        Vector3 fromScale = spriteB.localScale;
        Vector3 fromPos = spriteB.localPosition;
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float n = Mathf.Clamp01(t / duration);
            float k = curve.Evaluate(n);
            spriteB.localScale = Vector3.LerpUnclamped(fromScale, toScale, k);
            spriteB.localPosition = Vector3.Lerp(fromPos, toPos, Mathf.SmoothStep(0f, 1f, n));
            yield return null;
        }
        spriteB.localScale = toScale;
        spriteB.localPosition = toPos;
    }
}