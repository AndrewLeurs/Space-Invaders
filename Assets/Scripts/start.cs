using UnityEngine;
using UnityEngine.SceneManagement;

public class start : MonoBehaviour
{
    public string LevelName;
    private float waitTime = 1.0f;
    private float timer = 0;

    public void LoadLevel()
    {
        Manager.score = 0;
        Manager.lives = 3;
        Manager.wave = 1;
        SceneManager.LoadScene(LevelName);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) && timer >= waitTime)
        {
            LoadLevel();
        }
    }
}
