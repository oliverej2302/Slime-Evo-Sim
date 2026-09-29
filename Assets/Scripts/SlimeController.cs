using UnityEngine;

public class SlimeController : MonoBehaviour
{
    public float moveSpeed; //distance moved per tick
    public float visionRange;
    public float maxHunger;
    public float currentHunger;
    public float maxThirst;
    public float currentThirst;
    public float livingStatDrain;

    void Start()
    {
        TimeTickSystem.OnTick += delegate (object sender, TimeTickSystem.OnTickEventArgs e)
        {
            ApplyLivingStatDrain();
            MoveSlime();
        };
    }

    private void ApplyLivingStatDrain()
    {
        currentHunger -= livingStatDrain;
        currentThirst -= livingStatDrain;
    }

    private void MoveSlime()
    {
        Vector2 directionToMove = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        transform.position += transform.right * moveSpeed * directionToMove.x + transform.up * moveSpeed * directionToMove.y;
    }
}
