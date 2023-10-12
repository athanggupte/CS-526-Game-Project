using UnityEngine;
using Proyecto26;

public class DataCollector : MonoBehaviour
{
    private static DataCollector s_instance;

    public static DataCollector Instance
    {
        get {
            if (!s_instance)
                s_instance = new DataCollector();
            return s_instance;
        }
    }

    private const string firebaseURL = "https://hue-hustlers-default-rtdb.firebaseio.com/";
    private int[] colorSwitchCounts = new int[3] { 0, 0, 0 };

    public void CollectColorSwitch(LevelColor color)
    {
        colorSwitchCounts[(int)color]++;
    }

    public void SendColorSwitchCountsToFirebase()
    {
        ColorSwitchCountsData data = new ColorSwitchCountsData
        {
            Red = colorSwitchCounts[(int)LevelColor.Red],
            Blue = colorSwitchCounts[(int)LevelColor.Blue],
            Yellow = colorSwitchCounts[(int)LevelColor.Yellow]
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
