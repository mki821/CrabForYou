using System.Collections.Generic;

public class EnemyStateMachine<T> where T : System.Enum {
    public EnemyState<T> CurrentState {get; private set;}
    public Dictionary<T, EnemyState<T>> StateDictionary = new Dictionary<T, EnemyState<T>>();

    public Enemy enemy;

    public void Initialize(T startState, Enemy enemy) {
        CurrentState = StateDictionary[startState];
        CurrentState.Enter();
        this.enemy = enemy;
        
    }

    public void ChangeState(T newState) {
        if (enemy.CanStateChangeable == false) return;
        CurrentState.Exit();
        CurrentState = StateDictionary[newState];
        CurrentState.Enter();
    }

    public void AddState(T stateEnum, EnemyState<T> enemyState) {
        StateDictionary.Add(stateEnum, enemyState);
    }
}