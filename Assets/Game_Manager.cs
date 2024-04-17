using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;

public class Game_Manager : MonoBehaviour
{
    #region Variables & References
        public float moveSpeed = 5f;
        private Vector2 vector;
        private Vector3 moveDirection;

        private bool isPaused = false;

        private Camera mainCamera;
        private float minX, maxX, minY, maxY;
        private float playerWidth, playerHeight;

        public AudioSource keyPressSound;
        public GameObject coinPrefab; // Prefab of the coin to spawn
        public GameObject pauseCanvas;
        public GameObject lostCanvas;
        public TextMeshProUGUI healthText;
        public TextMeshProUGUI scoreText;
        public TextMeshProUGUI topScoreText;
        private int score = 0;
        private int topScore = 0;
        private int healthPoints = 3;

        private float spawnYTop; // Y position for top spawn
        private float spawnYBottom; // Y position for bottom spawn
        private bool spawnAtTop = true; // Flag to track current spawn position

        // Singleton pattern
        private static Game_Manager _instance;
        public static Game_Manager Instance { get { return _instance; } }

        // Immunity variables
        private bool isImmune = false;
        private readonly float immunityDuration = 2f;
        private Color immuneColor;
        private Color originalColor;
    #endregion

    #region Default Unity
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
            // Spawn a coin at the start of the game
            SpawnNewCoin();

            // Load top score from PlayerPrefs
            if(PlayerPrefs.HasKey("TopScore"))
            {
                topScore = PlayerPrefs.GetInt("TopScore");
                UpdateTopScoreText();
            }
        }

        void Update()
        {
            Movement();
        }
    #endregion

    #region Coin Pickup
        public void CoinDestroyed(Coin coin)
        {
            score++;
            UpdateScoreText();
            SpawnNewCoin();
        }

        // Function to spawn a new coin
        private void SpawnNewCoin()
        {
            float spawnY = spawnAtTop ? spawnYTop : spawnYBottom;
            spawnAtTop = !spawnAtTop;
            float randomX = Random.Range(minX, maxX);
            Vector3 spawnPosition = new(randomX, spawnY);
            Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
        }

        void UpdateScoreText()
        {
            if (scoreText != null)
            {
                scoreText.text = "Score: " + score.ToString();
            }
        }

        // Function to update top score text
        private void UpdateTopScoreText()
        {
            if (topScoreText != null)
            {
                topScoreText.text = "Top Score: " + topScore.ToString();
            }
        }

        // Function to save top score
        private void SaveTopScore()
        {
            PlayerPrefs.SetInt("TopScore", topScore);
            PlayerPrefs.Save();
        }

        // Function to check and update top score
        private void CheckAndUpdateTopScore()
        {
            if (score > topScore)
            {
                topScore = score;
                UpdateTopScoreText();
                SaveTopScore(); // Save new top score
            }
        }
    #endregion

    #region Player
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
    #endregion

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
            keyPressSound.Play();
            if (pauseCanvas != null)
            {
                pauseCanvas.SetActive(isPaused);
            }

            Time.timeScale = isPaused ? 0 : 1;
        }
    #endregion

    // Function called when the player collides with something
    void OnTriggerEnter2D(Collider2D other)
    {
        EnemyObject(other);
    }

    #region Death & Health
        private void EnemyObject(Collider2D other)
        {
            if (other.CompareTag("EnemyObject"))
            {
                if (!isImmune)
                {
                    StartCoroutine(ImmunityCoroutine());

                    if (healthPoints <= 0)
                    {
                        CheckAndUpdateTopScore();
                        lostCanvas.SetActive(true);
                        Time.timeScale = 0;
                    }
                }
            }
        }

        // Coroutine for immunity effect
        IEnumerator ImmunityCoroutine()
        {
            healthPoints--;
            UpdateHealthText();
            isImmune = true;
            Debug.Log("Immunity Active.");
            
            Color originalColor = transform.GetComponent<SpriteRenderer>().color;

            immuneColor = Color.red;
            immuneColor.a = 0.3f; // Transparency
            transform.GetComponent<SpriteRenderer>().color = immuneColor; 

            yield return new WaitForSeconds(immunityDuration);
            
            transform.GetComponent<SpriteRenderer>().color = originalColor;

            isImmune = false;
            Debug.Log("You can now receive damage.");
        }

        // Update health text to display current health points
        void UpdateHealthText()
        {
            if (healthText != null)
            {
                healthText.text = "HP: " + healthPoints.ToString();
            }
        }
    #endregion
}
