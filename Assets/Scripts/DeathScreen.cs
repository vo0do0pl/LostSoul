using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathScreen : MonoBehaviour
{
    [SerializeField] Image deathBackground;
    [SerializeField] Text deathText;
    [SerializeField] Button heavenButton;
    [SerializeField] Button playAgainButton;
    [SerializeField] Button quitGameButton;
    [SerializeField] float colorChangeColorStep = 0.1f;
    [SerializeField] float colorChangeTimerStep = 0.1f;

    float lastColorChange = 9999999f;
    bool animationEnded = false;

    // Start is called before the first frame update
    void Start()
    {
        TutorialCosmicManController.PlayerDied += OnPlayerDied;
        DieColliderController.PlayerDied += OnPlayerDied;
    }

    private void OnDestroy()
    {
        TutorialCosmicManController.PlayerDied -= OnPlayerDied;
        DieColliderController.PlayerDied -= OnPlayerDied;
    }

    private void OnPlayerDied()
    {
        deathBackground.gameObject.SetActive(true);
        lastColorChange = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (!animationEnded && lastColorChange + colorChangeTimerStep <= Time.time )
        {
            ChangeTheColor();
        }
    }

    private void ChangeTheColor()
    {
        Color cachedColor = deathBackground.color;
        cachedColor.a += colorChangeColorStep;

        deathBackground.color = cachedColor;
        lastColorChange = Time.time;

        if (deathBackground.color.a >= 1)
        {
            deathText.gameObject.SetActive(true);
            animationEnded = true;

            Invoke("ShowButton", 1.5f);
        }
    }

    private void ShowButton()
    {
        heavenButton?.gameObject.SetActive(true);

        playAgainButton?.gameObject.SetActive(true);

        if (Application.platform != RuntimePlatform.WindowsPlayer && Application.platform != RuntimePlatform.WindowsEditor)
        {
            RectTransform rect = playAgainButton.GetComponent<RectTransform>();

            var position = rect.anchoredPosition;
            position.x = 0f;
            rect.anchoredPosition = position;
        }
        else
            quitGameButton?.gameObject.SetActive(true);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Heaven");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
