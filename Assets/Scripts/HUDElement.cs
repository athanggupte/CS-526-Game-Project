using UnityEngine;
using UnityEngine.SceneManagement;

public class HUDElement : MonoBehaviour
{
    public void RestartGame()
    {
        LevelEvents.Instance.LevelEnd.Invoke(LevelEndCondition.LevelRestarted);
        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        LevelEvents.Instance.LevelEnd.Invoke(LevelEndCondition.GameQuit);
        UIController.currentMenu = UIController.MenuScreen.Main;
        SceneManager.LoadScene(0);
    }
}
