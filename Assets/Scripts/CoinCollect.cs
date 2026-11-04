
using UnityEngine;

public class CoinCollect : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (CoinManager.Instance != null)
            {
                CoinManager.Instance.AddCoin();
            }

            Destroy(gameObject);
        }
    }
}
