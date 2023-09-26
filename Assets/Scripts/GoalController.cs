using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalController : MonoBehaviour
{
    public string nextScene;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            UIController.currentMenu = UIController.MenuScreen.LevelEnd;
            SceneManager.LoadScene(nextScene);
        }
    }
}
