using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class CanvasManager : MonoBehaviour
{
    public GameObject start, game, end;
    public FirstPersonController moveScript;
    public CinemachineVirtualCamera startCam, playerCam;
    public GameObject startBttn, tutorial;

    public void swap()
    {
        if (start.activeSelf)
        {
            start.SetActive(false);
            moveScript.enabled = true;
            startCam.Priority = 7;
            game.SetActive(true);
        } else if (game.activeSelf)
        {
            game.SetActive(false);
            moveScript.enabled = false;
            playerCam.Priority = 8;
            end.SetActive(true);
        }
    }

    public void closeTut()
    {
        tutorial.SetActive(false);
        startBttn.SetActive(true);
    }
}
