using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavenGameModeManager : MonoBehaviour
{
    //TODO1: add Ethans demon
    //TODO2: add power up for speed and jump (clean vector icons) add wings to icons too

    //Clouds Responsibility
    #region Flow
    [SerializeField] Camera mainCamera;
    [SerializeField] GameObject dieCollider;
    [SerializeField] float movementStep = 0.2f;
    [SerializeField] GameObject leftSideBorder;
    [SerializeField] GameObject rightSideBorder;
    [SerializeField] GameObject EnemySpawner;

    [SerializeField] float speedOverTimeModifier = 0.001f;
    [SerializeField] float speedOverTimeTimer = 0.25f;

    bool gameStarted = false;
    float lastSpeedChangeAt = 9999999f;
    #endregion

    #region Clouds
    [SerializeField] List<GameObject> borderClouds;
    [SerializeField] float borderCloudHeightHalved = 1.25f;
    private float lastBorderCloudYPositionOnLeft = 8f;
    private float lastBorderCloudYPositionOnRight = 8f;

    [SerializeField] List<GameObject> platformClouds;
    [SerializeField] float platformMaxDistance = 2.8f;
    [SerializeField] float platformMinDistance = 1.85f;
    [SerializeField] float platformSpawnPointRange = 7.3f;

    private float lastPlatformCloudYPosition = 11.82f;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        PlayerController.PlayerStarted += OnPlayerStarted;
        DieColliderController.PlayerDied += OnPlayerDied;
    }

    private void Update()
    {
        if (gameStarted)
        {
            if(lastSpeedChangeAt + speedOverTimeTimer <= Time.time)
            {
                lastSpeedChangeAt = Time.time;
                movementStep += speedOverTimeModifier;
            }
        }
    }

    void FixedUpdate()
    {
        if (gameStarted)
        {
            Vector3 nextPosition = mainCamera.transform.position;
            nextPosition.y += movementStep;

            mainCamera.transform.position = Vector3.Lerp(nextPosition, mainCamera.transform.position, Time.deltaTime);

            nextPosition = dieCollider.transform.position;
            nextPosition.y += movementStep;

            dieCollider.transform.position = Vector3.Lerp(nextPosition, dieCollider.transform.position, Time.deltaTime);

            nextPosition = leftSideBorder.transform.position;
            nextPosition.y += movementStep;

            leftSideBorder.transform.position = Vector3.Lerp(nextPosition, leftSideBorder.transform.position, Time.deltaTime);

            nextPosition = rightSideBorder.transform.position;
            nextPosition.y += movementStep;

            rightSideBorder.transform.position = Vector3.Lerp(nextPosition, rightSideBorder.transform.position, Time.deltaTime);

            nextPosition = EnemySpawner.transform.position;
            nextPosition.y += movementStep;

            EnemySpawner.transform.position = Vector3.Lerp(nextPosition, EnemySpawner.transform.position, Time.deltaTime);
        }
    }

    private void OnDestroy()
    {
        PlayerController.PlayerStarted -= OnPlayerStarted;
        DieColliderController.PlayerDied -= OnPlayerDied;
        BorderCloudController.BorderCloudVanished -= OnBorderCloudVanished;
        PlatformCloudController.PlatformCloudVanished -= OnPlatformCloudVanished;
    }

    private void OnPlayerStarted()
    {
        BorderCloudController.BorderCloudVanished += OnBorderCloudVanished;
        PlatformCloudController.PlatformCloudVanished += OnPlatformCloudVanished;
        gameStarted = true;
        lastSpeedChangeAt = Time.time;
    }

    private void OnPlatformCloudVanished()
    {
        Vector3 platformPostion = new Vector3(0, 0, -5f);
        platformPostion.x = UnityEngine.Random.Range(-platformSpawnPointRange, platformSpawnPointRange + 0.01f);
        platformPostion.y = UnityEngine.Random.Range(platformMinDistance, platformMaxDistance + 0.01f) + lastPlatformCloudYPosition;

        lastPlatformCloudYPosition = platformPostion.y;

        Instantiate(platformClouds[UnityEngine.Random.Range(0, platformClouds.Count)],
            platformPostion,
            Quaternion.identity);
    }

    private void OnPlayerDied()
    {
        gameStarted = false;
        BorderCloudController.BorderCloudVanished -= OnBorderCloudVanished;
        PlatformCloudController.PlatformCloudVanished -= OnPlatformCloudVanished;
    }

    private void OnBorderCloudVanished(float positionX)
    {
        int cloudIndex = UnityEngine.Random.Range(0, borderClouds.Count);
        bool flip = Convert.ToBoolean(UnityEngine.Random.Range(0, 2));

        float nextPosition;
        if (positionX > 0)
        {
            lastBorderCloudYPositionOnRight += borderCloudHeightHalved * 2;
            nextPosition = lastBorderCloudYPositionOnRight;
        }
        else
        {
            lastBorderCloudYPositionOnLeft += borderCloudHeightHalved * 2;
            nextPosition = lastBorderCloudYPositionOnLeft;
        }

        Instantiate(borderClouds[cloudIndex],
            new Vector3(positionX, nextPosition, -5),
            flip ? Quaternion.Euler(0f, 0f, 180f) : Quaternion.identity);
    }
}
