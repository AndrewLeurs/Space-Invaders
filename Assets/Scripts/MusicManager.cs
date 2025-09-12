using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public AudioClip musicClip;
    public float titleVolume = 1f;
    public float gameVolume = 0.6f;

    private AudioSource audioSource;

    private static MusicManager instance;

    void Awake()
    {
        // Singleton pattern: only one MusicManager ever
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = musicClip;
        audioSource.loop = true;
        audioSource.volume = titleVolume;
        audioSource.Play();

        // Listen to scene changes
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Adjust volume depending on scene
        if (scene.name == "Title Screen")
        {
            audioSource.volume = titleVolume;
        }
        else
        {
            audioSource.volume = gameVolume;
        }
    }
}
