using UnityEngine;

public class EnemyState<T> where T : System.Enum {
    protected EnemyStateMachine<T> stateMachine;
    protected Enemy enemy;

    protected int animBoolHash;
    protected bool animEndTrigger;

    public EnemyState(Enemy enemy, EnemyStateMachine<T> stateMachine, string animBoolName) {
        this.enemy = enemy;
        this.stateMachine = stateMachine;
        animBoolHash = Animator.StringToHash(animBoolName);
    }

    public virtual void Enter() {
        animEndTrigger = false;
        enemy.AnimatorCompo.SetBool(animBoolHash, true);
    }

    public virtual void Exit() {
        enemy.AnimatorCompo.SetBool(animBoolHash, false);
    }

    public virtual void UpdateState() { }
    public virtual void AnimationAttackTrigger() { }
    public void AnimationFinishTrigger() => animEndTrigger = true;

}