using System.Collections;
using UnityEngine;

public class LaserEnemyAttackState : EnemyState<LaserEnemyStateEnum> {
    LaserEnemy enemy;

    private Vector3 originalHandLocalPosition;
    private Quaternion originalHandLocalRotation;

    public LaserEnemyAttackState(Enemy enemy, EnemyStateMachine<LaserEnemyStateEnum> stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        this.enemy = enemy as LaserEnemy;
    }

    Transform playerTrm;

    private float angle;
    private Vector2 direction;

    private bool isShootReady = false;
    private Vector2 finallyShootDir;

    public override void Enter() {
        base.Enter();

        playerTrm = PlayerManager.Instance.Player.transform;

        originalHandLocalPosition = enemy.Hand.transform.localPosition;
        originalHandLocalRotation = enemy.Hand.transform.localRotation;

        enemy.StartDelayCallback(2f, () => {
            isShootReady = true;
            enemy.lineRendererCompo.enabled = true;
            enemy.StartCoroutine(AttackRoutine());
        });
    }

    public override void Exit() {
        enemy.lineRendererCompo.enabled = false;
        enemy.lastAttackTime = Time.time;


        isShootReady = false;
        base.Exit();
    }

    public override void UpdateState() {
        base.UpdateState();

        if (enemy.isDead) {
            stateMachine.ChangeState(LaserEnemyStateEnum.Dead);
            return;
        }

        if (isShootReady) return;

        direction = playerTrm.position - enemy.body.transform.position;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        enemy.FlipController(direction.x);

        Vector3 startLaserPosition = enemy.firePos.position;
        Vector3 targetLaserPosition = playerTrm.position;
        Vector3 smoothEndPosition = Vector3.Lerp(enemy.lineRendererCompo.GetPosition(1), targetLaserPosition, enemy.aimingSpeed * Time.deltaTime);

        Vector3[] laserPositions = { startLaserPosition, smoothEndPosition };
        enemy.lineRendererCompo.SetPositions(laserPositions);

        Vector3 targetPosition = enemy.body.transform.position + new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad) * 1.5f, Mathf.Sin(angle * Mathf.Deg2Rad) * 1.5f);
        enemy.Hand.transform.position = Vector3.Lerp(enemy.Hand.transform.position, targetPosition, enemy.aimingSpeed * Time.deltaTime);

        finallyShootDir = smoothEndPosition - startLaserPosition;
        float smoothAngle = Mathf.Atan2(finallyShootDir.y, finallyShootDir.x) * Mathf.Rad2Deg + 120;
        enemy.Hand.transform.rotation = Quaternion.Euler(0, 0, smoothAngle);
    }

    private IEnumerator AttackRoutine() {
        float elapseTime = 0;
        float targetTime = 1;

        Vector2 direction = playerTrm.position - enemy.firePos.transform.position;

        Vector2 rotatedDirection1;
        Vector2 rotatedDirection2;

        enemy.lineRendererCompo.startWidth = 0.05f;
        enemy.lineRendererCompo.endWidth = 0.05f;

        float angle;

        while (elapseTime < targetTime) {
            float t = easeOutCirc(elapseTime / targetTime);
            angle = Mathf.Lerp(60 , 0, t);

            rotatedDirection1 = enemy.firePos.transform.position + (Vector3)Rotate(direction.normalized * 1000f, angle);
            rotatedDirection2 = enemy.firePos.transform.position + (Vector3)Rotate(direction.normalized * 1000f, -angle);

            // LineRenderer에 4개의 포인트 설정
            enemy.lineRendererCompo.positionCount = 4;
            enemy.lineRendererCompo.SetPosition(0, enemy.firePos.transform.position);
            enemy.lineRendererCompo.SetPosition(1, rotatedDirection1);
            enemy.lineRendererCompo.SetPosition(2, enemy.firePos.transform.position);
            enemy.lineRendererCompo.SetPosition(3, rotatedDirection2);

            elapseTime += Time.deltaTime;

            yield return null;
        }

        elapseTime = 0;
        targetTime = 0.1f;

        enemy.lineRendererCompo.positionCount = 2;
        enemy.lineRendererCompo.SetPosition(0, enemy.firePos.transform.position);
        enemy.lineRendererCompo.SetPosition(1, direction * 1000f);

        float width;

        while (elapseTime < targetTime) {
            float t = easeOutCirc(elapseTime / targetTime);
            width = Mathf.Lerp(0.1f, 0.5f, t);
            enemy.lineRendererCompo.startWidth = width;
            enemy.lineRendererCompo.endWidth = width;

            elapseTime += Time.deltaTime;
            yield return null;
        }

        // 여기에 캐스트
        yield return new WaitForSeconds(3f);

        
        elapseTime = 0;
        targetTime = 0.2f;

        while (elapseTime < targetTime) {
            float t = easeInCirc(elapseTime / targetTime);
            width = Mathf.Lerp(0.5f, 0f, t);
            enemy.lineRendererCompo.startWidth = width;
            enemy.lineRendererCompo.endWidth = width;

            elapseTime += Time.deltaTime;
            yield return null;
        }

        elapseTime = 0;
        targetTime = 0.5f;

        Vector3 startHandPosition = enemy.Hand.transform.localPosition;
        Quaternion startHandRotation = enemy.Hand.transform.localRotation;

        while (elapseTime < targetTime) {
            float t = elapseTime / targetTime;

            enemy.Hand.transform.localPosition = Vector3.Lerp(startHandPosition, originalHandLocalPosition, t);
            enemy.Hand.transform.localRotation = Quaternion.Slerp(startHandRotation, originalHandLocalRotation, t);

            elapseTime += Time.deltaTime;
            yield return null;
        }

        enemy.Hand.transform.localPosition = originalHandLocalPosition;
        enemy.Hand.transform.localRotation = originalHandLocalRotation;

        stateMachine.ChangeState(LaserEnemyStateEnum.Battle);
    }

    // Vector2에 각도를 추가하는 Rotate 함수
    private Vector2 Rotate(Vector2 vector, float angle) {
        float radian = angle * Mathf.Deg2Rad;
        float cos = Mathf.Cos(radian);
        float sin = Mathf.Sin(radian);

        return new Vector2(
            cos * vector.x - sin * vector.y,
            sin * vector.x + cos * vector.y
        );
    }

    private float easeInCirc(float x) {
        return 1 - Mathf.Sqrt(1 - Mathf.Pow(x, 2));
    }

    private float easeOutCirc(float x) {
        return Mathf.Sqrt(1 - Mathf.Pow(x - 1, 2));
    }
}