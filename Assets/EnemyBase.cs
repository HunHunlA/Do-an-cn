using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.UI;
public enum EnemyType
{
    None,
    Slime,
    SlimeForest,
    SlimeFusion,
    WolfAlpha,
    FloweringTreant,
    DryadMage,
}
public class EnemyBase : MonoBehaviour
{
    public string enemyName;
    public EnemyType enemyType;
    [TextArea] public string description;
    [Header("Base")]
    [Space(10)]
    public bool isAttacking;
    public bool isDie;
    public bool isFacingRight;

    [Header("Move")]
    public float moveSpeed;

    [Header("Health")]
    public int maxHealth;
    public int currentHealth;

    [Header("Attack")]
    public int damage;
    public float countTime;
    public float attackTime;
    public Transform firePoint;

    [Header("Animation")]
    public AnimationReferenceAsset idle;
    public AnimationReferenceAsset run;
    public AnimationReferenceAsset attack;
    public AnimationReferenceAsset takeDamage;
    public AnimationReferenceAsset die;

    public GameObject bullet;
    public Image healthFill;


    [Header("Component")]
    protected SkeletonAnimation skeletonAnimation;
    protected Rigidbody2D rb2d;
    protected GameManager gameManager => GameManager.instance;
    protected virtual void Awake()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        rb2d = GetComponent<Rigidbody2D>();
    }
    protected virtual void Start()
    {
        SetIniHealth();
    }
    public void SetIniHealth()
    {
        currentHealth = maxHealth;
    }
    protected virtual void Move()
    {
        float speed = isFacingRight ? moveSpeed : -moveSpeed;
        rb2d.linearVelocity = new Vector2(speed, 0f);
    }
    public void Idle()
    {
        skeletonAnimation.state.SetAnimation(0, idle, true);
    }
    public void Run()
    {
        skeletonAnimation.state.SetAnimation(0, run, true);
    }
    protected virtual void Die()
    {
        isDie = true;
        skeletonAnimation.state.SetAnimation(0, die, false);
    }
    protected virtual void HandleDie()
    {
        if (skeletonAnimation.AnimationState.GetCurrent(0).IsComplete && gameObject != null)
        {
            Destroy(gameObject);
        }
    }
    protected virtual void AutoChangeState() { }
    public virtual void TakeDamage(int value)
    {

        currentHealth -= value;
        if (!isAttacking)
        {
            skeletonAnimation.state.SetAnimation(0, takeDamage, false);
        }

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            GameManager.instance.panel.SetActive(true);
            Die();
        }
        GameManager.instance.PopupDamage(transform, value);
        healthFill.fillAmount = currentHealth / 100f;
    }
    protected virtual void Flip()
    {
        isFacingRight = !isFacingRight;

        //flip obj
        Vector2 newEnemyScale = transform.localScale;
        newEnemyScale.x *= -1f;
        transform.localScale = newEnemyScale;
    }
}