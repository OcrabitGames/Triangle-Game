using UnityEngine;

public class NPCMovementGroups : MonoBehaviour
{
    public GameObject movementSpotsParent;

    private Transform[] targetSpots;
    private int[] currentTargetIndices;
    private Transform[] children;
    private NPCMovement[] npcMovements;

    void Start()
    {
        int targetCount = movementSpotsParent.transform.childCount;
        targetSpots = new Transform[targetCount];
        for (int i = 0; i < targetCount; i++)
        {
            targetSpots[i] = movementSpotsParent.transform.GetChild(i);
        }

        int childCount = transform.childCount;
        children = new Transform[childCount];
        currentTargetIndices = new int[childCount];
        npcMovements = new NPCMovement[childCount];

        for (int i = 0; i < childCount; i++)
        {
            children[i] = transform.GetChild(i);
            currentTargetIndices[i] = 0;
            npcMovements[i] = children[i].GetComponent<NPCMovement>();
        }
    }

    void Update()
    {
        for (int i = 0; i < children.Length; i++)
        {
            Transform npc = children[i];
            Transform target = targetSpots[currentTargetIndices[i]];

            Vector3 delta = target.position - npc.position;
            delta.y = 0f;

            if (delta.magnitude < 0.2f)
            {
                currentTargetIndices[i] = (currentTargetIndices[i] + 1) % targetSpots.Length;
            }
            else
            {
                Vector3 targetDir = delta.normalized;
                Vector3 flockingDir = GetFlockingDirection(i);

                // Blend flocking and target direction
                Vector3 finalDirection = (targetDir + flockingDir).normalized;

                npcMovements[i].MoveDirection(finalDirection);
            }
        }
    }

    Vector3 GetFlockingDirection(int index)
    {
        Transform owl = children[index];
        Vector3 alignment = Vector3.zero;
        Vector3 cohesion = Vector3.zero;
        Vector3 separation = Vector3.zero;

        int neighborCount = 0;
        float neighborRadius = 5f;
        float separationDistance = 2f;

        for (int i = 0; i < children.Length; i++)
        {
            if (i == index) continue;

            Transform other = children[i];
            float distance = Vector3.Distance(owl.position, other.position);
            if (distance < neighborRadius)
            {
                neighborCount++;

                Rigidbody rb = other.GetComponent<Rigidbody>();
                if (rb != null)
                    alignment += rb.linearVelocity;

                cohesion += other.position;

                if (distance < separationDistance)
                    separation += (owl.position - other.position) / distance;
            }
        }

        Vector3 direction = Vector3.zero;
        if (neighborCount > 0)
        {
            alignment /= neighborCount;
            cohesion = (cohesion / neighborCount - owl.position).normalized;
            separation /= neighborCount;

            direction = alignment.normalized + cohesion + separation;
            direction.Normalize();
        }

        return direction;
    }

}
