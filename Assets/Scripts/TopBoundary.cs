using UnityEngine;

public class TopBoundary : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Emissile") || other.CompareTag("player laser"))
            Destroy(other.gameObject);
    }
}
