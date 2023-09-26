using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject levelMenu;
    public GameObject ControlsMenu;
    public GameObject levelendMenu;



    public void Start()
    {
        mainMenu.SetActive(true);
        levelMenu.SetActive(false);
        ControlsMenu.SetActive(false);
        levelendMenu.SetActive(false);
    }

    // Call this function when the "Play Game" button is clicked
    public void ShowLevelMenu()
    {
        levelMenu.SetActive(true);
        mainMenu.SetActive(false);
        ControlsMenu.SetActive(false);
        levelendMenu.SetActive(false);

    }

    public void ShowControlsMenu()
    {
        levelMenu.SetActive(false);
        mainMenu.SetActive(false);
        ControlsMenu.SetActive(true);
        levelendMenu.SetActive(false);

    }

    public void ShowLevelEndMenu()
    {
        levelMenu.SetActive(false);
        mainMenu.SetActive(false);
        ControlsMenu.SetActive(false);
        levelendMenu.SetActive(true);

    }

    public void NextScene()
    {
        SceneManager.LoadScene(2);

    }
}
