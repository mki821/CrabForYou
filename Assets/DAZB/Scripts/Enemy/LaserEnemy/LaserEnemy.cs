using System;
using UnityEngine;

public enum LaserEnemyStateEnum {
    Catched, Battle, Attack, Stealth, Dead
}

public class LaserEnemy : Enemy {
    public EnemyStateMachine<LaserEnemyStateEnum> StateMachine;

    public Transform firePos;
    public Transform Hand;
    public float aimingSpeed;

    public LineRenderer lineRendererCompo {get; private set;}

    protected override void Awake() {
        base.Awake();
        StateMachine = new EnemyStateMachine<LaserEnemyStateEnum>();

        foreach (LaserEnemyStateEnum stateEnum in Enum.GetValues(typeof(LaserEnemyStateEnum))) {
            string typeName = stateEnum.ToString();
            Type t = Type.GetType($"LaserEnemy{typeName}State");

            try {
                var enemyState = Activator.CreateInstance(t, this, StateMachine, typeName) as EnemyState<LaserEnemyStateEnum>;
                StateMachine.AddState(stateEnum, enemyState);
            }
            catch {
                Debug.LogError($"[Enemy LaserEnemy] : Not Found State [{typeName}]");
            }
        }

        lineRendererCompo = GetComponent<LineRenderer>();
        lineRendererCompo.enabled = false;
    }

    private void Start() {
        StateMachine.Initialize(LaserEnemyStateEnum.Battle, this);
    }

    private void Update() {
        StateMachine.CurrentState.UpdateState();
    }

    public override void Catched() {
        StateMachine.ChangeState(LaserEnemyStateEnum.Catched);
    }

    public override void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();
    public override void Attack() => StateMachine.CurrentState.AnimationAttackTrigger();
}
