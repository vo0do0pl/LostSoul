using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavenAudioController : MonoBehaviour
{
    [SerializeField] List<AudioClip> clips;
    [SerializeField] AudioClip dieClip;
    public AudioSource audioSource;

    int lastAudioIndex = -1;
    bool gameOver = false;
    // Start is called before the first frame update
    void Start()
    {
        PlayerController.PlayerStarted += OnPlayerStarted;
        DieColliderController.PlayerDied += OnPlayerDied;
    }

    private void OnDestroy()
    {
        PlayerController.PlayerStarted -= OnPlayerStarted;
        DieColliderController.PlayerDied -= OnPlayerDied;
    }

    private void OnPlayerDied()
    {
        audioSource.Stop();
        audioSource.clip = dieClip;
        audioSource.Play();
        audioSource.loop = true;

        gameOver = true;
    }

    private void OnPlayerStarted()
    {
        audioSource.Stop();

        int nextIndex = UnityEngine.Random.Range(0, clips.Count);
        while(nextIndex == lastAudioIndex)
        {
            nextIndex = UnityEngine.Random.Range(0, clips.Count);
        }

        lastAudioIndex = nextIndex;
        audioSource.clip = clips[lastAudioIndex];
        
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (!audioSource.isPlaying && !gameOver)
            OnPlayerStarted();
    }
}
