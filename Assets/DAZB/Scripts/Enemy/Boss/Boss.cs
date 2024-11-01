using System;
using System.Collections.Generic;
using UnityEngine;

public enum BossStateEnum {
    WaitPattern, Pattern1, Pattern2, Pattern3, Dead
}

public class Boss : Enemy {
    public EnemyStateMachine<BossStateEnum> StateMachine {get; private set;}

    [Header("Boss Setting")]
    public Transform leftEyeTrm;
    public Transform rightEyeTrm;
    public Transform leftHandTrm;
    public Transform rightHandTrm;
    
    [Header("Pattern1 Setting")]
    public Transform leftHandAttackTrm;
    public Transform rightHandAttackTrm;

    [Header("Pattern2 Setting")]
    public List<Enemy> enemyPrefabs;

    [HideInInspector] public float lastPattenTime;
    public Particle deadParticle;

    public LineRenderer lineRendererCompo;

    protected override void Awake() {
        base.Awake();
        StateMachine = new EnemyStateMachine<BossStateEnum>();

        foreach (BossStateEnum stateEnum in Enum.GetValues(typeof(BossStateEnum))) {
            string typeName = stateEnum.ToString();
            Type t = Type.GetType($"Boss{typeName}State");

            try {
                var enemyState = Activator.CreateInstance(t, this, StateMachine, typeName) as EnemyState<BossStateEnum>;
                StateMachine.AddState(stateEnum, enemyState);
            }
            catch {
                Debug.LogError($"[Enemy Boss] : Not Found State [{typeName}]");
            }
        }

                
        leftHandAttackTrm.SetParent(null);
        rightHandAttackTrm.SetParent(null);

        lineRendererCompo = GetComponent<LineRenderer>();
        lineRendererCompo.enabled = false;
    }

    private void Start() {
        StateMachine.Initialize(BossStateEnum.WaitPattern, this);
    }

    private void Update() {
        StateMachine.CurrentState.UpdateState();

        if (isDead) {
            StateMachine.ChangeState(BossStateEnum.Dead);
        }
    }

    public bool CanPatternStart() {
        return lastPattenTime + attackCooldown <= Time.time;
    }

    public override void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();
    public override void Attack() => StateMachine.CurrentState.AnimationAttackTrigger();

    public override void Catched() { }
}
