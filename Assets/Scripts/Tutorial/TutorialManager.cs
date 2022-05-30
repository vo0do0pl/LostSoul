using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    //TODO5 write actual texts

    [SerializeField] List<GameObject> dialogBoxes;

    public static event Action StopPlayerFromMoving;
    public static event Action LetPlayerMove;

    int currentDialogIndex = 0;
    bool playerPassedTutorial = false;

    void Start()
    {
        StopPlayerFromMoving?.Invoke();
    }

    void Update()
    {
        if (playerPassedTutorial) return;

        if (Input.anyKeyDown)
        {
            OnPlayerInput();
        }
    }

    private void OnPlayerInput()
    {
        ++currentDialogIndex;
        if (currentDialogIndex >= dialogBoxes.Count)
        {
            LetPlayerMove?.Invoke();
            playerPassedTutorial = true;
            dialogBoxes[currentDialogIndex - 1].gameObject.SetActive(false);
            return;
        }

        if(dialogBoxes[currentDialogIndex - 1] != null)
        {
            dialogBoxes[currentDialogIndex - 1].gameObject.SetActive(false);
        }

        dialogBoxes[currentDialogIndex].gameObject.SetActive(true);
    }
}
