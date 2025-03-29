using UnityEngine;

public class PersonTestMovement : MonoBehaviour
{
    private Rigidbody _rigidbody;
    public float moveTime = 2f;
    private float _moveTime;
    public float moveSpeed = 5f;
    [SerializeField] private Vector3 moveDirection;
    private bool _isMoving;
    public float borderPoint = 5f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _moveTime = moveTime;
    }

    // Fixed Update for Physics
    void FixedUpdate()
    {
        if (_isMoving)
        {
            _moveTime -= Time.fixedDeltaTime;
            _rigidbody.MovePosition(_rigidbody.position + moveDirection * (moveSpeed * Time.fixedDeltaTime));
            CheckMovementTime();
        } else {
            GenerateDirection();
            _isMoving = true;
        }
    }

    private void CheckMovementTime()
    {
        // Reset Movement Period if current moveTime is 0
        if (_moveTime <= 0)
        {
            _isMoving = false;
            _moveTime = moveTime;
        }
    }

    private void GenerateDirection()
    {
        Vector2 randDir = Random.insideUnitCircle.normalized;
        moveDirection = new Vector3(randDir.x, 0f, randDir.y);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.contacts.Length > 0)
        {
            Vector3 normal = collision.contacts[0].normal;
            moveDirection = Vector3.Reflect(moveDirection, normal).normalized;
        }
    }
}
