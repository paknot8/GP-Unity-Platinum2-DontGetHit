using UnityEngine;

public class Spawner1 : MonoBehaviour
{
    public GameObject spawnPoint;
    public GameObject objectToSpawn; // Prefab to spawn

    private float spawnTimer = 0f;
    private readonly float spawnInterval = 1f; // Time interval between spawns

    // Update is called once per frame
    void Update()
    {
        SpawnObjects();
    }

    void SpawnObjects()
    {
        // Increment the timer
        spawnTimer += Time.deltaTime;

        // Check if it's time to spawn a new object
        if (spawnTimer >= spawnInterval)
        {
            // Reset the timer
            spawnTimer = 0f;

            // Spawn a new object at the spawn point
            if(objectToSpawn != null){
                Instantiate(objectToSpawn, spawnPoint.transform.position, Quaternion.identity);
            }
        }
    }
}
