using UnityEngine;

public class Bullet : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject, 3f); // Destroy the bullet after 2 seconds
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy v = collision.GetComponent<Enemy>();
            if (v != null)
            {
                v.TakeDamage(20);
            }
            Destroy(gameObject);
        }
    }
}
