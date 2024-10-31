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

    public GameObject icon;

    public Vector2Int mapPosition;

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

    public void OpenPortal(int direction) {
        if((direction & (int)Direction.Up) > 0)
            upPortal.gameObject.SetActive(true);
        if((direction & (int)Direction.Down) > 0)
            downPortal.gameObject.SetActive(true);
        if((direction & (int)Direction.Right) > 0)
            rightPortal.gameObject.SetActive(true);
        if((direction & (int)Direction.Left) > 0)
            leftPortal.gameObject.SetActive(true);
    }
}
