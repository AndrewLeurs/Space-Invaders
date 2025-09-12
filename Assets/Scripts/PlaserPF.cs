using UnityEngine;

public class PlaserPF : MonoBehaviour
{
    public float speed = 10f;
    public GameObject explosion;

    void Start()
    {
        Destroy(gameObject, 2.0f);
    }

    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
        
    }

    private void OnTriggerStay2D(UnityEngine.Collider2D other)
    {
        if (other.CompareTag("Bunker") || other.CompareTag("Invader"))
        {
            Destroy(gameObject);
        }
    }
}
