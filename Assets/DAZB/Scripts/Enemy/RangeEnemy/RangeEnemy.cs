using System;
using UnityEngine;

public enum RangeEnemyStateEnum {
    Catched, Battle, Attack, Escape, Dead
}

public class RangeEnemy : Enemy {
    public EnemyStateMachine<RangeEnemyStateEnum> StateMachine {get; private set;}

    [Header("Enemy Setting")]
    public Transform Hand;
    public Transform GunTrm;
    public Transform firePos;
    public float aimingSpeed;
    public float distanceToRun; // escape 일때 도망갈 거리
    public Vector2 playerCheckRange; // escape로 들어갈 범위
    public Vector2 playerCheckOffset; // 오프셋
    public Projectile projectilePrf;
    public float shootPower;
    public float escapeCooldown;

    public Transform body;

    public LineRenderer lineRendererCompo {get; private set;}

    [HideInInspector] public float lastEscapeTime;

    protected override void Awake() {
        base.Awake();
        StateMachine = new EnemyStateMachine<RangeEnemyStateEnum>();

        foreach (RangeEnemyStateEnum stateEnum in Enum.GetValues(typeof(RangeEnemyStateEnum))) {
            string typeName = stateEnum.ToString();
            Type t = Type.GetType($"RangeEnemy{typeName}State");

            try {
                var enemyState = Activator.CreateInstance(t, this, StateMachine, typeName) as EnemyState<RangeEnemyStateEnum>;
                StateMachine.AddState(stateEnum, enemyState);
            }
            catch {
                Debug.LogError($"[Enemy RangeEnemy] : Not Found State [{typeName}]");
            }
        }

        lineRendererCompo = GetComponent<LineRenderer>();
        lineRendererCompo.enabled = false;
    }

    private void Start() {
        StateMachine.Initialize(RangeEnemyStateEnum.Battle, this);
    }

    private void Update() {
        StateMachine.CurrentState.UpdateState();
    }

    public override void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();
    public override void Attack() => StateMachine.CurrentState.AnimationAttackTrigger();

    public Projectile CreateProjectile() {
        return  Instantiate(projectilePrf);
    }

    private bool isFirstEscape = true;
    public bool CanEscape() {
        if (isFirstEscape == true) {
            isFirstEscape = false;
            return true;
        }
        return lastEscapeTime + escapeCooldown <= Time.time;
    }

    protected override void OnDrawGizmos() {
        base.OnDrawGizmos();

        // escape로 들어갈 범위 박스
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube((Vector2)transform.position + playerCheckOffset, playerCheckRange);
    }

    public override void Catched() {
        StateMachine.ChangeState(RangeEnemyStateEnum.Catched);
    }
}