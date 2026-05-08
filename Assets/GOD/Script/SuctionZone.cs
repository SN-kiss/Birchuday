using UnityEngine;

/// <summary>
/// オブジェクトAの吸いつき範囲
/// 別途CircleCollider2D(IsTrigger=ON)を大きめにつけること
/// </summary>
public class SuctionZone : MonoBehaviour
{
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Lipが範囲に入ったら吸着開始
        LipController lip = other.GetComponent<LipController>();
        if (lip != null)
        {
            lip.StartAttracting(rb);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // 範囲から出たら（くっつく前に離れた場合）戻す
        LipController lip = other.GetComponent<LipController>();
        if (lip != null && !lip.IsAttached)
        {
            lip.Detach();
        }
    }
}