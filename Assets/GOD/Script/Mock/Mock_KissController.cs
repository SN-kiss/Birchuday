using UnityEngine;

/// <summary>
/// 口パーツの制御
/// ・発射 → オブジェクトにくっつく → ボディを引き寄せる
/// </summary>
public class Mock_KissController : MonoBehaviour
{
    [Header("参照")]
    [SerializeField] private Rigidbody2D bodyRb;        // ボディのRigidbody2D
    [SerializeField] private SpringJoint2D bodySpring;  // ボディについてるSpringJoint2D

    [Header("パラメータ")]
    [SerializeField] private float launchSpeed = 10f;   // 発射速度
    [SerializeField] private float pullForce = 8f;      // ボディを引き寄せる力
    [SerializeField] private float maxNeckLength = 5f;  // 首の最大長

    private Rigidbody2D rb;
    private bool isAttached = false;     // くっついているか
    private bool isLaunched = false;     // 発射中か
    private Vector2 attachedPoint;       // くっついた座標
    private Transform attachedTarget;   // くっついた相手

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // SpringJointの初期設定（切断状態）
        if (bodySpring != null)
            bodySpring.enabled = false;
    }

    void Update()
    {
        // 最大射程を超えたら自動帰還
        if (isLaunched && !isAttached)
        {
            float dist = Vector2.Distance(transform.position, bodyRb.position);
            if (dist > maxNeckLength)
            {
                ReturnToBody();
            }
        }

        // くっついている間、ボディを引き寄せる力をかける
        if (isAttached)
        {
            Vector2 dir = (Vector2)transform.position - bodyRb.position;
            bodyRb.AddForce(dir.normalized * pullForce * Time.deltaTime * 10f);
        }
    }

    /// <summary>ボディの向きに向かって発射</summary>
    public void Launch(Vector2 direction)
    {
        if (isLaunched || isAttached) return;

        isLaunched = true;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.linearVelocity = direction.normalized * launchSpeed;
    }

    /// <summary>ボディへ帰還（くっつけなかった場合）</summary>
    public void ReturnToBody()
    {
        isLaunched = false;
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;

        // ボディの位置に戻す（少し前方）
        transform.position = bodyRb.position +
            (Vector2)bodyRb.transform.right * 0.5f;

        // Springを切断
        if (bodySpring != null)
            bodySpring.enabled = false;
    }

    /// <summary>オブジェクトにくっつく</summary>
    private void AttachTo(Collider2D other)
    {
        if (isAttached) return;

        isAttached = true;
        isLaunched = false;

        // 口を止める
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Kinematic;
        attachedTarget = other.transform;

        // SpringJointでボディを引き寄せ開始
        if (bodySpring != null)
        {
            bodySpring.connectedBody = rb;
            bodySpring.distance = 0.5f;       // 目標距離
            bodySpring.frequency = 1.5f;      // バネの硬さ
            bodySpring.dampingRatio = 0.3f;   // 減衰
            bodySpring.enabled = true;
        }
    }

    /// <summary>くっつきを解除してボディへ戻る</summary>
    public void Detach()
    {
        if (!isAttached) return;

        isAttached = false;
        attachedTarget = null;
        ReturnToBody();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 発射中のみ判定
        if (!isLaunched) return;

        // "Kissable"タグのオブジェクトにくっつく
        if (other.CompareTag("Kissable"))
        {
            AttachTo(other);
        }
    }

    public bool IsAttached => isAttached;
}