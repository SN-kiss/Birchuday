using UnityEngine;
using UnityEngine.InputSystem;

public class BodyMove_GOD : MonoBehaviour
{
    public float dashPower = 5f;

    private Rigidbody2D rb;

    private Vector2 moveInput;
    private Vector2 lookDirection = Vector2.right;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // 左スティック入力で向き変更
        if (moveInput.magnitude > 0.1f)
        {
            lookDirection = moveInput.normalized;

            float angle =
                Mathf.Atan2(lookDirection.y, lookDirection.x)
                * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    // 左スティック
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    // ↓ボタン
    public void OnDash()
    {
        rb.AddForce(lookDirection * dashPower,
            ForceMode2D.Impulse);
    }
}