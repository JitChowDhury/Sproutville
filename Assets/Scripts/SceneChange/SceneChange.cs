using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    [SerializeField] private string gameplaySceneName = "Farm"; // change if needed

    public void StartGame()
    {
        SceneManager.LoadScene("MainLevel");
    }


    public void QuitGame()
    {
        Application.Quit();
    }
}
