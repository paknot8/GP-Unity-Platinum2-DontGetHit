using UnityEngine;

// This script manages the behavior of coins in the game.
public class Coin : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
            // Notify the GameManager that the coin is destroyed
            Game_Manager.Instance.CoinDestroyed(this);
        }
    }
}
