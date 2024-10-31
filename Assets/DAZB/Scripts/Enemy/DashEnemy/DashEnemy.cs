using System;
using UnityEngine;

public enum DashEnemyStateEnum {
    Catched, Battle, Attack, Dead
}

public class DashEnemy : Enemy {
    public EnemyStateMachine<DashEnemyStateEnum> StateMachine {get; private set;}

    protected override void Awake() {
        base.Awake();
        StateMachine = new EnemyStateMachine<DashEnemyStateEnum>();

        foreach (DashEnemyStateEnum stateEnum in Enum.GetValues(typeof(DashEnemyStateEnum))) {
            string typeName = stateEnum.ToString();
            Type t = Type.GetType($"DashEnemy{typeName}State");

            try {
                var enemyState = Activator.CreateInstance(t, this, StateMachine, typeName) as EnemyState<DashEnemyStateEnum>;
                StateMachine.AddState(stateEnum, enemyState);
            }
            catch {
                Debug.LogError($"[Enemy DashEnemy] : Not Found State [{typeName}]");
            }
        }
    }

    private void Start() {
        StateMachine.Initialize(DashEnemyStateEnum.Battle, this);
    }

    private void Update() {
        StateMachine.CurrentState.UpdateState();
    }

    public override void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();
    public override void Attack() => StateMachine.CurrentState.AnimationAttackTrigger();
}
