using UnityEngine;

public class MSspawner : MonoBehaviour
{
    public GameObject ship;
    private GameObject currentShip;
    public float waitTime = 8f;
    private float timer = 0;
    private int originalWaveNum = 1;
    private int currentWaveNum;
    private int shipNum = 0;

    void Update()
    {
        currentWaveNum = Manager.wave;

        if (currentWaveNum != originalWaveNum)
        {
            shipNum = 0;
        }

        originalWaveNum = currentWaveNum;

        if (currentShip == null && shipNum < 2)
        {
            timer += Time.deltaTime;

            if (timer > waitTime)
            {
                SpawnShip();
                timer = 0;
                shipNum++;
            }
        }
    }

    void SpawnShip()
    {
        currentShip = Instantiate(this.ship, new Vector3(-17f, 11f, 0f), Quaternion.identity, this.transform);
    }
}
