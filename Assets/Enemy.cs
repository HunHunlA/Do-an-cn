using Spine.Unity;
using System.Collections;
using System.ComponentModel;
using UnityEditor.EditorTools;
using UnityEngine;

public class Enemy : EnemyBase
{
    [Header("Dryad Mage")]
    [Space(10)]
    public bool canSpecialAttack;
    public bool usedSpecialAttack;
    public AnimationReferenceAsset specialAttack;
    private BoxCollider2D boxCollider;

    protected override void Awake()
    {
        base.Awake();
        boxCollider = GetComponent<BoxCollider2D>();
    }
    protected override void Start()
    {
        base.Start();
        attackTime = Random.Range(attackTime - 0.5f, attackTime + 0.5f);
        skeletonAnimation.timeScale = 2;
        skeletonAnimation.AnimationState.SetAnimation(0, idle, true);
    }
    private void Update()
    {
        if (isDie)
        {
            HandleDie();
        }
        else
        {
            AutoChangeState();
            CountShootTime();
            ActiveSpecialShoot(currentHealth, maxHealth);
        }
    }
    protected override void AutoChangeState()
    {
        if (skeletonAnimation.AnimationState.GetCurrent(0).IsComplete)
        {
            Idle();
        }
    }
    public void CountShootTime()
    {
        if (countTime < attackTime) countTime += Time.deltaTime;
        else Attack();
    }
    public IEnumerator SpecialShoot()
    {
        countTime = 0;
        skeletonAnimation.state.SetAnimation(0, specialAttack, false);
        float duration = attack.Animation.Duration;
        yield return new WaitForSeconds(duration / 1.75f);
        AudioManager.instance.PlaySFX("EnemyShoot");
        Instantiate(bullet, firePoint.position, Quaternion.identity);
        usedSpecialAttack = true;
    }
    public void Attack()
    {
        if (!canSpecialAttack) StartCoroutine(Shoot());
        else StartCoroutine(SpecialShoot());

        Vector2 direction = (GameManager.instance.player.transform.position - transform.position).normalized;
        if(direction.x < 0 && isFacingRight || direction.x > 0 && !isFacingRight)
        {
            isFacingRight = !isFacingRight;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }

    }
    public void ActiveSpecialShoot(int currentHealth, int maxHealth)
    {
        if (!usedSpecialAttack)
        {
            if (maxHealth / currentHealth >= 2)
            {
                canSpecialAttack = true;
            }
        }
    }
    public IEnumerator Shoot()
    {
        isAttacking = true;
        countTime = 0;
        skeletonAnimation.state.SetAnimation(0, attack, false);
        float duration = attack.Animation.Duration;
        yield return new WaitForSeconds(duration / (skeletonAnimation.timeScale * 2f));
        AudioManager.instance.PlaySFX("EnemyShoot");

        Instantiate(bullet, firePoint.position, Quaternion.identity);
        isAttacking = false;
    }
    protected override void Die()
    {
        boxCollider.enabled = false;
        base.Die();
    }
}
