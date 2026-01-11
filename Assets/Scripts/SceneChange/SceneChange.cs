using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    [SerializeField] private string gameplaySceneName = "MainLevel";

    public void StartGame()
    {

        SceneManager.LoadScene(gameplaySceneName);
    }


    public void NewGame()
    {


        SceneManager.LoadScene(gameplaySceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
