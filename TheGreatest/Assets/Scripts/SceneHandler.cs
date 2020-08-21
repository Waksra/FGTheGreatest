﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public static SceneHandler SharedInstance;
    private int sceneIndex;

    private void Awake()
    {
        SharedInstance = this;
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    public void SwitchScene()
    {
        switch (sceneIndex)
        {
            case 0: // StartScene
                SceneManager.LoadScene(1);
                break;

            case 1: // MainScene
                SceneManager.LoadScene(2);
                break;

            case 2: // EndScene
                SceneManager.LoadScene(1);
                break;
            default:
                SceneManager.LoadScene(0);
                break;
        }
    }
}
