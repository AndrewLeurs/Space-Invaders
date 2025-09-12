using Unity.VisualScripting;
using UnityEngine;

public class BunkerPiece : MonoBehaviour
{
    public Sprite[] sprites;
    private int hits = 0;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D box;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        box = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Emissile") || other.CompareTag("player laser"))
        {
            if (hits >= 3)
            {
                Destroy(gameObject);
            }
            else
            {
                hits++;
                spriteRenderer.sprite = sprites[hits];
                Bounds bounds = box.bounds;
                Vector3 bottomCenter = new Vector3(bounds.center.x, bounds.min.y, transform.position.z);
                float size = spriteRenderer.bounds.size.y;
                Vector3 spriteOffset = new Vector3(0, size / 2, 0);
                spriteRenderer.transform.position = bottomCenter + spriteOffset;
            }
        }
    }
}
