using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float speed = 6.0f;
    public Sprite deathSprite;
    private SpriteRenderer spriteRenderer;
    private Sprite originalSprite;
    private float originalSpeed;
    private bool isDead = false;
    public GameObject plaser;
    public float waitTime = 2.0f;
    private float timer;
    public AudioClip shootSound;
    private AudioSource audioSource;
    public AudioClip deathSound;

    public bool IsDead()
    {
        return isDead;
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalSprite = spriteRenderer.sprite;
        originalSpeed = speed;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) && timer >= waitTime && !isDead)
        {
            Vector3 player_position = transform.position;
            Instantiate(plaser, player_position, Quaternion.identity);
            timer = 0;
            audioSource.PlayOneShot(shootSound);
        }

        if (transform.position.x < -13)
        {
            transform.position = new Vector3(-13, transform.position.y, transform.position.z);
        }
        else if (transform.position.x > 13)
        {
            transform.position = new Vector3(13, transform.position.y, transform.position.z);
        }

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(UnityEngine.Collider2D other)
    {
        if (other.CompareTag("Emissile") && !isDead)
        {
            StartCoroutine(Die());
            Manager.lives--;
            Manager.Instance.UpdateLivesText(Manager.lives);
            audioSource.PlayOneShot(deathSound);
        }
    }

    private System.Collections.IEnumerator Die()
    {
        isDead = true;
        spriteRenderer.sprite = deathSprite;
        speed = 0f;
        yield return new WaitForSeconds(3f);
        spriteRenderer.sprite = originalSprite;
        speed = originalSpeed;
        isDead = false;

        if (Manager.lives <= 0)
        {
            Destroy(gameObject);
            SceneManager.LoadScene("GameOver");
        }
    }
}
