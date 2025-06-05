using UnityEngine;

public class Dan : MonoBehaviour
{
    public int damage = 10;

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Ki?m tra n?u ??n ch?m vào Player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Gây sát th??ng cho Player
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                // N?u Player có ph??ng th?c TakeDamage, g?i nó
                // player.TakeDamage(damage);
                Debug.Log("Bắn trúng người chơi và gây " + damage + " sát thương!");
            }

            // H?y ??n sau khi trúng player
            Destroy(gameObject);
        }
        else if (!collision.CompareTag("Enemy")) // Không h?y n?u ch?m vào enemy (ng??i b?n)
        {
            // H?y ??n n?u ch?m vào các v?t th? khác (nh? t??ng, m?t ??t)
            Destroy(gameObject);
        }
    }
}
