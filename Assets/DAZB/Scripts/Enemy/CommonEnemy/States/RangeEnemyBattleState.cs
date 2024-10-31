using UnityEngine;

public class RangeEnemyBattleState : EnemyState<RangeEnemyStateEnum> {
    private readonly int battleModeHash = Animator.StringToHash("BattleMode");

    private RangeEnemy enemy;
    private Animator anim;

    public RangeEnemyBattleState(Enemy enemy, EnemyStateMachine<RangeEnemyStateEnum> stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        this.enemy = enemy as RangeEnemy;
        anim = enemy.AnimatorCompo;
    }
    private Transform playerTrm;

    private float directionToPlayer;

    public override void Enter() {
        base.Enter();

        enemy.StopImmediately(false);
        playerTrm = PlayerManager.Instance.Player.transform;
        anim.SetFloat(battleModeHash, 0);
    }

    public override void Exit() {
        base.Exit();
    }

    public override void UpdateState() {
        base.UpdateState();
        if (enemy.isDead) stateMachine.ChangeState(RangeEnemyStateEnum.Dead);

        if (enemy.IsPlayerInRange(enemy.canAttackCheckOffset, enemy.canAttackRange)) {
            anim.SetFloat(battleModeHash, 0);

            directionToPlayer = playerTrm.position.x - enemy.transform.position.x;
            enemy.FlipController(directionToPlayer);

            enemy.StopImmediately(false);

            if (enemy.CanAttack()) {
                stateMachine.ChangeState(RangeEnemyStateEnum.Attack);
            }

            if (enemy.IsPlayerInRange(enemy.playerCheckOffset, enemy.playerCheckRange)) {
                if (!enemy.CanAttack()) {
                    stateMachine.ChangeState(RangeEnemyStateEnum.Escape);
                } 
            }
        }
        else {
            anim.SetFloat(battleModeHash, 1);
            Move();
        }
    }

    private void Move() {
        Vector2 dir = (playerTrm.position - enemy.transform.position).normalized;
        enemy.SetVelocity(enemy.moveSpeed * dir.x, enemy.RigidbodyCompo.linearVelocityY);
    }
}