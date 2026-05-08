using UnityEngine;
using UnityEngine.InputSystem;

public class BodyMove_GOD : MonoBehaviour
{
    [Header("パラメータ")]
    public float dashPower = 5f;

    [Header("参照")]
    [SerializeField] private LipController lipController;

    [Header("Lipとの最大距離")]
    [SerializeField] private float maxDistanceFromLip = 3f;


    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 lookDirection = Vector2.right;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.linearDamping = 0.5f;
    }

    private void Update()
    {
        if (moveInput.magnitude > 0.1f)
        {
            lookDirection = moveInput.normalized;
            float angle = Mathf.Atan2(lookDirection.y, lookDirection.x)
                          * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnDash()
    {
        // くっついてる間はダッシュ不可（引っ張りゲームにしたいなら消してもOK）
        // if (lipController != null && lipController.IsAttached) return;
        rb.AddForce(lookDirection * dashPower, ForceMode2D.Impulse);
    }

    // 離すボタン（任意）
    public void OnDetach()
    {
        if (lipController != null)
            lipController.Detach();
    }

    private void FixedUpdate()
    {
        // Lipがくっついている間だけ距離制限
        if (lipController != null && lipController.IsAttached)
        {
            Vector2 lipPos = lipController.transform.position;
            Vector2 dir = rb.position - lipPos;

            if (dir.magnitude > maxDistanceFromLip)
            {
                // Lipから maxDistance の位置にクランプ
                rb.position = lipPos + dir.normalized * maxDistanceFromLip;
                // 離れる方向の速度成分を殺す
                Vector2 outwardVel = Vector3.Project(rb.linearVelocity, dir.normalized);
                rb.linearVelocity -= outwardVel;
            }
        }
    }
}