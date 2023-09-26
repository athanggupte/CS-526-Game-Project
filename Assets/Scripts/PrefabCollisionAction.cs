using UnityEngine;
using UnityEngine.SceneManagement;

public class PrefabCollisionAction : MonoBehaviour
{
    [SerializeField]
    private int sceneIndexToLoad = 1;  // Default to scene index 1; adjust this in the Inspector

    // Detects trigger collision with another GameObject
    void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("OOO");
        // Check if the colliding object has the "Player" tag
        if (collider.gameObject.CompareTag("Player"))
        {
            Debug.Log("o");
            ExecuteAction();
        }
    }

    // Define the action to be executed when the player collides with the prefab
    void ExecuteAction()
    {
        // Change to the desired scene by its index
        SceneManager.LoadScene(sceneIndexToLoad);
        Debug.Log("ADS");
    }
}
