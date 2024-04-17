using UnityEngine;

public class Coin : MonoBehaviour
{
    public GameObject coinPrefab;
    public float spawnYTop;
    public float spawnYBottom;
    public float minX;
    public float maxX;

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
}
