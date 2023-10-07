using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    static int currentLevel = 0;
    static int nextLevel = 1;

    public static int CurrentLevel
    {
        get => currentLevel;
        set
        {
            currentLevel = value;
            nextLevel = value + 1;
        }
    }

    public static int NextLevel
    {
        get => nextLevel;
    }

    private static int baseLevelBuildIndex = 2;

    public static void LoadLevel(int level)
    {
        CurrentLevel = level;
        SceneManager.LoadScene(CurrentLevel + baseLevelBuildIndex);
    }

    void Start()
    {
        SceneManager.LoadScene("Overlay-LevelHUD", LoadSceneMode.Additive);
    }

}
