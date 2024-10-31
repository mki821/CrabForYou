using UnityEngine;

public class DroneEnemyBattleState : EnemyState<DroneEnemyStateEnum> {
    private readonly int battleModeHash = Animator.StringToHash("BattleMode");

    private DroneEnemy enemy;
    private Animator anim;
    
    public DroneEnemyBattleState(Enemy enemy, EnemyStateMachine<DroneEnemyStateEnum> stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        this.enemy = enemy as DroneEnemy;
        anim = enemy.AnimatorCompo;
    }

    
    private Transform playerTrm;
    private float directionToPlayer;

    public override void Enter() {
        base.Enter();

        playerTrm = PlayerManager.Instance.Player.transform;
    }

    public override void Exit() {
        base.Exit();
    }

    public override void UpdateState() {
        base.UpdateState();
        if (enemy.isDead) stateMachine.ChangeState(DroneEnemyStateEnum.Dead);

        if (enemy.IsPlayerInRange(enemy.canAttackCheckOffset, enemy.canAttackRange)) {
            anim.SetFloat(battleModeHash, 0);

            directionToPlayer = playerTrm.position.x - enemy.transform.position.x;
            enemy.FlipController(directionToPlayer);

            enemy.StopImmediately(false);

            if (enemy.CanAttack()) {
                stateMachine.ChangeState(DroneEnemyStateEnum.Attack);
            }
        }
        else {
            anim.SetFloat(battleModeHash, 1);
            Move();
        }
    }

    private void Move() {
        Vector2 dir = (playerTrm.position - enemy.transform.position).normalized;
        enemy.SetVelocity(enemy.moveSpeed * dir.x, enemy.moveSpeed * dir.y);
    }
}