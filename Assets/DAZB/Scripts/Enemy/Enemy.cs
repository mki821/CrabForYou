using System;
using System.Net.Http.Headers;
using UnityEngine;

public abstract class Enemy : Entity, IDamageable {
    public LayerMask whatIsPlayer;

    public float moveSpeed;


    [Header("Attack Setting")]
    public float attackCooldown; // 보스는 패턴 쿨타임
    [HideInInspector] public float lastAttackTime;

    [Header("Detect Value Setting")]
    public Vector2 canAttackRange;
    public Vector2 canAttackCheckOffset;

    [SerializeField] private int health;
    [HideInInspector] public bool isCatchCanceled;

    public DamageCaster damageCasterCompo {get; private set;}

    public event Action<Enemy> DeadEvent;

    protected override void Awake()
    {
        base.Awake();

        damageCasterCompo = GetComponent<DamageCaster>();
    }

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

    public T CreateObject<T>(T prefab) where T : UnityEngine.Object {
        return Instantiate(prefab);
    }

    public void ApplyDamage(int amount)
    {
        health -= amount;

        if (health <= 0) {
            isDead = true;
            DeadEvent?.Invoke(this);
        }
    }
    public abstract void Catched();
}
