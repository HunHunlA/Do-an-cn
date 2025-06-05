using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class voi : MonoBehaviour
{

    public float moveSpeed = 2f;
    public Transform groundCheck;
    public LayerMask groundLayer;

    // Các biến tấn công
    public float attackRange = 1.5f;
    public int bulletDamage = 10;
    public int attackDamage = 0;
    public float attackCooldown = 2f;
    public Transform attackPoint;
    public LayerMask playerLayer;
    // Biến cho đạn
    public GameObject bulletPrefab; // Kéo prefab đạn vào đây trong Inspector
    public float bulletSpeed = 2f;
    // Biến cho địa chấn
    public bool useEarthquake = true; // Bật/tắt tính năng địa chấn
    public float earthquakeRange = 5f; // Phạm vi tác động của địa chấn
    public int earthquakeDamage = 15; // Sát thương gây ra bởi địa chấn
    public float earthquakeCooldown = 5f; // Thời gian hồi cho địa chấn
    public GameObject earthquakeEffectPrefab; // Hiệu ứng địa chấn (tùy chọn)
    private float nextEarthquakeTime = 0f;

    // Biến cho nhảy
    public float jumpForce = 5f; // Lực nhảy khi tạo địa chấn
    private Rigidbody2D rb;
    private bool isGrounded = true;

    // Biến để kiểm soát hướng di chuyển
    private bool movingRight = true;
    private float nextAttackTime = 0f;
    private Animator animator; // Nếu bạn có animation

    public int maxHealth = 100;
    public int health = 100;

    public Image healthFill;
    void Start()
    {
        animator = GetComponent<Animator>(); // Lấy component Animator nếu có
        rb = GetComponent<Rigidbody2D>(); // Lấy component Rigidbody2D

        // Nếu không có Rigidbody2D, hiển thị cảnh báo
        if (rb == null)
        {
            Debug.LogWarning("Enemy cần có component Rigidbody2D để thực hiện nhảy!");
        }
        health = maxHealth; // Khởi tạo máu
    }

    void Update()
    {
        // Kiểm tra xem Enemy có đang đứng trên mặt đất không
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);


        // Kiểm tra xem có thể tấn công không
        if (Time.time >= nextAttackTime)
        {
            // Kiểm tra xem người chơi có trong phạm vi tấn công không
            Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);

            if (hitPlayers.Length > 0)
            {
                Attack();
            }
        }

        // Kiểm tra xem có thể tạo địa chấn không
        if (useEarthquake && Time.time >= nextEarthquakeTime)
        {
            // Kiểm tra nếu người chơi ở trong phạm vi địa chấn (lớn hơn phạm vi tấn công thông thường)
            Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(transform.position, earthquakeRange, playerLayer);

            if (hitPlayers.Length > 0)
            {
                CreateEarthquake();
            }
        }
    }


    public void TakeDamage(int damage)
    {
        health -= damage;
       
        healthFill.fillAmount = health / 100f;
        if (health <= 0)
        {
         
            health = 0;
            gameObject.SetActive(false);
        }
    }
    void Attack()
    {
        // Đặt thời gian cho lần tấn công tiếp theo
        nextAttackTime = Time.time + attackCooldown;

        // Phát animation tấn công nếu có
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }

        // Bắn đạn
        ShootBullet();
        // Phát hiện và gây sát thương cho người chơi trong phạm vi
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);

        foreach (Collider2D player in hitPlayers)
        {
            // Gây sát thương cho người chơi
            Player playerHealth = player.GetComponent<Player>();
            if (playerHealth != null)
            {
                //playerHealth.TakeDamage(attackDamage); 
                Debug.Log("Quái vật đã tấn công người chơi!");
            }
        }
    }
    
    void ShootBullet()
    {
        Debug.Log("Enemy đang bắn đạn!");
        // Tạo đạn tại vị trí attackPoint
        GameObject bullet = Instantiate(bulletPrefab, attackPoint.position, Quaternion.identity);

        // Xác định hướng bắn
        Collider2D[] players = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, playerLayer);
        if (players.Length > 0)
        {
            // Tính hướng từ enemy đến player
            Vector2 direction = (players[0].transform.position - transform.position).normalized;

            // Lấy component Rigidbody2D của đạn (nếu có)
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                // Áp dụng lực cho đạn theo hướng player
                rb.linearVelocity = direction * bulletSpeed;
            }

            // Lấy component Bullet (nếu có)
            Dan bulletScript = bullet.GetComponent<Dan>();
            if (bulletScript != null)
            {
                bulletScript.damage = bulletDamage;
            }

            Debug.Log("Enemy bắn đạn về phía player!");
        }
        else
        {
            // Nếu không tìm thấy player, bắn theo hướng enemy đang nhìn
            float direction = movingRight ? 1f : -1f;

            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = new Vector2(direction, 0) * bulletSpeed;
            }
        }

        // Tự hủy đạn sau 3 giây để tránh rò rỉ bộ nhớ
        Destroy(bullet, 3f);
    }
    void CreateEarthquake()
    {
        // Đặt thời gian chờ cho lần địa chấn tiếp theo
        nextEarthquakeTime = Time.time + earthquakeCooldown;

        // Chạy animation địa chấn nếu có
        if (animator != null)
        {
            animator.SetTrigger("Earthquake");
        }
        // Thực hiện nhảy cho Enemy
        if (rb != null && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            Debug.Log("Enemy nhảy lên khi tạo địa chấn!");
        }
        // Hiển thị hiệu ứng địa chấn (nếu có)
        if (earthquakeEffectPrefab != null)
        {
            GameObject effect = Instantiate(earthquakeEffectPrefab, transform.position, Quaternion.identity);
            // Có thể điều chỉnh kích thước hiệu ứng theo phạm vi
            effect.transform.localScale = new Vector3(earthquakeRange * 2, earthquakeRange * 2, 1);
            Destroy(effect, 2f); // Tự hủy hiệu ứng sau 2 giây
        }

        // Tạo hiệu ứng rung camera (nếu bạn có script CameraShake)
        // CameraShake.Instance.ShakeCamera(0.5f, 0.5f);

        // Phát hiện và gây sát thương cho người chơi trong phạm vi địa chấn
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(transform.position, earthquakeRange, playerLayer);

        foreach (Collider2D player in hitPlayers)
        {
            // Gây sát thương cho người chơi
            Player playerHealth = player.GetComponent<Player>();
            if (playerHealth != null)
            {
                // Nếu player có phương thức TakeDamage, gọi nó
                // playerHealth.TakeDamage(earthquakeDamage);

                // Tạo lực đẩy player lên (hiệu ứng bị hất tung)
                Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
                if (playerRb != null)
                {
                    float knockbackForce = 5f;
                    playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x, knockbackForce);
                }

                Debug.Log("Địa chấn đã làm tụt " + earthquakeDamage + " máu của người chơi!");
            }
        }
        // Hiển thị phạm vi tấn công trong cửa sổ Scene (để dễ điều chỉnh)
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, earthquakeRange);
        }
    }

    }

