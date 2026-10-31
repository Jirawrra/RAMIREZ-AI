using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    public int mainMenuSceneIndex = 0;
    public int gameSceneIndex = 1;
    public GameObject gameOverUI;
    public void StartGame()
    {
        SceneManager.LoadScene(gameSceneIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(mainMenuSceneIndex);
    }

    public void resetGame()
    {
        SceneManager.LoadScene(gameSceneIndex);
    }



    public void gameOver()
    {
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }
    }
}
