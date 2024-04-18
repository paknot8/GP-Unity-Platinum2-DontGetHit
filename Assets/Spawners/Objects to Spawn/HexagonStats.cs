using UnityEngine;

public class HexagonStats : MonoBehaviour
{
    public float moveSpeed;
    public float destroyPosition;

    void Awake()
    {
        moveSpeed = 8f;
        destroyPosition = -100f;
    }

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
