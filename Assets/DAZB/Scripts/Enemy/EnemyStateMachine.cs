using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine<T> where T : System.Enum {
    public EnemyState<T> CurrentState {get; private set;}
    public Dictionary<T, EnemyState<T>> StateDictionary = new Dictionary<T, EnemyState<T>>();

    public Enemy enemy;

    public void Initialize(T startState, Enemy enemy) {
        CurrentState = StateDictionary[startState];
        this.enemy = enemy;
        CurrentState.Enter();
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