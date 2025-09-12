using UnityEngine;
using UnityEngine.UIElements;

public class MysterShip : MonoBehaviour
{
    public float speed = 3f;
    private Manager scoreScript;
    public int scoreValue;
    private Player player;
    public AudioClip deathSound;
    public Sprite deathSprite;
    private AudioSource audioSource;
    private SpriteRenderer spriteRenderer;
    private bool isDying = false;
    private float deathTimer = 0f;
    public GameObject floatingTextPrefab;
    private GameObject floatingTextInstance;

    private void Start()
    {
        scoreScript = FindFirstObjectByType<Manager>();
        int[] scoreValues = { 50, 60, 70, 80, 90, 100 };
        scoreValue = scoreValues[Random.Range(0, scoreValues.Length)];
        player = FindFirstObjectByType<Player>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (isDying)
        {
            deathTimer += Time.deltaTime;
            if (deathTimer >= 0.5f)
            {
                Destroy(gameObject);
            }
            return;
        }

        if (!player.IsDead())
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }

        if (transform.position.x > 17f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(UnityEngine.Collider2D other)
    {
        if (other.CompareTag("player laser") && !isDying)
        {
            scoreScript.AddScore(scoreValue);
            isDying = true;
            deathTimer = 0f;
            if (deathSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(deathSound);
            }

            if (deathSprite != null && spriteRenderer != null)
            {
                spriteRenderer.sprite = deathSprite;
            }

            if (floatingTextPrefab != null)
            {
                Vector3 offset = new Vector3(0.2f, 0, 0); // Adjust as needed
                Canvas canvas = FindFirstObjectByType<Canvas>();
                if (canvas != null)
                {
                    Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + offset);
                    floatingTextInstance = Instantiate(floatingTextPrefab, canvas.transform);
                    floatingTextInstance.transform.position = screenPos;
                    var tmp = floatingTextInstance.GetComponent<TMPro.TextMeshProUGUI>();
                    if (tmp != null)
                        tmp.text = scoreValue.ToString();
                }
            }

            // Destroy the player laser
            Destroy(other.gameObject);
        }
    }
}
