using System;
using UnityEngine;

public enum BossStateEnum {
    WaitPatten, Patten1, Patten2, Patten3
}

public class Boss : Enemy {
    public EnemyStateMachine<BossStateEnum> StateMachine {get; private set;}

    [Header("Boss Setting")]
    public Transform leftEyeTrm;
    public Transform rightEyeTrm;
    public Transform leftHandTrm;
    public Transform rightHandTrm;

    [HideInInspector] public float lastPattenTime;

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
    }

    private void Start() {
        StateMachine.Initialize(BossStateEnum.WaitPatten, this);
    }

    private void Update() {
        StateMachine.CurrentState.UpdateState();
    }

    public bool CanPattenStart() {
        return lastPattenTime + attackCooldown <= Time.time;
    }

    public override void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();
    public override void Attack() => StateMachine.CurrentState.AnimationAttackTrigger();


    public override void Catched() { }
}
