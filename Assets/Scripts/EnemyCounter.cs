using UnityEngine;

public class EnemyCounter : MonoBehaviour
{
    public int enemiesAlive { get; private set; }
    public bool AllDead { get; private set; }// Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemiesAlive= FindObjectsByType<ZombieController>(FindObjectsSortMode.None).Length;
    }

    // Update is called once per frame
    public void enemiesKilled()
    {
        enemiesAlive--;

        if (enemiesAlive <= 0)
            AllDead = true;
    }

}
