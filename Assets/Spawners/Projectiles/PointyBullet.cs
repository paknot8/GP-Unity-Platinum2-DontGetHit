using System.Collections;
using UnityEngine;

public class PointyBullet : MonoBehaviour
{
    private float moveSpeed; // Speed at which the bullet moves

    void Awake()
    {
        moveSpeed = 2f;
    }

    void Start()
    {
        // Start a coroutine to destroy the bullet
        StartCoroutine(DestroyAfterDelay(2f));
    }

    void Update()
    {
        // Move the bullet forward along the Y-axis
        transform.Translate(moveSpeed * Time.deltaTime * Vector3.up);
    }

    IEnumerator DestroyAfterDelay(float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Destroy the bullet object
        Destroy(gameObject);
    }

    // Called when the object becomes invisible to any camera
    void OnBecameInvisible()
    {
        // Destroy the bullet when it goes outside the camera view
        Destroy(gameObject);
    }
}
