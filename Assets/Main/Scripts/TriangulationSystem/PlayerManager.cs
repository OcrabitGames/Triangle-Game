using System;
using NUnit.Framework.Constraints;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject playerTracePrefab;
    public String parentName;
    
    private Camera _camera;
    [SerializeField] private GameObject enemy;
    
    public int activePlayerNum;
    
    private Transform _parentTransform;
    private bool _waitToSpawn;
    [SerializeField] private GameObject[] players = new GameObject[3];
    private PlayerFollow[] _playerFollowScripts = new PlayerFollow[3];
    private GameObject _playerTrace;
    private Vector3 _traceOffset;
    
    private int _pendingPlayerNum;
    public float groundLevel;
    private TriangulationManager _triangulationManager;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _camera = Camera.main;
        _triangulationManager = TriangulationManager.Instance;
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        
        GameObject emptyObj = new GameObject(parentName);
        _parentTransform = Instantiate(emptyObj, transform.position, Quaternion.identity).transform;
    }

    // Update is called once per frame
    void Update()
    {
        CheckTracePosition();

        HandleMovement();
    }
    
    public void SpawnOrSelectPlayer(int num)
    {
        if (players[num] == null)
        {
            _pendingPlayerNum = num;
            _waitToSpawn = true;
            SpawnTransparentTrace();
        }
        else
        {
            // Set Active
            activePlayerNum = num;
        }
    }

    private void SpawnTransparentTrace()
    {
        _playerTrace = Instantiate(playerTracePrefab, transform.position, Quaternion.identity);
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void CheckTracePosition()
    {
        if (!_waitToSpawn) return;
        
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 newPosition = hit.point;
            newPosition.y = transform.position.y;
            if (_playerTrace)
            {
                newPosition.y = groundLevel;
                _playerTrace.transform.position = newPosition;
            }
        }

        // If clicked to Spawn Player
        if (Input.GetMouseButtonDown(0))
        {
            // Get Trace Position
            Vector3 spawnPosition = _playerTrace ? _playerTrace.transform.position : transform.position;
            
            // Spawn and Set Hierarchy
            GameObject player = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
            player.name = "Player" + _pendingPlayerNum;
            player.transform.SetParent(_parentTransform);
            
            // Update Scripts
            TriangulationManager.Instance.SetPlayer(player, _pendingPlayerNum);
            players[_pendingPlayerNum] = player;
            _playerFollowScripts[_pendingPlayerNum] = player.GetComponent<PlayerFollow>();
            
            // Set Movement
            activePlayerNum = _pendingPlayerNum;
            _playerFollowScripts[_pendingPlayerNum].Initialize(enemy);


            // Reset Trace
            _waitToSpawn = false;
            if (_playerTrace)
            {
                Destroy(_playerTrace);
                _playerTrace = null;
            }
        }
    }

    private void HandleMovement()
    {
        if (activePlayerNum == -1) return;
        
        GameObject activePlayer = players[activePlayerNum];
        if (!activePlayer) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _playerFollowScripts[activePlayerNum].StartFollowing();
            activePlayerNum = -1;
            return;
        }
        _playerFollowScripts[activePlayerNum].StopFollowing();

        float moveSpeed = 5f;
        Vector3 direction = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) direction += Vector3.forward;
        if (Input.GetKey(KeyCode.S)) direction += Vector3.back;
        if (Input.GetKey(KeyCode.A)) direction += Vector3.left;
        if (Input.GetKey(KeyCode.D)) direction += Vector3.right;
        activePlayer.transform.Translate(direction * (moveSpeed * Time.deltaTime), Space.World);
    }
}
