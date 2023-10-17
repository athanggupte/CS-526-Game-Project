using TMPro;
using UnityEngine;

public class LevelHUDController : MonoBehaviour
{
    void Start()
    {
        var textComp = GetComponentInChildren<TextMeshProUGUI>();
        textComp.text = "Level " + LevelManager.CurrentLevel;
    }

}
