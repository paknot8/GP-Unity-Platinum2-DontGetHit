using System.Collections;
using UnityEngine;

public class HexagonStats : MonoBehaviour
{
    private float moveSpeed; // Speed at which the object moves
    private float destroyPosition; // Position at which the object should be destroyed
    public GameObject projectilePrefab; // Reference to the projectile prefab
    private float shootInterval; // Interval between each shot

    private bool canShoot = true; // Flag to track if shooting is allowed
    private Transform player; // Reference to the player's transform

    void Awake()
    {
        // Initialize variables
        moveSpeed = 7f;
        destroyPosition = -20f;
        shootInterval = 2f;

        // Find and store the player object's transform
        player = FindObjectOfType<Game_Manager>().transform;
    }

    void Start()
    {
        StartCoroutine(ShootTimer());
    }

    void Update()
    {
        MoveAndDestroy();
    }

    void MoveAndDestroy()
    {
        // Move the object to the left
        transform.Translate(moveSpeed * Time.deltaTime * Vector3.left);

        // Check if the object has reached the destroy position
        if (transform.position.x <= destroyPosition)
        {
            // Destroy the object
            Destroy(gameObject);
        }
    }

    IEnumerator ShootTimer()
    {
        while (true)
        {
            yield return new WaitForSeconds(shootInterval);

            if (canShoot && projectilePrefab != null && player != null)
            {
                // Calculate the direction towards the player
                Vector3 direction = (player.position - transform.position).normalized;

                // Instantiate the projectile and set its initial direction towards the player
                GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                projectile.GetComponent<Rigidbody2D>().velocity = direction * moveSpeed;
            }
        }
    }
}
