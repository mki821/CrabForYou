using System.Collections;
using UnityEngine;

public class BossPattern2State : EnemyState<BossStateEnum> {
    private Boss boss;

    public BossPattern2State(Enemy enemy, EnemyStateMachine<BossStateEnum> stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        boss = enemy as Boss;
    }

    public override void Enter() {
        base.Enter();

        enemy.StartCoroutine(PatternRoutine());
    }

    private IEnumerator PatternRoutine() {
        float elapseTime = 0;
        float targetTime = 0.5f;

        Transform hand = boss.leftHandTrm;
        Transform parentTrm = hand.parent;
        hand.SetParent(null);

        Vector2 startPos = hand.transform.position;
        Vector2 endPos = new Vector2(10, hand.transform.position.y + 3);

        Vector2 originalPos = hand.position;

        while (elapseTime < targetTime) {
            float t = elapseTime / targetTime;
            
            hand.transform.position = Vector2.Lerp(startPos, endPos, t);

            elapseTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(2f);

        int rand = Random.Range(0, boss.enemyPrefabs.Count);
        Enemy enemy = boss.CreateObject(boss.enemyPrefabs[rand]);
        enemy.gameObject.SetActive(true);
        enemy.transform.position = Vector2.zero;
        enemy.transform.SetParent(hand);
        enemy.enabled = false;

        elapseTime = 0;
        targetTime = 0.5f;

        startPos = hand.transform.position;
        endPos = new Vector2(0, 0);

        while (elapseTime < targetTime) {
            float t = elapseTime / targetTime;
            hand.transform.position = Vector2.Lerp(startPos, endPos, t);

            elapseTime += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(1f);

        enemy.transform.SetParent(null);
        enemy.enabled = true;

        elapseTime = 0;
        targetTime = 0.5f;

        startPos = hand.transform.position;
        endPos = originalPos;

        while (elapseTime < targetTime) {
            float t = elapseTime / targetTime;
            hand.transform.position = Vector2.Lerp(startPos, endPos, t);

            elapseTime += Time.deltaTime;
            yield return null;
        }


        hand.SetParent(parentTrm);

        stateMachine.ChangeState(BossStateEnum.WaitPattern);
    }
}