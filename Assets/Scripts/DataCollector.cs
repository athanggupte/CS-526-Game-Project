using UnityEngine;
using UnityEngine.UI;
using Proyecto26;
using System.Collections.Generic;

public class DataCollector : MonoBehaviour
{
    private const string firebaseURL = "https://hue-hustlers-default-rtdb.firebaseio.com/";
    private int[] colorSwitchCounts;

    private void Start()
    {
        colorSwitchCounts = new int[3] { 0, 0, 0 };
    }

    public void CollectColorSwitch(LevelColorController.Level color)
    {
        colorSwitchCounts[(int)color]++;
    }

    public void SendColorSwitchCountsToFirebase()
    {
        ColorSwitchCountsData data = new ColorSwitchCountsData
        {
            Red = colorSwitchCounts[(int)LevelColorController.Level.Red],
            Blue = colorSwitchCounts[(int)LevelColorController.Level.Blue],
            Yellow = colorSwitchCounts[(int)LevelColorController.Level.Yellow]
        };
        string jsonData = JsonUtility.ToJson(data);

        RestClient.Post(firebaseURL + "colorSwitchCounts.json", jsonData).Then(response =>
        {
            Debug.Log("Successfully sent color switch counts to Firebase");
        }).Catch(error =>
        {
            Debug.LogError("Error sending color switch counts to Firebase: " + error.Message);
        });
    }

    public void ResetColorSwitchCounts()
    {
        for (int i = 0; i < colorSwitchCounts.Length; i++)
        {
            colorSwitchCounts[i] = 0;
        }
    }

    [System.Serializable]
    private class ColorSwitchCountsData
    {
        public int Red;
        public int Blue;
        public int Yellow;
    }

}
