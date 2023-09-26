using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelHUDController : MonoBehaviour
{
    void Start()
    {
        var textComp = GetComponent<TextMeshProUGUI>();
        textComp.text = "Level " + LevelManager.CurrentLevel;
    }

}
