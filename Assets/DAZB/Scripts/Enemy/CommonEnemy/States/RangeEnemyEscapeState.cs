using System.Collections;
using UnityEngine;

public class RangeEnemyEscapeState : EnemyState<RangeEnemyStateEnum> {
    private RangeEnemy enemy;
    
    public RangeEnemyEscapeState(Enemy enemy, EnemyStateMachine<RangeEnemyStateEnum> stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        this.enemy = enemy as RangeEnemy;
    }

    private Transform playerTrm;

    private Vector2 startPosition;

    public override void Enter() {
        base.Enter();

        playerTrm = PlayerManager.Instance.Player.transform;
        startPosition = enemy.transform.position;
    }

    public override void Exit() {
        enemy.lastEscapeTime = Time.deltaTime;

        base.Exit();
    }

    public override void UpdateState() {
        base.UpdateState();

        if (Vector2.Distance(startPosition, enemy.transform.position) > enemy.distanceToRun) {
            stateMachine.ChangeState(RangeEnemyStateEnum.Battle);
            return;
        }

        Move();
    }

    private void Move() {
        Vector2 dir = (enemy.transform.position - playerTrm.transform.position).normalized;
        enemy.SetVelocity(enemy.moveSpeed * dir.x, enemy.RigidbodyCompo.linearVelocityY);
    }
}