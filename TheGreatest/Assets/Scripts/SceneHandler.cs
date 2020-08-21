using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    private int sceneIndex;

    private void Awake()
    {
        sceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    private void SwitchScene()
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
        }
    }
}
