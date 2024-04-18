using UnityEngine;

public class TriangleStats : MonoBehaviour
{
    public float moveSpeed;
    public float destroyPosition;

    void Awake()
    {
        moveSpeed = 7f;
        destroyPosition = 100f;
    
    }

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
