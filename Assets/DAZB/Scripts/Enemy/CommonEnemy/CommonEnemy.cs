using System;
using UnityEngine;

public enum CommonEnemyStateEnum {
    Battle, Attack, Dead
}

public class CommonEnemy : Enemy {
    public EnemyStateMachine<CommonEnemyStateEnum> StateMachine {get; private set;}

    protected override void Awake() {
        base.Awake();
        StateMachine = new EnemyStateMachine<CommonEnemyStateEnum>();

        foreach (CommonEnemyStateEnum stateEnum in Enum.GetValues(typeof(CommonEnemyStateEnum))) {
            string typeName = stateEnum.ToString();
            Type t = Type.GetType($"CommonEnemy{typeName}State");

            try {
                var enemyState = Activator.CreateInstance(t, this, StateMachine, typeName) as EnemyState<CommonEnemyStateEnum>;
                StateMachine.AddState(stateEnum, enemyState);
            }
            catch {
                Debug.LogError($"[Enemy CommonEnemy] : Not Found State [{typeName}]");
            }
        }
    }

    private void Start() {
        StateMachine.Initialize(CommonEnemyStateEnum.Battle, this);
    }

    private void Update() {
        StateMachine.CurrentState.UpdateState();
    }

    public override void AnimationFinishTrigger() {

    }

    public override void Attack() {

    }
}