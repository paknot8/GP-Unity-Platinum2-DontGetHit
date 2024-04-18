using UnityEngine;

public class CircleStats : MonoBehaviour
{
    public float moveSpeed;
    public float destroyPosition;

    void Awake()
    {
        moveSpeed = 4f;
        destroyPosition = 100f;
    }

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
