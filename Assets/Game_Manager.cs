using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;

public class Game_Manager : MonoBehaviour
{
    #region Variables & References
        [Header("Player Variables")]
        public float moveSpeed = 5f;
        public static bool isPaused = false;
        public static bool inGame = false;
        public static bool hardMode = false;
        public GameObject hardModeSpawner;
        [HideInInspector] public Vector2 vector;
        [HideInInspector] public Vector3 moveDirection;

        // --- Camera Border Check --- //
        private Camera mainCamera;
        private float minX, maxX, minY, maxY;
        private float playerWidth, playerHeight;

        [Header("Music & Sound")]
        public AudioSource keyPressSound;

        [Header("Canvas")]
        public GameObject pauseCanvas;
        public GameObject lostCanvas;

        [Header("Player Score")]
        public TextMeshProUGUI scoreText;
        public TextMeshProUGUI topScoreText;
        private int score = 0;
        private int topScore = 0;

        [Header("Player Health")]
        public AudioSource LoseHealthSound;
        public TextMeshProUGUI healthText;
        public int healthPoints = 3;

        [Header("Coins")]
        public GameObject coinPrefab;
        public AudioSource CoinPickSound;
        private float spawnYTop; // Y position for top spawn
        private float spawnYBottom; // Y position for bottom spawn
        private bool spawnAtTop = true; // Flag to track current spawn position

        // Singleton pattern
        private static Game_Manager _instance;
        [HideInInspector] public static Game_Manager Instance { get { return _instance; } }

        // Immunity variables
        private bool isImmune = false;
        private readonly float immunityDuration = 0.5f;
        private Color immuneColor;
        private Color originalColor;

        // --- Player States --- //
        [HideInInspector] public PlayerBaseState playerState;
        [HideInInspector] public PlayerIdleState idleState = new();
        [HideInInspector] public PlayerMoveState moveState = new();
    #endregion

    #region Default Unity
        void Awake()
        {
            SingletonInstance();
        }

        // Creates a singleton instance of the GameManager.
        private void SingletonInstance()
        {
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
            TimeAndDifficulty();
            InitializeGameState();
            PlayerToCameraBorderCheck();
            InitializePlayerAppearance();
            InitializeCoinSpawn();
            LoadTopScoreFromPlayerPrefs();
        }

        private void TimeAndDifficulty()
        {
            Time.timeScale = 1; // Always start game on normal mode
            if(hardMode){
                hardModeSpawner.SetActive(true);
            } else {
                hardModeSpawner.SetActive(false);
            }
        }

        private void InitializeGameState()
        {
            playerState = idleState;
            playerState.EnterState(this);
        }

        private void InitializePlayerAppearance()
        {
            originalColor = transform.GetComponent<SpriteRenderer>().color;
            UpdateHealthText();
        }

        private void InitializeCoinSpawn()
        {
            spawnYTop = maxY;
            spawnYBottom = minY;
            SpawnNewCoin(); // Spawn the 1st coin at the start of the game
        }

        private void LoadTopScoreFromPlayerPrefs()
        {
            if(PlayerPrefs.HasKey("TopScore"))
            {
                topScore = PlayerPrefs.GetInt("TopScore");
                UpdateTopScoreText();
            }
        }

        void Update()
        {
            playerState.UpdateState(this);
        }

        // Switches the player state to the given state.
        public void SwitchState(PlayerBaseState state)
        {
            playerState.ExitState(this);
            playerState = state ?? playerState; // If state is null -> stay on the playerState
            playerState.EnterState(this);
        }
    #endregion

    #region Coin Pickup
        // Handles the destruction of a coin when picked up by the player.
        public void CoinDestroyed(Coin coin)
        {
            CoinPickSound.Play();

            // If on hard mode, you get 2 poitns each score
            if(hardMode){
                score+=2;
            } else {
                score++;
            }
            
            UpdateScoreText();
            SpawnNewCoin();
        }

        // Spawns a new coin at a random location.
        private void SpawnNewCoin()
        {
            float spawnY = spawnAtTop ? spawnYTop : spawnYBottom;
            spawnAtTop = !spawnAtTop;
            float randomX = Random.Range(minX, maxX);
            Vector3 spawnPosition = new(randomX, spawnY);
            Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
        }

        // Updates the score text UI.
        void UpdateScoreText()
        {
            if (scoreText != null)
            {
                scoreText.text = "Score: " + score.ToString();
            }
        }

        // Updates the top score text UI.
        private void UpdateTopScoreText()
        {
            if (topScoreText != null)
            {
                topScoreText.text = "Top Score: " + topScore.ToString();
            }
        }

        // Saves the top score to player preferences.
        private void SaveTopScore()
        {
            PlayerPrefs.SetInt("TopScore", topScore);
            PlayerPrefs.Save();
        }

        // Checks if the current score exceeds the top score, and updates it if necessary.
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
        // Checks if the player is within the camera borders.
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

        // Handles player movement based on input.
        // public void Movement()
        // {
        //     moveDirection = new Vector3(vector.x, vector.y);
        //     Vector3 newPosition = transform.position + moveSpeed * Time.deltaTime * moveDirection;

        //     float clampedX = Mathf.Clamp(newPosition.x, minX, maxX);
        //     float clampedY = Mathf.Clamp(newPosition.y, minY, maxY);
        //     transform.position = new Vector3(clampedX, clampedY, transform.position.z);
        // }

        // Handles player movement based on input.
        public void Movement()
        {
            moveDirection = new Vector3(vector.x, vector.y);
            Vector3 newPosition = transform.position + moveSpeed * Time.deltaTime * moveDirection;

            float clampedX = Mathf.Clamp(newPosition.x, minX, maxX);
            float clampedY = Mathf.Clamp(newPosition.y, minY, maxY);
            transform.position = new Vector3(clampedX, clampedY, transform.position.z);

            // Calculate angle between current position and target position
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;

            // Rotate the player sprite to face the movement direction
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    #endregion

    #region New Input System
        // Handles player movement input.
        void OnMove(InputValue value) => vector = value.Get<Vector2>();

        // Handles pause input.
        void OnPause(InputValue value){
            if (value.isPressed)
            {
                keyPressSound.Play();
                PauseGame();
            }
        }

        // Pauses or resumes the game.
        private void PauseGame()
        {
            if (!isPaused)
            {
                pauseCanvas.SetActive(true);
                Time.timeScale = 0;
                isPaused = true;
            }
            else
            {
                Time.timeScale = 1;
                pauseCanvas.SetActive(false);
                isPaused = false;
            }
        }
    #endregion

    // Handles collisions with the player.
    void OnTriggerEnter2D(Collider2D other)
    {
        EnemyObject(other);
    }

    #region Death & Health
        // Handles interactions with enemy objects.
        private void EnemyObject(Collider2D other)
        {
            if (other.CompareTag("EnemyObject"))
            {
                LoseHealthSound.pitch = 2f; // Increase pitch to increase playback
                LoseHealthSound.Play();
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

        // Coroutine for the immunity effect.
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

        // Updates the health text UI.
        void UpdateHealthText()
        {
            if (healthText != null)
            {
                healthText.text = "HP: " + healthPoints.ToString();
            }
        }
    #endregion
}
