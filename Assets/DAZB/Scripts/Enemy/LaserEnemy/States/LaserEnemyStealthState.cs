using System.Collections;
using UnityEngine;

public class LaserEnemyStealthState : EnemyState<LaserEnemyStateEnum> {
    private LaserEnemy enemy;
    private SpriteRenderer sr;

    public LaserEnemyStealthState(Enemy enemy, EnemyStateMachine<LaserEnemyStateEnum> stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        this.enemy = enemy as LaserEnemy;
        sr = enemy.SpriteRendererCompo;
    }

    private Transform playerTrm;
    private Vector2 direction;

    public override void Enter() {
        base.Enter();

        playerTrm = PlayerManager.Instance.Player.transform;
        direction = (playerTrm.position - enemy.transform.position).normalized;

        Color color = sr.color;
        color.a = 0.2f;
        sr.color = color;

        enemy.StartDelayCallback(1.5f, () => stateMachine.ChangeState(LaserEnemyStateEnum.Battle));
    }

    public override void Exit() {
        Color color = sr.color;
        color.a = 1;
        sr.color = color;

        base.Exit();
    }

    public override void UpdateState() {
        base.UpdateState();
        Move();
    }

    private void Move() {
        enemy.SetVelocity(10 * direction.x, enemy.RigidbodyCompo.linearVelocityY);
    }
}