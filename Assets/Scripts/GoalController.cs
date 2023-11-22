using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Goal Triggered");

        if (collision.CompareTag("Player"))
        {
            UIController.currentMenu = UIController.MenuScreen.LevelEnd;
            SceneManager.LoadScene(0);
            LevelEvents.Instance.LevelEnd.Invoke(LevelEndCondition.GoalReached);
        }
    }
}




