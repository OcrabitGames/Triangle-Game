using System;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject playerTracePrefab;
    public String parentName;
    private Transform _parentTransform;
    private bool _waitToSpawn;
    [SerializeField] private GameObject[] players = new GameObject[3];
    private GameObject _playerTrace;
    private Vector3 _traceOffset;
    
    private Camera _camera;
    private int _pendingPlayerNum;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _camera = Camera.main;
        
        GameObject emptyObj = new GameObject(parentName);
        _parentTransform = Instantiate(emptyObj, transform.position, Quaternion.identity).transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (_waitToSpawn)
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Vector3 newPosition = hit.point;
                newPosition.y = transform.position.y;
                if (_playerTrace != null)
                {
                    _playerTrace.transform.position = newPosition;
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                Vector3 spawnPosition = _playerTrace != null ? _playerTrace.transform.position : transform.position;
                GameObject player = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
                player.name = "Player" + _pendingPlayerNum;
                player.transform.SetParent(_parentTransform);
                TriangulationManager.Instance.SetPlayer(player, _pendingPlayerNum);

                _waitToSpawn = false;
                if (_playerTrace != null)
                {
                    Destroy(_playerTrace);
                }
            }
        }
    }
    
    public void SpawnPlayer(int num)
    {
        _pendingPlayerNum = num;
        _waitToSpawn = true;
        SpawnTransparentTrace();
    }

    private void SpawnTransparentTrace()
    {
        _playerTrace = Instantiate(playerTracePrefab, transform.position, Quaternion.identity);
    }
}
