using UnityEngine;
using UnityEngine.InputSystem;

public class Game_Manager : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector2 vector;
    private Vector3 moveDirection;

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        moveDirection = new Vector3(vector.x, 0f, vector.y);
        transform.Translate(moveSpeed * Time.deltaTime * moveDirection, Space.World);
    }

    void OnMove(InputValue value) => vector = value.Get<Vector2>();
}
