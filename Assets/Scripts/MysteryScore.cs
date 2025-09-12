using UnityEngine;

public class MysteryScript : MonoBehaviour
{
    public float lifetime = 2f;
    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }
}
