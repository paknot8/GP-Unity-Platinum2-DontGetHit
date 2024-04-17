using UnityEngine;

public class Spawner1 : MonoBehaviour
{
    public GameObject objectToSpawn; // Prefab to spawn
    private float spawnInterval = 1f; // Time interval between spawns
    private float spawnDistanceFromLeft = 1f; // Distance from the left side of the screen to spawn
    private float verticalSpawnPosition = -3f; // Vertical position to spawn objects

    private Camera mainCamera;
    private float spawnTimer = 0f;

    void Start()
    {
        mainCamera = Camera.main;
    }

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

            // Calculate spawn position on the left side of the screen
            float screenHeight = 2f * mainCamera.orthographicSize;
            Vector3 spawnPosition = new Vector3(mainCamera.transform.position.x - mainCamera.aspect * mainCamera.orthographicSize - spawnDistanceFromLeft, verticalSpawnPosition, 0f);

            // Spawn a new object at the calculated spawn position
            if (objectToSpawn != null)
            {
                Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
            }
        }
    }
}
