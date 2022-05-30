using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject demon;
    [SerializeField] float demonSpawnStart;
    [SerializeField] float demonSpawnTimer;
    [SerializeField] float demonSpawnXPointRange = 7.3f;

    bool gameStarted = false;
    float gameStartedAt;
    float lastDemonSpawned = 0f;
    // Start is called before the first frame update
    void Start()
    {
        PlayerController.PlayerStarted += OnPlayerStarted;
        DieColliderController.PlayerDied += OnPlayerDied;
    }

    private void OnPlayerDied()
    {
        gameStarted = false;
    }

    private void OnDestroy()
    {
        PlayerController.PlayerStarted -= OnPlayerStarted;
        DieColliderController.PlayerDied -= OnPlayerDied;
    }

    private void OnPlayerStarted()
    {
        gameStarted = true;
        gameStartedAt = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameStarted)
        {
            if(gameStartedAt + demonSpawnStart <= Time.time)
            {
                if(lastDemonSpawned + demonSpawnTimer <= Time.time)
                {
                    lastDemonSpawned = Time.time;
                    SpawnDemon();
                }
            }
        }
    }

    private void SpawnDemon()
    {
        Vector3 demonPosition = new Vector3(0, 0, -5f);
        demonPosition.x = UnityEngine.Random.Range(-demonSpawnXPointRange, demonSpawnXPointRange + 0.01f);
        demonPosition.y = transform.position.y;

        Instantiate(demon,
            demonPosition,
            Quaternion.identity);
    }
}
