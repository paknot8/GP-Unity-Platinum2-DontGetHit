using UnityEngine;

public class TriangleStats : MonoBehaviour
{
    public float moveSpeed = 3f; // Speed at which the object moves
    public float destroyPosition = 30f; // Position at which the object should be destroyed

    // Update is called once per frame
    void Update()
    {
        MoveAndDestroy();
    }

    void MoveAndDestroy()
    {
        transform.Translate(moveSpeed * Time.deltaTime * Vector3.right);

        // Check if the object has reached the destroy position
        if (transform.position.x >= destroyPosition)
        {
            // Destroy the object
            Destroy(gameObject);
        }
    }
}
