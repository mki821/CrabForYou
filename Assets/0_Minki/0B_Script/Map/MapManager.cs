using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoSingleton<MapManager>
{
    [SerializeField] private List<Map> _mapPrefabs;

    [SerializeField] private Vector2 _mapOffset;
    [SerializeField] private Vector2Int _mapMaxSize;

    [SerializeField] private Transform _minimapTrm;
    [SerializeField] private Image _mapUIPrefab;

    [SerializeField] private InputReader _inputReader;

    private int _roomCount = 0;
    private Vector2Int _playerPosition;

    private Map[,] _maps;
    private bool[,] _mapGenerated;
    private Image[,] _mapUI;

    protected override void Awake() {
        base.Awake();

        _mapGenerated = new bool[_mapMaxSize.y, _mapMaxSize.x];
        _maps = new Map[_mapMaxSize.y, _mapMaxSize.x];
        _mapUI = new Image[_mapMaxSize.y, _mapMaxSize.x];

        _inputReader.MinimapEvent += OpenMinimap;
        _inputReader.MinimapCancelEvent += CloseMinimap;
    }

    private void Start() {
        _roomCount = 0;
        Generate(new Vector2Int(_mapMaxSize.x / 2, _mapMaxSize.y / 2));

        if(_roomCount == 0) {
            _roomCount = 0;
            Generate(new Vector2Int(_mapMaxSize.x / 2, _mapMaxSize.y / 2));
        }

        CorrectAllMap();
        SetUI();

        _playerPosition = new Vector2Int(_mapMaxSize.x / 2, _mapMaxSize.y / 2);
        SetPlayerPosition(new Vector2Int(_mapMaxSize.x / 2, _mapMaxSize.y / 2));
    }

    private void OnDisable() {
        _inputReader.MinimapEvent -= OpenMinimap;
        _inputReader.MinimapCancelEvent -= CloseMinimap;
    }

    public void Generate(Vector2Int position) {
        if(position.x >= _mapMaxSize.x || position.x < 0 || position.y >= _mapMaxSize.y || position.y < 0) return;
        if(_mapGenerated[position.y, position.x]) return;
        _mapGenerated[position.y, position.x] = true;

        Map randomMap = _mapPrefabs[Random.Range(0, _mapPrefabs.Count)];
        Map generatedMap = Instantiate(randomMap, new Vector2((position.x - 2) * _mapOffset.x, (position.y - 2) * _mapOffset.y), Quaternion.identity);
        generatedMap.mapPosition = position;
        ++_roomCount;

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

                _maps[i, j].ActovePortal(openDirection);
            }
        }
    }

    private void SetUI() {
        for(int i = 0; i < _mapMaxSize.y; ++i) {
            for(int j = 0; j < _mapMaxSize.x; ++j) {
                if(_maps[i, j] == null) continue;

                Image img = Instantiate(_mapUIPrefab, _minimapTrm);
                _mapUI[i, j] = img;
                
                Instantiate(_maps[i, j].icon, img.transform);

                Vector2 position = new Vector2((j - 2) * 220, (i - 2) * 220);
                img.rectTransform.localPosition = position;
            }
        }
    }

    public void SetPlayerPosition(Vector2Int position) {
        _mapUI[_playerPosition.y, _playerPosition.x].color = Color.white;
        _playerPosition = position;
        _mapUI[_playerPosition.y, _playerPosition.x].color = Color.cyan;
    }

    private void OpenMinimap() => _minimapTrm.gameObject.SetActive(true);
    private void CloseMinimap() => _minimapTrm.gameObject.SetActive(false);

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;

        for(int i = 0; i < _mapMaxSize.y; ++i) {
            for(int j = 0; j < _mapMaxSize.x; ++j) {
                float x = j - _mapMaxSize.x / 2;
                float y = i - _mapMaxSize.y / 2;
                Gizmos.DrawWireCube(transform.position + new Vector3(_mapOffset.x * x, _mapOffset.y * y), new Vector2(_mapOffset.x - 0.5f, _mapOffset.y - 0.5f));
            }
        }
    }
}
