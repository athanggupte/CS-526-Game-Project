using UnityEngine;
using UnityEngine.SceneManagement;

public class HudOverlay : MonoBehaviour
{
    [SerializeField] private int hudSceneIndex = 1; // Assuming HUD scene is at index 1 in Build Settings
    [SerializeField] private string levelName;

    void Start()
    {
        LevelManager.LevelName = levelName;

        // Check if the HUD scene is already loaded (optional but can be efficient)
        if (!SceneManager.GetSceneByBuildIndex(hudSceneIndex).isLoaded)
        {
            // Load the HUD scene additively using its index
            SceneManager.LoadScene(hudSceneIndex, LoadSceneMode.Additive);
        }
    }
}
