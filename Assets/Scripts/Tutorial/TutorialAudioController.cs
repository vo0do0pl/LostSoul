using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialAudioController : MonoBehaviour
{
    [SerializeField] AudioClip dieClip;
    public AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        TutorialCosmicManController.PlayerDied += OnPlayerDied;
    }

    private void OnDestroy()
    {
        TutorialCosmicManController.PlayerDied -= OnPlayerDied;
    }

    private void OnPlayerDied()
    {
        audioSource.Stop();

        audioSource.clip = dieClip;

        audioSource.Play();
    }
}
