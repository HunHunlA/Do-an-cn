using UnityEngine;

public class Dan : MonoBehaviour
{
    public int damage = 10;
    public float speed;
    private Rigidbody2D rb2d;
    private Vector2 initPos;
    private Vector2 initScale;
    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        initScale = transform.localScale;
    }
    private void OnEnable()
    {
        initPos = transform.position;
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector2 direction = (playerPos - transform.position).normalized;
        rb2d.linearVelocity = direction * speed;
    }
    private void OnDisable()
    {
        transform.localScale = initScale;
    }
    private void Update()
    {
        if (Vector2.Distance(transform.position, initPos) > 20f)
        {
            gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            gameObject.SetActive(false);

            collision.GetComponent<Player>().TakeDamage(damage);
        }
    }
}
