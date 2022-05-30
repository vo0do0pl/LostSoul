using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    [SerializeField] List<GameObject> projectiles;
    [SerializeField] GameObject player;
    [SerializeField] float projectileSpawnDelay = 0.1f;

    float lastProjectileSpawnedAt = 0f;

    void Start()
    {
        TutorialCosmicManController.PlayerMoved += OnLetPlayerMove;
    }

    private void OnLetPlayerMove()
    {
        if (lastProjectileSpawnedAt + projectileSpawnDelay >= Time.time) return;

        int projectileIndex = Random.Range(0, projectiles.Count - 1);
        var projectile = Instantiate(projectiles[projectileIndex], transform.position, Quaternion.identity);
        projectile.GetComponent<TutorialProjectile>().SetTargetAndLaunch(player.gameObject.transform.position);

        lastProjectileSpawnedAt = Time.time;
    }
}
