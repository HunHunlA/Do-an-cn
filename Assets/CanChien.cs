using UnityEngine;

public class CanChien : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 1f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            voi v = collision.GetComponent<voi>();
            if (v != null)
            {
                v.TakeDamage(10);
            }
            Destroy(gameObject);
        }
    }
}
