using System.Net.Http.Headers;
using UnityEngine;

public abstract class Enemy : Entity, IDamageable {
    public LayerMask whatIsPlayer;

    public float moveSpeed;


    [Header("Attack Setting")]
    public float attackCooldown;
    [HideInInspector] public float lastAttackTime;

    [Header("Detect Value Setting")]
    public Vector2 canAttackRange;
    public Vector2 canAttackCheckOffset;

    [SerializeField] private int _health;
    [HideInInspector] public bool isCatchCanceled;

    public abstract void AnimationFinishTrigger();
    public abstract void Attack();

    public virtual bool IsPlayerInRange(Vector2 checkOffset, Vector2 checkRange) {
        return Physics2D.OverlapBox((Vector2)transform.position + checkOffset * FacingDirection, checkRange, 0, whatIsPlayer);
    }

    private bool isFirstAttack = true;
    public bool CanAttack() {
        if (isFirstAttack == true) {
            isFirstAttack = false;
            return true;
        }
        return lastAttackTime + attackCooldown <= Time.time;
    }

    protected virtual void OnDrawGizmos() {
        // Can Attack Range;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube((Vector2)transform.position + canAttackCheckOffset * FacingDirection, canAttackRange);
    }

    public void ApplyDamage()
    {
        return;
    }

    public void ApplyDamage(int amount)
    {
        _health -= amount;
    }
    public abstract void Catched();
}
