using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    public float movement;
    public float speed = 5f;
    public float jumpHeight = 7f;

    public bool isGround = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        movement = Input.GetAxis("Horizontal");
        movement = SimpleInput.GetAxis("Horizontal");
    }

    private void FixedUpdate()
    {
        transform.position += new Vector3(movement, 0f, 0f) * Time.fixedDeltaTime * speed;
    }

    public void JumpButton()
    {
        if (isGround == true) {
            Vector2 velocity = rb.linearVelocity;
            velocity.y = jumpHeight;
            rb.linearVelocity = velocity;
            isGround = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground") {
            isGround = true;
        }
    }

}
