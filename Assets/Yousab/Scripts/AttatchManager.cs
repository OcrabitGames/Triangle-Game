using UnityEngine;

public class AttachManager : MonoBehaviour
{
    public GameObject enemy;
    private bool isFollowing = false;
    private Vector3 offset;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartFollowingEnemy();
        }

        if (isFollowing)
        {
            FollowEnemy();
        }
    }

    void StartFollowingEnemy()
    {
        isFollowing = true;
        offset = transform.position - enemy.transform.position;
    }

    public void StopFollowingEnemy()
    {
        isFollowing = false;
    }

    void FollowEnemy()
    {
        if (enemy != null) {
        transform.position = enemy.transform.position + offset;
        }
    }
}
