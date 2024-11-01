using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction {
    Up = 1, Down = 2, Right = 4, Left = 8
}

public class Map : MonoBehaviour
{
    public Portal upPortal;
    public Portal downPortal;
    public Portal rightPortal;
    public Portal leftPortal;
    
    [SerializeField] private List<Enemy> _enemyList;

    public GameObject icon;
    private int direction;

    public Vector2Int mapPosition;

    private bool _isVisited = false;
    private int _enemyCount = 0;

    private void Awake() {
        upPortal.Init(this);
        downPortal.Init(this);
        rightPortal.Init(this);
        leftPortal.Init(this);
    }

    public int GenerateRandom() {
        int direction = 0;

        if(Random.Range(0, 2) == 1) {
            direction |= (int)Direction.Up;
        }
        if(Random.Range(0, 2) == 1) {
            direction |= (int)Direction.Down;
        }
        if(Random.Range(0, 2) == 1) {
            direction |= (int)Direction.Right;
        }
        if(Random.Range(0, 2) == 1) {
            direction |= (int)Direction.Left;
        }

        return direction;
    }

    public void ActovePortal(int direction) {
        this.direction = direction;

        if((direction & (int)Direction.Up) > 0)
            upPortal.gameObject.SetActive(true);
        if((direction & (int)Direction.Down) > 0)
            downPortal.gameObject.SetActive(true);
        if((direction & (int)Direction.Right) > 0)
            rightPortal.gameObject.SetActive(true);
        if((direction & (int)Direction.Left) > 0)
            leftPortal.gameObject.SetActive(true);
    }

    private void OpenPortal() {
        if((direction & (int)Direction.Up) > 0)
            upPortal.Open();
        if((direction & (int)Direction.Down) > 0)
            downPortal.Open();
        if((direction & (int)Direction.Right) > 0)
            rightPortal.Open();
        if((direction & (int)Direction.Left) > 0)
            leftPortal.Open();
    }

    private void ClosePortal() {
        if((direction & (int)Direction.Up) > 0)
            upPortal.Close();
        if((direction & (int)Direction.Down) > 0)
            downPortal.Close();
        if((direction & (int)Direction.Right) > 0)
            rightPortal.Close();
        if((direction & (int)Direction.Left) > 0)
            leftPortal.Close();
    }

    public void EnterPlayer() {
        if(_isVisited) return;

        _isVisited = true;

        if(_enemyList.Count > 0) {
            ClosePortal();
            StartCoroutine(SpawnEnemy());
        }
    }

    private IEnumerator SpawnEnemy() {
        yield return new WaitForSeconds(1f);
        
        _enemyCount = _enemyList.Count;
        for(int i = 0; i < _enemyList.Count; ++i) {
            _enemyList[i].gameObject.SetActive(true);
            _enemyList[i].DeadEvent += (enemy) => {
                _enemyList.Remove(enemy);
                DecreaseCount();
            };
        }
    }

    private void DecreaseCount() {
        --_enemyCount;

        if(_enemyCount == 0) {
            OpenPortal();
        }
    }
}
