using UnityEngine;
using System.Collections.Generic;

public class OLDNPCMovementGroups : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float neighborRadius = 3f;
    public float separationDistance = 1f;
    public float cohesionWeight = 1.0f;
    public float alignmentWeight = 1.0f;
    public float separationWeight = 1.5f;
    public float wanderChance = 0.1f;
    public float changeTargetIntervalMin = 3f;
    public float changeTargetIntervalMax = 7f;
    public float randomTargetRadius = 10f;
    public float smoothFactor = 0.2f;
    public float inertia = 0.9f;

    private List<Rigidbody> groupMembers = new List<Rigidbody>();
    private Dictionary<Rigidbody, Vector3> velocityCache = new Dictionary<Rigidbody, Vector3>();
    private Dictionary<Rigidbody, NPCMovement> owlMovements = new Dictionary<Rigidbody, NPCMovement>();
    private Vector3 groupTargetPosition;
    private float targetTimer;
    private Color groupColor;

    public bool useRandom;
    public Transform[] targetPaths;
    private int currentTargetIndex;
    private Transform target;

    void Start()
    {
        foreach (Transform child in transform)
        {
            Rigidbody rb = child.GetComponent<Rigidbody>();
            NPCMovement om = child.GetComponent<NPCMovement>();
            
            if (rb && om)
            {
                groupMembers.Add(rb);
                rb.interpolation = RigidbodyInterpolation.Interpolate;
                velocityCache[rb] = Vector3.zero;
                owlMovements[rb] = om;
            }
        }

        if (useRandom) SetRandomTarget();
        else GetNextTarget();
        
        targetTimer = GetRandomTargetInterval();
    }

    void FixedUpdate()
    {
        targetTimer -= Time.fixedDeltaTime;
        if (targetTimer <= 0)
        {
            if (useRandom) SetRandomTarget();
            else GetNextTarget();
            targetTimer = GetRandomTargetInterval();
        }

        foreach (Rigidbody rb in groupMembers)
        {
            Vector3 moveDirection = (Random.value < wanderChance)
                ? Random.insideUnitSphere.normalized
                : CalculateFlockingDirection(rb);

            moveDirection.y = 0f;

            Vector3 toTarget = (groupTargetPosition - rb.position).normalized;
            moveDirection = Vector3.Lerp(moveDirection, toTarget, smoothFactor);
            moveDirection = moveDirection.normalized;

            Vector3 previousVelocity = velocityCache[rb];
            Vector3 smoothedVelocity = Vector3.Lerp(previousVelocity, moveDirection * moveSpeed, 1f - inertia);
            velocityCache[rb] = smoothedVelocity;

            if (owlMovements.TryGetValue(rb, out NPCMovement owl))
            {
                Vector3 targetPoint = rb.position + smoothedVelocity;
                owl.MoveToward(targetPoint);
            }
        }
    }

    private void SetRandomTarget()
    {
        groupTargetPosition = transform.position + new Vector3(
            Random.Range(-randomTargetRadius, randomTargetRadius),
            0,
            Random.Range(-randomTargetRadius, randomTargetRadius)
        );

        // AssignGroupColor();
    }

    private void GetNextTarget()
    {
        if (currentTargetIndex < targetPaths.Length - 1) { currentTargetIndex++; }
        else currentTargetIndex = 0;
        target = targetPaths[currentTargetIndex];
        groupTargetPosition = new Vector3(target.position.x, 0f, target.position.z);
    }

    private float GetRandomTargetInterval()
    {
        return Random.Range(changeTargetIntervalMin, changeTargetIntervalMax);
    }

    private Vector3 CalculateFlockingDirection(Rigidbody rb)
    {
        Vector3 alignment = Vector3.zero;
        Vector3 cohesion = Vector3.zero;
        Vector3 separation = Vector3.zero;
        int neighborCount = 0;

        foreach (Rigidbody other in groupMembers)
        {
            if (rb == other) continue;

            Vector3 toOther = other.position - rb.position;
            float distance = toOther.magnitude;

            if (distance < neighborRadius)
            {
                alignment += other.linearVelocity;
                cohesion += other.position;

                if (distance < separationDistance)
                {
                    separation -= toOther.normalized / distance;
                }

                neighborCount++;
            }
        }

        if (neighborCount > 0)
        {
            alignment = (alignment / neighborCount).normalized * alignmentWeight;
            cohesion = ((cohesion / neighborCount) - rb.position).normalized * cohesionWeight;
            separation = separation.normalized * separationWeight;
        }

        return (alignment + cohesion + separation).normalized;
    }

    // private void AssignGroupColor()
    // {
    //     groupColor = new Color(Random.value, Random.value, Random.value);
    //     foreach (Rigidbody rb in groupMembers)
    //     {
    //         Renderer renderer = rb.GetComponent<Renderer>();
    //         if (renderer != null)
    //         {
    //             renderer.material.color = groupColor;
    //         }
    //     }
    // }
}
