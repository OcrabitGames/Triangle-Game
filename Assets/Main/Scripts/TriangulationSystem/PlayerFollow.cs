using UnityEngine;

public class PlayerFollow : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    [SerializeField] private bool isFollowing = false;
    private Vector3 offset;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isFollowing)
        {
            if (enemy) {
                transform.position = enemy.transform.position + offset;
            }
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
