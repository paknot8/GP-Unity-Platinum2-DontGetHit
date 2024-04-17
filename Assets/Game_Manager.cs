using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;

public class Game_Manager : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector2 vector;
    private Vector3 moveDirection;

    private bool isPaused = false;

    private Camera mainCamera;
    private float minX, maxX, minY, maxY;
    private float playerWidth, playerHeight;

    public GameObject coinPrefab; // Prefab of the coin to spawn
    public GameObject pauseCanvas;
    public TextMeshProUGUI healthText; // Text object for displaying health points

    private float spawnYTop; // Y position for top spawn
    private float spawnYBottom; // Y position for bottom spawn
    private bool spawnAtTop = false; // Flag to track current spawn position

    // Singleton pattern
    private static Game_Manager _instance;
    public static Game_Manager Instance { get { return _instance; } }

    private int healthPoints = 3; // Initial health points

    // Immunity variables
    private bool isImmune = false;
    private readonly float immunityDuration = 2f;
    private Color immuneColor;
    private Color originalColor;

    void Awake()
    {
        if (transform.GetComponent<SpriteRenderer>() == null)
        {
            Debug.LogError("SpriteRenderer component not found on player GameObject.");
        }

        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    void Start()
    {
        PlayerToCameraBorderCheck();

        originalColor = transform.GetComponent<SpriteRenderer>().color;

        spawnYTop = maxY;
        spawnYBottom = minY;

        UpdateHealthText();
    }

    void Update()
    {
        Movement();
    }

    // Method to receive message that a coin has been destroyed
    public void CoinDestroyed(Coin coin)
    {
        // Instantiate a new coin
        float spawnY = spawnAtTop ? spawnYTop : spawnYBottom;
        spawnAtTop = !spawnAtTop;
        float randomX = Random.Range(minX, maxX);
        Vector3 spawnPosition = new Vector3(randomX, spawnY, 0f);
        Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
    }

    private void PlayerToCameraBorderCheck()
    {
        mainCamera = Camera.main;

        float camHeight = 2f * mainCamera.orthographicSize;
        float camWidth = camHeight * mainCamera.aspect;

        playerWidth = transform.localScale.x;
        playerHeight = transform.localScale.y;

        minX = mainCamera.transform.position.x - camWidth / 2f + playerWidth / 2f;
        maxX = mainCamera.transform.position.x + camWidth / 2f - playerWidth / 2f;
        minY = mainCamera.transform.position.y - camHeight / 2f + playerHeight / 2f;
        maxY = mainCamera.transform.position.y + camHeight / 2f - playerHeight / 2f;
    }

    void Movement()
    {
        moveDirection = new Vector3(vector.x, vector.y);
        Vector3 newPosition = transform.position + moveSpeed * Time.deltaTime * moveDirection;

        float clampedX = Mathf.Clamp(newPosition.x, minX, maxX);
        float clampedY = Mathf.Clamp(newPosition.y, minY, maxY);
        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }

    #region New Input System
    void OnMove(InputValue value) => vector = value.Get<Vector2>();
    void OnPause(InputValue value){
        if (value.isPressed)
        {
            PauseGame();
        }
    }

    private void PauseGame()
    {
        isPaused = !isPaused;

        if (pauseCanvas != null)
        {
            pauseCanvas.SetActive(isPaused);
        }

        Time.timeScale = isPaused ? 0 : 1;
    }
    #endregion

    // Function called when the player collides with an enemy object
    void OnTriggerEnter2D(Collider2D other)
    {
        EnemyObject(other);
    }

    // // Function called when a coin is collected
    // public void CollectCoin(Collider2D other)
    // {
    //     if (transform.CompareTag("Coins")){
    //         // Determine spawn position based on spawnAtTop flag
    //         float spawnY = spawnAtTop ? spawnYTop : spawnYBottom;

    //         // Toggle spawn position flag
    //         spawnAtTop = !spawnAtTop;

    //         float randomX = Random.Range(minX, maxX);
    //         Vector3 spawnPosition = new Vector3(randomX, spawnY, 0f);
    //         Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
    //     }
    // }

    private void EnemyObject(Collider2D other)
    {
        if (other.CompareTag("EnemyObject"))
        {
            if (!isImmune)
            {
                StartCoroutine(ImmunityCoroutine());

                if (healthPoints <= 0)
                {
                    Time.timeScale = 0;
                    // Open Death menu
                }
            }
        }
    }

    // Update health text to display current health points
    void UpdateHealthText()
    {
        if (healthText != null)
        {
            healthText.text = "" + healthPoints.ToString();
        }
    }

    // Coroutine for immunity effect
    IEnumerator ImmunityCoroutine()
    {
        healthPoints--;
        UpdateHealthText();
        isImmune = true;
        Debug.Log("Immune");
        
        Color originalColor = transform.GetComponent<SpriteRenderer>().color;

        immuneColor = Color.red;
        immuneColor.a = 0.3f; // Transparency
        transform.GetComponent<SpriteRenderer>().color = immuneColor; 

        yield return new WaitForSeconds(immunityDuration);
        
        transform.GetComponent<SpriteRenderer>().color = originalColor;

        isImmune = false;
        Debug.Log("You can now receive damage");
    }
}
