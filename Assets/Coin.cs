using UnityEngine;

public class Coin : MonoBehaviour
{
    public GameObject coinPrefab;
    public float spawnYTop;
    public float spawnYBottom;
    public float minX;
    public float maxX;

    private bool spawnAtTop = true;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Destroy the coin when it collides with the player
            Destroy(gameObject);
            // Send message to GameManager that the coin has been destroyed
            Game_Manager.Instance.CoinDestroyed(this);
        }
    }

    // Function called when a coin is collected
    public void CollectCoin(Collider2D other)
    {
        if (other.CompareTag("Coins"))
        {
            // Determine spawn position based on spawnAtTop flag
            float spawnY = spawnAtTop ? spawnYTop : spawnYBottom;

            // Toggle spawn position flag
            spawnAtTop = !spawnAtTop;

            float randomX = Random.Range(minX, maxX);
            Vector3 spawnPosition = new Vector3(randomX, spawnY, 0f);
            Instantiate(coinPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
