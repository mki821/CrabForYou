using System;
using UnityEngine;

public enum DroneEnemyStateEnum {
    Catched, Asphyxia, Battle, Attack, Dead
}

public class DroneEnemy : Enemy {
    public EnemyStateMachine<DroneEnemyStateEnum> StateMachine {get; private set;}

    public bool isAttack = false;

    protected override void Awake() {
        base.Awake();
        StateMachine = new EnemyStateMachine<DroneEnemyStateEnum>();

        foreach (DroneEnemyStateEnum stateEnum in Enum.GetValues(typeof(DroneEnemyStateEnum))) {
            string typeName = stateEnum.ToString();
            Type t = Type.GetType($"DroneEnemy{typeName}State");

            try {
                var enemyState = Activator.CreateInstance(t, this, StateMachine, typeName) as EnemyState<DroneEnemyStateEnum>;
                StateMachine.AddState(stateEnum, enemyState);
            }
            catch {
                Debug.LogError($"[Enemy DroneEnemy] : Not Found State [{typeName}]");
            }
        }
    }

    private void Start() {
        StateMachine.Initialize(DroneEnemyStateEnum.Battle, this);
    }

    private void Update() {
        StateMachine.CurrentState.UpdateState();
    }

    public override void AnimationFinishTrigger() => StateMachine.CurrentState.AnimationFinishTrigger();
    public override void Attack() => StateMachine.CurrentState.AnimationAttackTrigger();

    public override void Catched() {
        StateMachine.ChangeState(DroneEnemyStateEnum.Catched);
    }

     private void OnTriggerEnter2D(Collider2D other) {
        if (isAttack) {
            if (other.TryGetComponent<IDamageable>(out IDamageable component) && other.gameObject.layer == LayerMask.NameToLayer("Player")) {
                component.ApplyDamage();
            }
        }
    }
}
