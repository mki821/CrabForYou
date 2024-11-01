using System.Collections;
using UnityEngine;

public class DashEnemyAttackState : EnemyState<DashEnemyStateEnum> {
    DashEnemy enemy;

    public DashEnemyAttackState(Enemy enemy, EnemyStateMachine<DashEnemyStateEnum> stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        this.enemy = enemy as DashEnemy;
    }
    
    public override void Enter() {
        base.Enter();

        enemy.isAttack = true;

        enemy.StartCoroutine(DashRoutine());
    }

    public override void Exit() {
        enemy.lastAttackTime = Time.time;

        enemy.isAttack = false;

        base.Exit();
    }

    public override void UpdateState() {
        base.UpdateState();
    }

    private IEnumerator DashRoutine() {
        yield return new WaitForSeconds(0.5f);

        float elapseTime = 0;
        float targetTime = 0.4f;

        float t;
        float n = 1f;

        Vector2 startPos = enemy.transform.position;
        Vector2 endPos = new Vector2(enemy.transform.position.x + (enemy.canAttackRange.x + n) * enemy.FacingDirection, enemy.transform.position.y);

        SoundManager.Instance.PlaySFX("SHOOK");
        while (elapseTime < targetTime) {
            t = easeOutExpo(elapseTime / targetTime);
            enemy.transform.position = Vector2.Lerp(startPos, endPos, t);
            elapseTime += Time.deltaTime;

            yield return null;
        }
        stateMachine.ChangeState(DashEnemyStateEnum.Battle);
    }

    private float easeOutExpo(float x)  {
        return x == 1 ? 1 : 1 - Mathf.Pow(2, -10 * x);
    }
}