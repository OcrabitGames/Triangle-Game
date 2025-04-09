using UnityEngine;

public class FoxFollow : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    [SerializeField] private bool isFollowing = false;
    
    private Vector3 offset;
    private FoxMovement _foxMovement;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _foxMovement = GetComponent<FoxMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isFollowing && enemy)
        {
            _foxMovement.MoveToward(enemy.transform.position + offset);
        }
        if (!enemy) {
            _foxMovement.StopMovement();
        }
    }

    public void Initialize(GameObject enemyObj)
    {
        enemy = enemyObj;
    }
    
    public void StartFollowing()
    {
        isFollowing = true;
        offset = transform.position - enemy.transform.position;
    }
    
    public void StopFollowing()
    {
        isFollowing = false;
    }
}
