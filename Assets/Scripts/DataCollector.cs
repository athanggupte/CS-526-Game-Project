using UnityEngine;
using UnityEngine.UI;
using Proyecto26;
using System.Collections.Generic;

public class DataCollector : MonoBehaviour
{
    private const string firebaseURL = "https://hue-hustlers-default-rtdb.firebaseio.com/";
    private Dictionary<LevelColorController.Level, int> colorSwitchCounts = new Dictionary<LevelColorController.Level, int>();

    private void Start()
    {
    }

    public void CollectColorSwitch(LevelColorController.Level color)
    {
        if (colorSwitchCounts.ContainsKey(color))
        {
            colorSwitchCounts[color]++;
        }
        else
        {
            colorSwitchCounts[color] = 1;
        }
    }

    public void SendColorSwitchCountsToFirebase()
    {
        ColorSwitchCountsData data = new ColorSwitchCountsData
        {
            Red = colorSwitchCounts.ContainsKey(LevelColorController.Level.Red) ? colorSwitchCounts[LevelColorController.Level.Red] : 0,
            Blue = colorSwitchCounts.ContainsKey(LevelColorController.Level.Blue) ? colorSwitchCounts[LevelColorController.Level.Blue] : 0,
            Yellow = colorSwitchCounts.ContainsKey(LevelColorController.Level.Yellow) ? colorSwitchCounts[LevelColorController.Level.Yellow] : 0
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
        colorSwitchCounts.Clear();
    }

    [System.Serializable]
    private class ColorSwitchCountsData
    {
        public int Red;
        public int Blue;
        public int Yellow;
    }

}
