using UnityEngine;

public class CapsuleStats : MonoBehaviour
{
    public float moveSpeed = 6f;
    public float destroyPosition = -30f;

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
