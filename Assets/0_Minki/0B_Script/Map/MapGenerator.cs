using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private List<Map> _mapPrefabs;

    [SerializeField] private Vector2 _mapOffset;
    [SerializeField] private Vector2Int _mapMaxSize;

    [SerializeField] private Transform _minimapTrm;
    [SerializeField] private Image _mapUI;

    private Map[,] _maps;
    private bool[,] _mapGenerated;

    private void Awake() {
        _mapGenerated = new bool[_mapMaxSize.y, _mapMaxSize.x];
        _maps = new Map[_mapMaxSize.y, _mapMaxSize.x];
    }

    private void Start() {
        Generate(new Vector2Int(_mapMaxSize.x / 2, _mapMaxSize.y / 2));
        CorrectAllMap();
        SetUI();
    }

    public void Generate(Vector2Int position) {
        if(position.x >= _mapMaxSize.x || position.x < 0 || position.y >= _mapMaxSize.y || position.y < 0) return;
        if(_mapGenerated[position.y, position.x]) return;
        _mapGenerated[position.y, position.x] = true;

        Map randomMap = _mapPrefabs[Random.Range(0, _mapPrefabs.Count)];
        Map generatedMap = Instantiate(randomMap, new Vector2(position.x * _mapOffset.x, position.y * _mapOffset.y), Quaternion.identity);

        _maps[position.y, position.x] = generatedMap;

        int generatedDirection = generatedMap.GenerateRandom();

        if((generatedDirection & (int)Direction.Up) > 0)
            Generate(position + Vector2Int.up);
        if((generatedDirection & (int)Direction.Down) > 0)
            Generate(position + Vector2Int.down);
        if((generatedDirection & (int)Direction.Right) > 0)
            Generate(position + Vector2Int.right);
        if((generatedDirection & (int)Direction.Left) > 0)
            Generate(position + Vector2Int.left);
    }

    private void CorrectAllMap() {
        for(int i = 0; i < _mapMaxSize.y; ++i) {
            for(int j = 0; j < _mapMaxSize.x; ++j) {
                if(_maps[i, j] == null) continue;

                int openDirection = 0;
                if(i < _mapMaxSize.y - 1 && _mapGenerated[i + 1, j]) {
                    openDirection |= (int)Direction.Up;
                    _maps[i, j].upPortal.connectedPortal = _maps[i + 1, j].downPortal;
                    _maps[i + 1, j].downPortal.connectedPortal = _maps[i, j].upPortal;
                }
                if(i > 0 && _mapGenerated[i - 1, j]) {
                    openDirection |= (int)Direction.Down;
                    _maps[i, j].downPortal.connectedPortal = _maps[i - 1, j].upPortal;
                    _maps[i - 1, j].upPortal.connectedPortal = _maps[i, j].downPortal;
                }
                if(j < _mapMaxSize.x - 1 && _mapGenerated[i, j + 1]) {
                    openDirection |= (int)Direction.Right;
                    _maps[i, j].rightPortal.connectedPortal = _maps[i, j + 1].leftPortal;
                    _maps[i, j + 1].leftPortal.connectedPortal = _maps[i, j].rightPortal;
                }
                if(j > 0 && _mapGenerated[i, j - 1]) {
                    openDirection |= (int)Direction.Left;
                    _maps[i, j].leftPortal.connectedPortal = _maps[i, j - 1].rightPortal;
                    _maps[i, j - 1].rightPortal.connectedPortal = _maps[i, j].leftPortal;
                }

                _maps[i, j].OpenPortal(openDirection);
            }
        }
    }

    private void SetUI() {
        for(int i = 0; i < _mapMaxSize.y; ++i) {
            for(int j = 0; j < _mapMaxSize.x; ++j) {
                if(_maps[i, j] == null) continue;

                Image img = Instantiate(_mapUI, _minimapTrm);

                Vector2 position = new Vector2((j - 2) * 220, (i - 2) * 220);
                img.rectTransform.localPosition = position;
            }
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;

        for(int i = -5; i <= 5; ++i) {
            for(int j = -5; j <= 5; ++j) {
                Gizmos.DrawWireCube(transform.position + new Vector3(_mapOffset.x * j, _mapOffset.y * i), new Vector2(_mapOffset.x - 0.5f, _mapOffset.y - 0.5f));
            }
        }
    }
}
