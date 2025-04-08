using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject[] playerPrefabs;
    public GameObject[] playerTracePrefabs;
    public String parentName;
    
    private Camera _camera;
    [SerializeField] private GameObject enemy;
    
    public int activePlayerNum;
    
    private Transform _parentTransform;
    private bool _waitToSpawn;
    private bool _tracePlaced = false;
    
    [SerializeField] private GameObject[] players = new GameObject[3];
    private FoxFollow[] _playerFollowScripts = new FoxFollow[3];
    private GameObject _playerTrace;
    private Vector3 _traceOffset;
    
    private int _pendingPlayerNum;
    public float groundLevel;
    private TriangulationManager _triangulationManager;

    private GameObject soundManager;
    public AudioClip placeSound;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _camera = Camera.main;
        _triangulationManager = TriangulationManager.Instance;
        enemy = GameObject.FindGameObjectWithTag("Enemy");
        
        GameObject emptyObj = new GameObject(parentName);
        _parentTransform = Instantiate(emptyObj, transform.position, Quaternion.identity).transform;

        soundManager = GameObject.FindGameObjectWithTag("Sound FX Manager");
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
            _tracePlaced = false; // skip first click
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
        _playerTrace = Instantiate(playerTracePrefabs[_pendingPlayerNum], transform.position, Quaternion.identity);
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void CheckTracePosition()
    {
        if (!_waitToSpawn) return;
        
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 newPosition = hit.point;
            newPosition.y = groundLevel;
            if (_playerTrace)
            {
                _playerTrace.transform.position = newPosition;
            }
        }
        
        // Skip first click
        if (!_tracePlaced)
        {
            // Wait until the UI click is released before accepting placement
            if (Input.GetMouseButtonUp(0))
            {
                _tracePlaced = true;
            }
            return;
        }
        
        // If clicked to Spawn Player
        if (Input.GetMouseButtonDown(0))
        {
            SpawnPlayerAtTrace();
        }
    }

    private void SpawnPlayerAtTrace()
    {
        // Play sound
        soundManager.GetComponent<SoundFXManager>().PlaySound(placeSound);

        // Get Trace Position
        Vector3 spawnPosition = _playerTrace ? _playerTrace.transform.position : transform.position;
            
        // Spawn and Set Hierarchy
        GameObject player = Instantiate(playerPrefabs[_pendingPlayerNum], spawnPosition, Quaternion.identity);
        player.name = "Player" + _pendingPlayerNum;
        player.transform.SetParent(_parentTransform);
            
        // Update Scripts
        TriangulationManager.Instance.SetPlayer(player, _pendingPlayerNum);
        players[_pendingPlayerNum] = player;
        _playerFollowScripts[_pendingPlayerNum] = player.GetComponent<FoxFollow>();
            
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

    // ReSharper disable Unity.PerformanceAnalysis
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
        
        Vector3 direction = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) direction += Vector3.forward;
        if (Input.GetKey(KeyCode.S)) direction += Vector3.back;
        if (Input.GetKey(KeyCode.A)) direction += Vector3.left;
        if (Input.GetKey(KeyCode.D)) direction += Vector3.right;
        
        FoxMovement fm = activePlayer.GetComponent<FoxMovement>();
        if (fm)
        {
            fm.MoveDirection(direction);
        }
    }
}
