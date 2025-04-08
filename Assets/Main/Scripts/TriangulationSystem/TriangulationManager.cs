using System;
using UnityEngine;

public class TriangulationManager : MonoBehaviour
{
    // an Instance that doesn't get destroyed so this works on all scenes
    public static TriangulationManager Instance { get; private set; }
    
    // Script Ref
    [NonSerialized] public SoundFXManager soundFXManager;
    [NonSerialized] public LevelManager levelManager;
    
    // Script Activity Management
    public bool gameActive;
    public bool isDrawing;
    
    // The three players and their stashed positions
    private GameObject player1;
    private GameObject player2;
    private GameObject player3;
    private Vector3 _cachedPlayer1Pos;
    private Vector3 _cachedPlayer2Pos;
    private Vector3 _cachedPlayer3Pos;
    
    // The lines drawn between connections
    public GameObject linePrefab;
    private LineCube _line12;
    private LineCube _line23;
    private LineCube _line13;
    
    // The bools for connections
    private bool _connect12;
    private bool _connect23;
    private bool _connect13;
    private bool _allConnected;
    
    // For Creating the glowing Triangle when they triangulate
    public GameObject triangularPlanePrefab;
    private TriangularPlane _triangularPlane;
    
    // Requirements for triangulation
    public float distanceRequired;
    public float timeRequired = 3f;
    
    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
            
            // Reset
            Start();
        }
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Make sure vals are false at start
        // Reset Vals
        player1 = null;
        player2 = null;
        player3 = null;
        _connect12 = false;
        _connect23 = false;
        _connect13 = false;
        _allConnected = false;
        _line12 = null;
        _line23 = null;
        _line13 = null;
        _triangularPlane = null;
        
        // Set TriangularPlane Up
        _triangularPlane = Instantiate(triangularPlanePrefab).GetComponent<TriangularPlane>();
        _triangularPlane.Initialize(timeRequired, this);
        
        // Set Up LineCubes
        _line12 = CreateLineCube(Vector3.zero, Vector3.zero);
        _line23 = CreateLineCube(Vector3.zero, Vector3.zero);
        _line13 = CreateLineCube(Vector3.zero, Vector3.zero);
        
        // Set Draw Mode Active
        SetDrawing(true);
        
        // Script Ref
        soundFXManager = SoundFXManager.Instance;
        levelManager = LevelManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameActive) return;

        if (isDrawing)
        {
            CalculateDistance();
            if (player1 && player1.transform.position != _cachedPlayer1Pos)
            {
                //print($"Logging Player 1 {player1.transform.position}");
                _cachedPlayer1Pos = player1.transform.position;
                DrawConnection(_connect12, _connect13, player1, player2, player3, ref _line12, ref _line13);
            }
            if (player2 && player2.transform.position != _cachedPlayer2Pos)
            {
                //print($"Logging Player 2 {player2.transform.position}");
                _cachedPlayer2Pos = player2.transform.position;
                DrawConnection(_connect12, _connect23, player2, player1, player3, ref _line12, ref _line23);
            }
            if (player3 && player3.transform.position != _cachedPlayer3Pos)
            {                
                //print($"Logging Player 3 {player3.transform.position}");
                _cachedPlayer3Pos = player3.transform.position;
                DrawConnection(_connect13, _connect23, player3, player1, player2, ref _line13, ref _line23);
            }
            
            if (_allConnected)
            {
                // Create triangular plane
                _triangularPlane.UpdateTriangle(player1.transform.position, player2.transform.position, player3.transform.position);
            } else {
                _triangularPlane.Deactivate();
            }
        }
    }

    private void CalculateDistance()
    {
        if (player1)
        {
            if (player2) _connect12 = Vector3.Distance(player1.transform.position, player2.transform.position) <= distanceRequired;
            if (player3) _connect13 = Vector3.Distance(player1.transform.position, player3.transform.position) <= distanceRequired;
        }

        if (player2)
        {
            if (player3) _connect23 = Vector3.Distance(player2.transform.position, player3.transform.position) <= distanceRequired;
        }
        _allConnected = _connect12 && _connect23 && _connect13;
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void DrawConnection(bool connection1, bool connection2, GameObject firstObject, GameObject secondObject, GameObject thirdObject, ref LineCube line1, ref LineCube line2)
    {
        if (firstObject && secondObject)
        {
            Vector3 firstPosition = firstObject.transform.position;
            Vector3 secondPosition = secondObject.transform.position;
            if (connection1)
            {
                if (line1.isActive)
                {
                    line1.UpdateEndpoints(firstPosition, secondPosition);
                } else {
                    line1.Initialize(firstPosition, secondPosition);
                }
            } else {
                line1.Deactivate();
            }
        }

        if (firstObject && thirdObject)
        {
            Vector3 firstPosition = firstObject.transform.position;
            Vector3 thirdPosition = thirdObject.transform.position;
            if (connection2)
            {
                if (line2.isActive)
                {
                    line2.UpdateEndpoints(firstPosition, thirdPosition);
                }
                else
                {
                    line2.Initialize(firstPosition, thirdPosition);
                }
            }
            else
            {
                line2.Deactivate();
            }
        }
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    private void DrawAllConnections()
    {
        if (_connect12)
        {
            if (_line12 == null)
                _line12 = CreateLineCube(player1.transform.position, player2.transform.position);
            else
                UpdateLinePosition(_line12, player1.transform.position, player2.transform.position);
        }
        else if (_line12 != null)
        {
            _line12.Deactivate();
        }

        if (_connect23)
        {
            if (_line23 == null)
                _line23 = CreateLineCube(player2.transform.position, player3.transform.position);
            else
                UpdateLinePosition(_line23, player2.transform.position, player3.transform.position);
        }
        else if (_line23 != null)
        {
            _line23.Deactivate();
        }

        if (_connect13)
        {
            if (_line13 == null)
                _line13 = CreateLineCube(player1.transform.position, player3.transform.position);
            else
                UpdateLinePosition(_line13, player1.transform.position, player3.transform.position);
        }
        else if (_line13 != null)
        {
            _line13.Deactivate();
        }
    }

    private LineCube CreateLineCube(Vector3 start, Vector3 end)
    {
        var lineObj = Instantiate(linePrefab);
        var lineScript = lineObj.GetComponent<LineCube>();
        lineScript.Initialize(start, end, true);
        //lineObj.SetActive(false);
        return lineScript;
    }

    private void UpdateLinePosition(LineCube line, Vector3 start, Vector3 end)
    {
        line.UpdateEndpoints(start, end);
    }

    public void SetDrawing(bool drawing)
    {
        isDrawing = drawing;
    }

    public void SetPlayer(GameObject player, int num)
    {
        if (num == 0)
        {
            player1 = player;
        } else if (num == 1)
        {
            player2 = player;
        } else if (num == 2)
        {
            player3 = player;
        }
    }
}
