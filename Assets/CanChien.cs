using UnityEngine;

public class CanChien : MonoBehaviour
{
    void Start()
    {
        AudioManager.instance.PlaySFX("PlayerCanChien");
        Destroy(gameObject, 1f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy v = collision.GetComponent<Enemy>();
            if (v != null)
            {
                v.TakeDamage(10);
            }
            Destroy(gameObject);
        }
    }
}
