using UnityEngine;

public class Spawner4 : MonoBehaviour
{
    public GameObject objectToSpawn; // Prefab to spawn
    private float spawnInterval = 2f; // Time interval between spawns
    private float spawnDistanceFromRight = 1f; // Distance from the right side of the screen to spawn
    private float verticalSpawnRange = 2f; // Range from the top and bottom of the screen to spawn

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

            // Calculate spawn position on the right side of the screen
            float screenHeight = 2f * mainCamera.orthographicSize;
            float screenWidth = screenHeight * mainCamera.aspect;
            Vector3 spawnPosition = new Vector3(mainCamera.transform.position.x + (screenWidth / 2) + spawnDistanceFromRight, Random.Range(-verticalSpawnRange, verticalSpawnRange), 0f);

            // Clamp spawn position within the vertical range
            spawnPosition.y = Mathf.Clamp(spawnPosition.y, -mainCamera.orthographicSize + verticalSpawnRange, mainCamera.orthographicSize - verticalSpawnRange);

            // Spawn a new object at the calculated spawn position
            if (objectToSpawn != null)
            {
                GameObject spawnedObject = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
                // Attach CapsuleStats script to the spawned object
                CapsuleStats capsuleStats = spawnedObject.GetComponent<CapsuleStats>();
                if (capsuleStats != null)
                {
                    // Set the destroy position to the left side of the screen
                    capsuleStats.destroyPosition = -screenWidth / 2 - spawnDistanceFromRight;
                }
            }
        }
    }
}
