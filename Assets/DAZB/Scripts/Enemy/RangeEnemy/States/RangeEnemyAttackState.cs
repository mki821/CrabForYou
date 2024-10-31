using System;
using System.Collections;
using UnityEngine;

public class RangeEnemyAttackState : EnemyState<RangeEnemyStateEnum> {
    private RangeEnemy enemy;

    private Vector3 originalHandLocalPosition;
    private Quaternion originalHandLocalRotation;

    public RangeEnemyAttackState(Enemy enemy, EnemyStateMachine<RangeEnemyStateEnum> stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName) 
    {
        this.enemy = enemy as RangeEnemy;
    }
    
    private Transform playerTrm;
    private float angle;
    private Vector2 direction;

    private bool isShootReady = false;
    private Vector2 finallyShootDir;

    public override void Enter() {
        base.Enter();
        playerTrm = PlayerManager.Instance.Player.transform;
        enemy.lineRendererCompo.enabled = true;

        originalHandLocalPosition = enemy.Hand.transform.localPosition;
        originalHandLocalRotation = enemy.Hand.transform.localRotation;

        enemy.StartDelayCallback(2f, () => {
            isShootReady = true;
            enemy.StartCoroutine(ShootRoutine());
        });
    }

    public override void Exit() {
        enemy.lastAttackTime = Time.time;
        enemy.lineRendererCompo.enabled = false;

        enemy.Hand.transform.localPosition = originalHandLocalPosition;
        enemy.Hand.transform.localRotation = originalHandLocalRotation;

        isShootReady = false;
        base.Exit();
    }

    public override void UpdateState() {
        base.UpdateState();

        if (enemy.isDead) {
            stateMachine.ChangeState(RangeEnemyStateEnum.Dead);
            return;
        }

        if (isShootReady) return;

        direction = playerTrm.position - enemy.transform.position;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        enemy.FlipController(direction.x);

        Vector3 startLaserPosition = enemy.firePos.position;
        Vector3 targetLaserPosition = playerTrm.position;
        Vector3 smoothEndPosition = Vector3.Lerp(enemy.lineRendererCompo.GetPosition(1), targetLaserPosition, enemy.aimingSpeed * Time.deltaTime);

        Vector3[] laserPositions = { startLaserPosition, smoothEndPosition };
        enemy.lineRendererCompo.SetPositions(laserPositions);

        Vector3 targetPosition = enemy.transform.position + new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * 1.5f, Mathf.Sin(angle * Mathf.Deg2Rad) * 1.5f);
        enemy.Hand.transform.position = Vector3.Lerp(enemy.Hand.transform.position, targetPosition, enemy.aimingSpeed * Time.deltaTime);

        finallyShootDir = smoothEndPosition - startLaserPosition;
        float smoothAngle = Mathf.Atan2(finallyShootDir.y, finallyShootDir.x) * Mathf.Rad2Deg;
        enemy.Hand.transform.rotation = Quaternion.Euler(0, 0, smoothAngle);
    }

    private IEnumerator ShootRoutine() {
        yield return new WaitForSeconds(0.5f);
        Projectile projectile = enemy.CreateProjectile();
        projectile.Shoot(enemy.firePos.position, finallyShootDir, enemy.shootPower);

        stateMachine.ChangeState(RangeEnemyStateEnum.Battle);
    }

    public override void AnimationAttackTrigger() {
        base.AnimationAttackTrigger();
    }
}
