using UnityEngine;
using UnityEngine.SceneManagement;

public class HUDElement : MonoBehaviour
{
    public void RestartGame()
    {
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        UIController.currentMenu = UIController.MenuScreen.Main;
        SceneManager.LoadScene(0);
    }
}
