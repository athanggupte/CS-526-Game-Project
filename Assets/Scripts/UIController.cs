using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject levelMenu;
    public GameObject controlsMenu;
    public GameObject levelEndMenu;

    public GameObject nextLevelButton;

    public enum MenuScreen
    {
        Main,
        Levels,
        Controls,
        LevelEnd
    };

    public static MenuScreen currentMenu = MenuScreen.Main;

    public void Start()
    {
        SetMenu();
    }

    public void ShowMainMenu()
    {
        SetMenu(MenuScreen.Main);
    }

    public void ShowLevelMenu()
    {
        SetMenu(MenuScreen.Levels);
    }

    public void ShowControlsMenu()
    {
        SetMenu(MenuScreen.Controls);
    }

    public void ShowLevelEndMenu()
    {
        SetMenu(MenuScreen.LevelEnd);
    }

    public void SetMenu(MenuScreen menu)
    {
        currentMenu = menu;
        SetMenu();
    }

    private void SetMenu()
    {
        mainMenu.SetActive(currentMenu == MenuScreen.Main);
        levelMenu.SetActive(currentMenu == MenuScreen.Levels);
        controlsMenu.SetActive(currentMenu == MenuScreen.Controls);
        levelEndMenu.SetActive(currentMenu == MenuScreen.LevelEnd);

        if (LevelManager.CurrentLevel == 5) // TODO: change when more levels added
        {
            nextLevelButton.SetActive(false);
        }
    }

    public void StartLevel(int level)
    {
        LevelManager.LoadLevel(level);
    }

    public void StartNextLevel()
    {
        LevelManager.LoadLevel(LevelManager.NextLevel);
    }
}
