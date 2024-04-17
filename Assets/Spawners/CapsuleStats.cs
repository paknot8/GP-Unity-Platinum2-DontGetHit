using UnityEngine;

public class CapsuleStats : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed at which the object moves
    public float destroyPosition = 100f; // Position at which the object should be destroyed

    // Update is called once per frame
    void Update()
    {
        MoveAndDestroy();
    }

    void MoveAndDestroy()
    {
        transform.Translate(moveSpeed * Time.deltaTime * Vector3.left);

        // Check if the object has reached the destroy position
        if (transform.position.x <= destroyPosition)
        {
            // Destroy the object
            Destroy(gameObject);
        }
    }
}
