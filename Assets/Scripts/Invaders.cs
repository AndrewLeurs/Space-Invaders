using UnityEngine;

public class Invaders : MonoBehaviour
{
    public Invader[] prefabs;
    public int rows = 5;
    public int columns = 12;
    public float speed = 5.0f;
    public float originalSpeed;
    public float fireRate = 15f;
    private Player player;
    private float waveTimer = 0f;
    private Manager scoreScript;
    public bool partOneComplete = false;
    public bool partTwoComplete = false;
    private bool waveIncreased = false;
    private float initialWaveTimer = 2f;
    private bool initialWaveSpawned = false;
    private float instructionsTimer = 4f;
    private bool instructionsShown = false;
    public GameObject instructionsText;

    private void SpawnInvaders()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        Color color1 = Color.HSVToRGB(Random.value, 1f, 1f);
        Color color2 = Color.HSVToRGB(Random.value, 1f, 1f);
        Color color3 = Color.HSVToRGB(Random.value, 1f, 1f);

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                Vector3 position = new Vector3(-11.0f + 2.0f * col, 16f, 0.0f);
                Invader invader = Instantiate(prefabs[row], position, Quaternion.identity, this.transform);
                invader.rowIndex = row;
  
                if (row == 0)
                {
                    invader.GetComponent<SpriteRenderer>().color = color1;
                }
                else if (row == 1 || row == 2)
                {
                    invader.GetComponent<SpriteRenderer>().color = color2;
                }
                else if (row == 3|| row == 4)
                {
                    invader.GetComponent<SpriteRenderer>().color = color3;
                }
            }
        }
    }

    private void Start()
    {
        originalSpeed = speed;
        player = FindFirstObjectByType<Player>();
        scoreScript = FindFirstObjectByType<Manager>();
        Manager.Instance.UpdateLivesText(Manager.lives);

        if (instructionsText != null)
            instructionsText.SetActive(true);

        if (Manager.Instance.waveText != null)
            Manager.Instance.waveText.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (!instructionsShown)
        {
            instructionsTimer -= Time.fixedDeltaTime;
            if (instructionsTimer <= 0f)
            {
                if (instructionsText != null)
                    instructionsText.SetActive(false);
                if (Manager.Instance.waveText != null)
                    Manager.Instance.waveText.gameObject.SetActive(true);
                instructionsShown = true;
            }
            return;
        }

        if (!initialWaveSpawned)
        {
            initialWaveTimer -= Time.fixedDeltaTime;
            if (initialWaveTimer <= 0f)
            {
                if (Manager.Instance.waveText != null)
                    Manager.Instance.waveText.gameObject.SetActive(false);
                SpawnInvaders();
                initialWaveSpawned = true;
            }
            return;
        }

        if (player.IsDead())
        {
            speed = 0f;
        }
        else
        {
            speed = originalSpeed;
        }

        bool allGone = true;
        foreach (Transform invader in transform)
        {
            if (invader.gameObject.activeSelf)
            {
                allGone = false;
                break;
            } 
        }

        // Check if all invaders in rows 3 and 4 are destroyed
        if (!partOneComplete) {
            bool allRow3And4Gone = true;
            foreach (Transform invader in transform) {
                Invader invaderScript = invader.GetComponent<Invader>();
                if (invaderScript != null && (invaderScript.rowIndex == 3 || invaderScript.rowIndex == 4)) {
                    if (invader.gameObject.activeSelf) {
                        allRow3And4Gone = false;
                        break;
                    }
                }
            }
            if (allRow3And4Gone) {
                partOneComplete = true;
            }
        }
        else if (!partTwoComplete)
        {
            bool allRow1And2Gone = true;
            foreach (Transform invader in transform) {
                Invader invaderScript = invader.GetComponent<Invader>();
                if (invaderScript != null && (invaderScript.rowIndex == 1 || invaderScript.rowIndex == 2)) {
                    if (invader.gameObject.activeSelf) {
                        allRow1And2Gone = false;
                        break;
                    }
                }
            }
            if (allRow1And2Gone) {
                partTwoComplete = true;
            }
        }
        
        
        if (allGone)
        {
            waveTimer += Time.deltaTime;

            if (Manager.Instance.waveText != null)
            {
                Manager.Instance.waveText.gameObject.SetActive(true);
            }
            
            if (waveIncreased == false)
            {
                scoreScript.AddWave();
                waveIncreased = true;
            }
            
            if (waveTimer > 2f)
            {
                if (Manager.Instance.waveText != null)
                {
                    Manager.Instance.waveText.gameObject.SetActive(false);
                }
                
                SpawnInvaders();
                waveTimer = 0f;
                partOneComplete = false;
                partTwoComplete = false;
                waveIncreased = false;
            }
        }
    }
}
