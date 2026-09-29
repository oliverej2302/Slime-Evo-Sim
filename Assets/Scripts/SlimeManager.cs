using Unity.Mathematics;
using UnityEngine;

public class SlimeManager : MonoBehaviour
{
    [SerializeField] GameObject slimePrefab;

    void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            Vector3 spawnPosition = new Vector3(i, 0, 0);
            SpawnNewSlime(spawnPosition);
        }

        TimeTickSystem.OnTick += delegate (object sender, TimeTickSystem.OnTickEventArgs e)
        {
            Debug.Log("Tick = " + e.tick);
        };

    }

    private void SpawnNewSlime(Vector3 spawnPosition)
    {
        GameObject newSlimeInstance = Instantiate(slimePrefab, spawnPosition, quaternion.identity, transform);
        SlimeColourChanger newSlimeColourChanger = newSlimeInstance.GetComponent<SlimeColourChanger>();
        newSlimeColourChanger.SetNewColour(Color.green);
        SlimeController newSlimeController = newSlimeInstance.GetComponent<SlimeController>();
        newSlimeController.moveSpeed = 0.4f;
        newSlimeController.visionRange = 3f;
        newSlimeController.maxHunger = 100;
        newSlimeController.currentHunger = 50;
        newSlimeController.maxThirst = 100;
        newSlimeController.currentThirst = 50;
        newSlimeController.livingStatDrain = 0.2f;
    }
}
