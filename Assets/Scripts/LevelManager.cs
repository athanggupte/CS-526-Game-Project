using System.Collections;
using System.Collections.Generic;
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

    void Start()
    {
        SceneManager.LoadScene("Overlay-LevelHUD", LoadSceneMode.Additive);
    }

}
