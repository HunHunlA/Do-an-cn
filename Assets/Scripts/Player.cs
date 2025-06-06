using System.Collections;
using System.ComponentModel;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject canChienPrefab;
    public Animator animator;
    public Rigidbody2D rb;
    public float movement;
    public float speed = 5f;
    public float jumpHeight = 7f;
    public float health = 100;

    public bool canShoot = true;
    public bool isGround = true;
    public bool facingRight = true;
    public Transform attackPoint;
    public float shootCD = 1f;
    public Coroutine shootBulletCoroutine;
    public Coroutine canChienCoroutine;
    public Image healthFill;


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
        CanChienButton();
    }

    public void CanChienButton()
    {
        if (canChienCoroutine != null)
        {
            StopCoroutine(canChienCoroutine);
        }
        canChienCoroutine = StartCoroutine(SpawnCanChien());
    }

    public IEnumerator SpawnCanChien()
    {
        yield return new WaitForSeconds(0.5f);
        var canChien = Instantiate(canChienPrefab, attackPoint.position, Quaternion.identity);
        var canChienRb = canChien.GetComponent<Rigidbody2D>();
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
    public void ShootButton()
    {
        if(canShoot == false) {
            return;
        }
        if (shootBulletCoroutine != null) {
            StopCoroutine(shootBulletCoroutine);
        }
        shootBulletCoroutine = StartCoroutine(ShootBullet());
    }
    public IEnumerator ShootBullet()
    {
        canShoot = false;
        animator.SetTrigger("Attack1");
        yield return new WaitForSeconds(0.5f);
        AudioManager.instance.PlaySFX("PlayerShoot");
        var bullet = Instantiate(bulletPrefab, attackPoint.position, Quaternion.identity);
        var bulletRb = bullet.GetComponent<Rigidbody2D>();
        if (facingRight) {
            bulletRb.AddForce(new Vector2(1f, 0f) * 20f, ForceMode2D.Impulse);
        }
        else {
            bulletRb.AddForce(new Vector2(-1f, 0f) * 20f, ForceMode2D.Impulse);
        }
        yield return new WaitForSeconds(shootCD);
        canShoot = true;
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        animator.SetTrigger("TakeDamage");
        healthFill.fillAmount = health / 100f;
        GameManager.instance.PopupDamage(transform, damage);
        if (health <= 0) {
            animator.SetTrigger("Die");
            health = 0;
            gameObject.SetActive(false);
            AudioManager.instance.PlaySFX("PlayerDie");
        }
    }
    public void ChoiLai()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
