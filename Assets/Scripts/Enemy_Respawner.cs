using Unity.VisualScripting;
using UnityEngine;

public class Enemy_Respawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] respawnPoints;
    [SerializeField] private float respawnTime = 2f;
    [SerializeField] private float cooldownDecreaseRate = .05f;
    [SerializeField] private float cooldownCap = .7f;
    private Transform player;
    private float timer;

    private void Awake()
    {
        player = FindFirstObjectByType<Entity>().transform;
    }


    // Update is called once per frame
    private void Update()
    {
        timer -= Time.deltaTime; // Decrease timer by the time elapsed since last frame
        if (timer < 0)           // When timer reaches zero or below reset it and spawn a new enemy
        {
            timer = respawnTime;
            CreateNewEnemy();

            respawnTime = Mathf.Max(cooldownCap, respawnTime - cooldownDecreaseRate); 
            // Reduce respawn time but not below the cap to increase difficulty over time
        }
    }

    private void CreateNewEnemy()
    {
        int respawnPointIndex = Random.Range(0, respawnPoints.Length);
        Vector3 spawnPosition = respawnPoints[respawnPointIndex].position;
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        

        bool createdOnTheRight = newEnemy.transform.position.x > player.position.x;

        if (createdOnTheRight)
            newEnemy.GetComponent<Enemy>().Flip();

    }
}
