using UnityEngine;

public class Emissile : MonoBehaviour
{
    public static float speed = 10f; // Static speed shared by all emissiles
    private Player player;

    private void Start()
    {
        player = FindFirstObjectByType<Player>();
    }

    void Update()
    {
        if (player != null && player.IsDead())
        {
            Destroy(gameObject);
        }
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(UnityEngine.Collider2D other)
    {
        if (other.CompareTag("Bunker") || other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
