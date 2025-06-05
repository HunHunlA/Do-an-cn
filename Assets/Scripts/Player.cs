using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D rb;
    public float movement;
    public float speed = 5f;
    public float jumpHeight = 7f;

    public bool isGround = true;
    public bool facingRight = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        movement = Input.GetAxis("Horizontal");
        movement = SimpleInput.GetAxis("Horizontal");

        if (movement < 0f && facingRight == true)
        {
            transform.eulerAngles = new Vector3(0f, -180f, 0f);
            facingRight = false;
        }
        else if (movement > 0f && facingRight == false) {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
            facingRight = true;
        }

        if (Mathf.Abs(movement) > 0.1f) {
            animator.SetFloat("Run", 1f);
        }
        else if (movement < 0.1f) {
            animator.SetFloat("Run", 0f);
        }
    }

    private void FixedUpdate()
    {
        transform.position += new Vector3(movement, 0f, 0f) * Time.fixedDeltaTime * speed;
    }

    public void PlayAttackAnimation() {
        int randomAttack = Random.Range(0, 3);

        if (randomAttack == 0) {
            animator.SetTrigger("Attack1");
        }
        else if (randomAttack == 1) {
            animator.SetTrigger("Attack2");
        }
        else {
            animator.SetTrigger("Attack3");
        }
    }

    public void JumpButton()
    {
        if (isGround == true) {
            Vector2 velocity = rb.linearVelocity;
            velocity.y = jumpHeight;
            rb.linearVelocity = velocity;
            isGround = false;
            animator.SetBool("Jump", true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground") {
            isGround = true;
            animator.SetBool("Jump", false);
        }
    }

}
