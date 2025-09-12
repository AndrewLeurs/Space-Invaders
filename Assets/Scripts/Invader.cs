using UnityEngine;
using UnityEngine.SceneManagement;

public class Invader : MonoBehaviour
{
    
    public Sprite[] animationSprites;
    public float animationTime = 1.0f;
    private SpriteRenderer _spriteRenderer;
    private int _animationFrame;
    public GameObject missile;
    private float timer = 0f;
    private float randTime;
    private Manager scoreScript;
    public int scoreValue = 10;
    private Player player;
    public Sprite impactSprite;
    private bool isHit = false;
    private float hitTimer = 0f;
    public int rowIndex;
    private Invaders invaders;
    private enum MovePhase { Phase1, Phase2, Phase3, Phase4, Phase5, Phase6, Phase7, Phase8 }
    private MovePhase movePhase = MovePhase.Phase1;
    float timeUntilDrop;
    int dropLevel;
    float dropTimer = 0;
    private Vector3 startPosition;
    int direction;
    private AudioSource audioSource;
    public AudioClip deathSound;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        invaders = FindFirstObjectByType<Invaders>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Start()
    {
        InvokeRepeating(nameof(AnimateSprite), animationTime, animationTime);
        if (invaders != null)
        {
            randTime = Random.Range(1.0f, invaders.fireRate);
        }
        else
        {
            randTime = Random.Range(1.0f, 15f); // Default fallback
        }
        scoreScript = FindFirstObjectByType<Manager>();
        player = FindFirstObjectByType<Player>();
        timeUntilDrop = Random.Range(1f, 15f);
        dropLevel = Random.Range(-9, 12);
        startPosition = transform.position;
        direction = Random.value < 0.5f ? -1 : 1;
    }

    private void Update()
    {
        if (isHit)
        {
            hitTimer += Time.deltaTime;
            _spriteRenderer.sprite = impactSprite;
            if (hitTimer >= 0.2f)
            {
                Destroy(gameObject);
            }
            return;
        }

        timer += Time.deltaTime;

        if (timer >= randTime)
        {
            if (player != null && !player.IsDead())
            {
                Instantiate(missile, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
            }
            if (invaders != null)
            {
                randTime = Random.Range(1.0f, invaders.fireRate);
            }
            else
            {
                randTime = Random.Range(1.0f, 15f); // Default fallback
            }
            timer = 0f;
        }

        if (invaders == null) return; // Early exit if invaders is null

        float moveSpeed = invaders.speed * Time.deltaTime;
        Vector2 diagUpRgt = new Vector2(1, 2).normalized;
        Vector2 diagUpLft = new Vector2(-1, 2).normalized;

        if (rowIndex == 4)
        {    
            switch (movePhase)
            {
                case MovePhase.Phase1:
                    transform.Translate(Vector3.left * moveSpeed);
                    if (transform.position.x <= -12f) movePhase = MovePhase.Phase2;
                    break;
                case MovePhase.Phase2:
                    transform.Translate(Vector3.down * moveSpeed);
                    if (transform.position.y <= 6f) movePhase = MovePhase.Phase3;
                    break;
                case MovePhase.Phase3:
                    transform.Translate(Vector3.right * moveSpeed);
                    if (transform.position.x >= -1f) movePhase = MovePhase.Phase4;
                    break;
                case MovePhase.Phase4:
                    transform.Translate(Vector3.down * moveSpeed);
                    if (transform.position.y <= 0f) movePhase = MovePhase.Phase5;
                    break;
                case MovePhase.Phase5:
                    transform.Translate(Vector3.left * moveSpeed);
                    if (transform.position.x <= -12f) movePhase = MovePhase.Phase6;
                    break;
                case MovePhase.Phase6:
                    transform.Translate(Vector3.down * moveSpeed);
                    if (transform.position.y <= -4f) movePhase = MovePhase.Phase7;
                    break;
                case MovePhase.Phase7:
                    transform.Translate(Vector3.right * moveSpeed);
                    if (transform.position.x >= -1f) movePhase = MovePhase.Phase8;
                    break;
                case MovePhase.Phase8:
                    transform.Translate(diagUpRgt * moveSpeed);
                    if (transform.position.y >= 16f) movePhase = MovePhase.Phase1; // or stop, or loop as you wish
                    break;
            }
        }

        if (rowIndex == 3)
        {
            switch (movePhase)
            {
                case MovePhase.Phase1:
                    transform.Translate(Vector3.right * moveSpeed);
                    if (transform.position.x >= 12f) movePhase = MovePhase.Phase2;
                    break;
                case MovePhase.Phase2:
                    transform.Translate(Vector3.down * moveSpeed);
                    if (transform.position.y <= 6f) movePhase = MovePhase.Phase3;
                    break;
                case MovePhase.Phase3:
                    transform.Translate(Vector3.left * moveSpeed);
                    if (transform.position.x <= 1f) movePhase = MovePhase.Phase4;
                    break;
                case MovePhase.Phase4:
                    transform.Translate(Vector3.down * moveSpeed);
                    if (transform.position.y <= 0f) movePhase = MovePhase.Phase5;
                    break;
                case MovePhase.Phase5:
                    transform.Translate(Vector3.right * moveSpeed);
                    if (transform.position.x >= 12f) movePhase = MovePhase.Phase6;
                    break;
                case MovePhase.Phase6:
                    transform.Translate(Vector3.down * moveSpeed);
                    if (transform.position.y <= -4f) movePhase = MovePhase.Phase7;
                    break;
                case MovePhase.Phase7:
                    transform.Translate(Vector3.left * moveSpeed);
                    if (transform.position.x <= 1f) movePhase = MovePhase.Phase8;
                    break;
                case MovePhase.Phase8:
                    transform.Translate(diagUpLft * moveSpeed);
                    if (transform.position.y >= 16f) movePhase = MovePhase.Phase1; // or stop, or loop as you wish
                    break;
            }
        }

        if ((rowIndex == 2 || rowIndex == 1) && invaders.partOneComplete)
        {
            dropTimer += Time.deltaTime;

            if (dropTimer >= timeUntilDrop && transform.position.y >= dropLevel)
            {
                transform.Translate(Vector3.down * moveSpeed);
            }
            else if (dropTimer >= timeUntilDrop && transform.position.y <= dropLevel)
            {
                switch (movePhase)
                {
                    case MovePhase.Phase1:
                        transform.Translate(Vector3.left * moveSpeed);
                        if (transform.position.x <= -12f) movePhase = MovePhase.Phase2;
                        break;
                    case MovePhase.Phase2:
                        transform.Translate(Vector3.right * moveSpeed);
                        if (transform.position.x >= 12f) movePhase = MovePhase.Phase1;
                        break;
                }
            }
        }
        else if (rowIndex == 0 && invaders.partTwoComplete)
        {
            dropTimer += Time.deltaTime;

            if (dropTimer >= timeUntilDrop && transform.position.y >= -8f)
            {
                // Sine wave movement
                float amplitude = 12f; // x limits: -12 to 12
                float frequency = 0.5f; // Adjust for wave width
                float newY = transform.position.y - moveSpeed / 3;
                float newX = Mathf.Sin(newY * frequency) * amplitude;
                transform.position = new Vector3(newX, newY, transform.position.z);
            }
            else if (transform.position.y < -8f)
            {
                transform.Translate(Vector3.right * moveSpeed * direction);

                if (transform.position.x > 17f || transform.position.x < -17f)
                {
                    transform.position = startPosition;
                    direction = Random.value < 0.5f ? -1 : 1;
                }
            }
        }
    }

    private void AnimateSprite()
    {
        _animationFrame++;

        if (_animationFrame >= animationSprites.Length)
        {
            _animationFrame = 0;
        }

        _spriteRenderer.sprite = animationSprites[_animationFrame];
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("player laser") && !isHit)
        {
            scoreScript.AddScore(scoreValue);
            isHit = true;
            hitTimer = 0f;
            
            // Play death sound
            if (deathSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(deathSound);
            }
            
            if (invaders != null && invaders.speed <= 15f)
            {
                invaders.speed += 0.05f;
                invaders.originalSpeed += 0.05f;
                invaders.fireRate = Mathf.Max(invaders.fireRate - 0.03f, 1.1f);
                Emissile.speed += 0.03f; // Increase static speed for all emissiles
            }
        }
    }
}
