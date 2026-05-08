using UnityEngine;

public class LipController : MonoBehaviour
{
    public enum LipState
    {
        FollowBody,
        Attracting,
        Attached
    }

    [Header("参照")]
    [SerializeField] private Rigidbody2D bodyRb;

    [Header("吸着パラメータ")]
    [SerializeField] private float attractSpeed = 8f;
    [SerializeField] private float attachThreshold = 0.1f;

    [Header("引き寄せパラメータ")]
    [SerializeField] private float pullForce = 5f;
    [SerializeField] private float pullStopDistance = 0.5f; // この距離以下になったら引くのをやめる

    [Header("引っ張り力")]
    [SerializeField] private float liftPower = 1f; // Bodyから受け取るか、ここで定義


    public LipState CurrentState { get; private set; } = LipState.FollowBody;

    private Rigidbody2D rb;
    private Rigidbody2D targetRb;
    private Transform originalParent; // 元の親（Body）を覚えておく

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalParent = bodyRb.transform; // 最初の親＝Body

        // 開始時はBodyの子として物理を無効化
        SetFollowMode();
    }

    void FixedUpdate()
    {
        switch (CurrentState)
        {
            case LipState.Attracting:
                HandleAttracting();
                break;
            case LipState.Attached:
                HandleAttached();
                break;
        }
        // FollowBody時はBodyの子なので何もしなくてOK
    }

    // 通常時モード：Bodyの子にする
    private void SetFollowMode()
    {
        CurrentState = LipState.FollowBody;

        // Bodyの子に戻す
        transform.SetParent(originalParent);

        // 物理を止める（親子移動に任せる）
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.linearVelocity = Vector2.zero;
    }

    // 吸着中：オブジェクトAへ引き寄せられる
    private void HandleAttracting()
    {
        if (targetRb == null) { ReturnToFollow(); return; }

        // オブジェクトAの表面の最近点を目標にする
        Collider2D targetCol = targetRb.GetComponent<Collider2D>();
        Vector2 targetPos = targetCol != null
            ? targetCol.ClosestPoint(rb.position)  // 表面の最近点
            : targetRb.position;                    // Colliderなければ中心

        Vector2 dir = targetPos - rb.position;
        float dist = dir.magnitude;

        rb.linearVelocity = dir.normalized * attractSpeed;

        // 表面に到達したら止まる
        if (dist < attachThreshold)
        {
            AttachToTarget();
        }
    }

    // くっついた後：BodyをLipへ引き寄せる
    private void HandleAttached()
    {
        transform.position = targetRb.transform.TransformPoint(attachedLocalOffset);

        Vector2 dir = (Vector2)transform.position - bodyRb.position;
        float dist = dir.magnitude;

        if (dist <= pullStopDistance) return;

        // ★ 重さチェック
        ObjectA liftable = targetRb.GetComponent<ObjectA>();
        float weight = liftable != null ? liftable.weight : 1f;

        float force = Mathf.Clamp(dist * pullForce, 0f, pullForce * 2f);

        if (liftPower >= weight)
        {
            // 持ち上げ可能 → オブジェクトAを引っ張る
            targetRb.AddForce(dir.normalized * force); // BodyへAを引き寄せる
        }

        // Bodyは常に引っ張られる
        bodyRb.AddForce(dir.normalized * force);
    }

    // オブジェクトAにくっつく
    private Vector2 attachedLocalOffset; // オブジェクトAからの相対位置

    private void AttachToTarget()
    {
        Debug.Log("Attached!");
        CurrentState = LipState.Attached;

        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;

        // ワールド座標 → オブジェクトAのローカル座標で記憶
        attachedLocalOffset = targetRb.transform
            .InverseTransformPoint(transform.position);
    }

    // 離れる・リセット
    public void Detach()
    {
        targetRb = null;
        ReturnToFollow();
    }

    private void ReturnToFollow()
    {
        SetFollowMode();
    }

    // SuctionZoneから呼ばれる
    public void StartAttracting(Rigidbody2D objectARb)
    {
        if (CurrentState != LipState.FollowBody) return;

        targetRb = objectARb;

        // Bodyの子から切り離す
        transform.SetParent(null);

        // 物理を有効化
        rb.bodyType = RigidbodyType2D.Dynamic;

        CurrentState = LipState.Attracting;
    }

    public bool IsAttached => CurrentState == LipState.Attached;
}