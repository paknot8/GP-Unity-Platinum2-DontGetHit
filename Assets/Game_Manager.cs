using UnityEngine;
using UnityEngine.InputSystem;

public class Game_Manager : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector2 vector;
    private Vector3 moveDirection;

    private Camera mainCamera;
    private float minX, maxX, minY, maxY;
    private float playerWidth, playerHeight;

    void Start()
    {
        mainCamera = Camera.main;

        // Calculate camera bounds
        float camHeight = 2f * mainCamera.orthographicSize;
        float camWidth = camHeight * mainCamera.aspect;

        // Calculate player size
        playerWidth = transform.localScale.x;
        playerHeight = transform.localScale.y;

        // Calculate bounds considering player size
        minX = mainCamera.transform.position.x - camWidth / 2f + playerWidth / 2f;
        maxX = mainCamera.transform.position.x + camWidth / 2f - playerWidth / 2f;
        minY = mainCamera.transform.position.y - camHeight / 2f + playerHeight / 2f;
        maxY = mainCamera.transform.position.y + camHeight / 2f - playerHeight / 2f;
    }

    void Update()
    {
        Movement();
    }

    void Movement()
    {
        moveDirection = new Vector3(vector.x, vector.y);
        Vector3 newPosition = transform.position + moveSpeed * Time.deltaTime * moveDirection;

        // Clamp player's position within camera bounds
        float clampedX = Mathf.Clamp(newPosition.x, minX, maxX);
        float clampedY = Mathf.Clamp(newPosition.y, minY, maxY);
        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }

    void OnMove(InputValue value) => vector = value.Get<Vector2>();
}
