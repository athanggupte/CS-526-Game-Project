using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject levelMenu;
    public GameObject ControlsMenu;// Assign the level menu GameObject in the Inspector

    public void Start()
    {
        mainMenu.SetActive(true);
        levelMenu.SetActive(false);
        ControlsMenu.SetActive(false);
    }

    // Call this function when the "Play Game" button is clicked
    public void ShowLevelMenu()
    {
        levelMenu.SetActive(true);
        mainMenu.SetActive(false);
        ControlsMenu.SetActive(false);
    }

    public void ShowControlsMenu()
    {
        levelMenu.SetActive(false);
        mainMenu.SetActive(false);
        ControlsMenu.SetActive(true);
    }

    public void NextScene()
    {
        SceneManager.LoadScene(2);

    }
}
