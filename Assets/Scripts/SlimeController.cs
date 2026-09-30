using UnityEngine;

public class SlimeController : MonoBehaviour
{
    public SlimeState state = SlimeState.Roaming;
    public LayerMask waterLayerMask;
    public LayerMask foodLayerMask;
    public float moveSpeed; //distance moved per tick
    public float visionRange;
    public float maxHunger;
    public float currentHunger;
    public float maxThirst;
    public float currentThirst;
    public float livingStatDrain;

    void Start()
    {
        waterLayerMask = LayerMask.GetMask("Water");
        foodLayerMask = LayerMask.GetMask("Food");

        TimeTickSystem.OnTick += delegate (object sender, TimeTickSystem.OnTickEventArgs e)
        {
            ApplyLivingStatDrain();
            DetermineNeeds();
        };
    }

    private void DetermineNeeds()
    {
        state = SlimeState.Roaming;

        if (currentThirst <= 30f)
        {
            FindAndMoveToClosestResource(ResourceType.Water);
        }
        else if (currentHunger <= 30f)
        {
            FindAndMoveToClosestResource(ResourceType.Food);
        }

        if (state == SlimeState.Roaming)
        {
            MoveSlimeRandom();
        }
    }

    private void ApplyLivingStatDrain()
    {
        currentHunger -= livingStatDrain;
        currentThirst -= livingStatDrain;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, visionRange);
    }

    private void FindAndMoveToClosestResource(ResourceType resource)
    {
        Collider2D[] resourceColliders = System.Array.Empty<Collider2D>();
        switch (resource)
        {
            case ResourceType.Water:
                state = SlimeState.Drinking;
                resourceColliders = Physics2D.OverlapCircleAll(transform.position, visionRange, waterLayerMask);
                break;
            case ResourceType.Food:
                state = SlimeState.Eating;
                resourceColliders = Physics2D.OverlapCircleAll(transform.position, visionRange, foodLayerMask);
                break;
            default:
                break;
        }

        if (resourceColliders.Length <= 0)
        {
            state = SlimeState.Roaming;
            return;
        }

        Vector2 closestResourcePosition = Vector2.zero;
        float shortestDistance = Mathf.Infinity;
        Vector2 currentPosition = transform.position;

        for (int i = 0; i < resourceColliders.Length; i++)
        {
            Vector2 toTarget = (Vector2)resourceColliders[i].transform.position - currentPosition;
            if (toTarget.magnitude < shortestDistance)
            {
                closestResourcePosition = (Vector2)resourceColliders[i].transform.position;
                shortestDistance = toTarget.magnitude;
            }
        }

        bool hasArrived = MoveSlimeTo(closestResourcePosition);
        if (hasArrived)
        {
            switch (resource)
            {
                case ResourceType.Water:
                    Drink();
                    break;
                case ResourceType.Food:
                    Eat();
                    break;
                default:
                    break;
            }
        }
    }

    private void Drink()
    {
        currentThirst += 20f;
        if (currentThirst > maxThirst)
        {
            currentThirst = maxThirst;
        }
    }

    private void Eat()
    {
        currentHunger += 20f;
        if (currentHunger > maxHunger)
        {
            currentHunger = maxHunger;
        }
    }

    private bool MoveSlimeTo(Vector2 targetPosition)
    {
        Vector2 currentPosition = transform.position;
        Vector2 toTarget = targetPosition - currentPosition;
        float distance = toTarget.magnitude;

        // Close enough to arrive this step: snap to the target so we don't overshoot
        if (distance <= moveSpeed)
        {
            transform.position = new Vector3(targetPosition.x, targetPosition.y, transform.position.z);
            return true;
        }

        // Too far for one step: move a full step in the target's direction
        Vector2 step = toTarget.normalized * moveSpeed;
        transform.position += (Vector3)step;
        return false;
    }

    private void MoveSlimeRandom()
    {
        Vector2 directionToMove = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        transform.position += transform.right * moveSpeed * directionToMove.x + transform.up * moveSpeed * directionToMove.y;
    }
}

public enum ResourceType
{
    Water,
    Food
}

public enum SlimeState
{
    Drinking,
    Eating,
    Roaming
}