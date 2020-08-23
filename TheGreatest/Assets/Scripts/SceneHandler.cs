using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
    public static SceneHandler SharedInstance;

    [SerializeField] private float sceneSwitchTimer;
    private int _sceneIndex;

    private void Awake()
    {
        SharedInstance = this;
        _sceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    public void SwitchScene()
    {
        switch (_sceneIndex)
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

    public void StartSceneSwitch()
    {
        StartCoroutine(WaitForSceneSwitch());
    }

    public IEnumerator WaitForSceneSwitch()
    {
        yield return new WaitForSeconds(sceneSwitchTimer);
        SwitchScene();
    }
}
