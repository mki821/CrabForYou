using System;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.InputSystem;

public enum LaserEnemyStateEnum {
    Catched, Battle, Attack, Stealth, Dead
}

public class LaserEnemy : Enemy {
    public EnemyStateMachine<LaserEnemyStateEnum> StateMachine;

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

        // 테스트 코드
        if (Keyboard.current.tKey.wasPressedThisFrame) {
            StateMachine.ChangeState(LaserEnemyStateEnum.Stealth);
        }
    }

    public override void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();
    public override void Attack() => StateMachine.CurrentState.AnimationAttackTrigger();
}
