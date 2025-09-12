using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;

public class Manager : MonoBehaviour
{
    public static int score = 0;
    public static int lives = 3;
    public static int wave = 1;
    public static Manager Instance;
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI livesText;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateLivesText(lives);
        UpdateWaveText(wave);
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        var allTexts = GameObject.FindObjectsByType<TextMeshProUGUI>(UnityEngine.FindObjectsInactive.Include, UnityEngine.FindObjectsSortMode.None);

        waveText = allTexts.FirstOrDefault(t => t.gameObject.name == "WaveText");
        livesText = allTexts.FirstOrDefault(t => t.gameObject.name == "LivesText");
        UpdateLivesText(lives);
        UpdateWaveText(wave);
    }

    public void UpdateLivesText(int lives)
    {
        if (livesText != null)
            livesText.text = "Lives: " + lives;
    }

    public void UpdateWaveText(int wave)
    {
        if (waveText != null)
            waveText.text = "Wave " + wave;
    }

    public void AddScore(int amount)
    {
        score += amount;
    }

    public void AddWave()
    {
        wave++;
        UpdateWaveText(wave);
    }
}
