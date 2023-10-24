using UnityEngine;
using UnityEngine.Tilemaps;

public class ColorAdapter : MonoBehaviour
{
    private Tilemap tilemap;

    // Start is called before the first frame update
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {
        var levelColorController = ServiceLocator.LevelColorController;

        if (!levelColorController.IsColorBlinded)
        {
            //tilemap.color = levelColorController.GetTileColorRGB(levelColorController.CurrentColor);
        }
        else
        {
            tilemap.color = levelColorController.ColorBlindColorRGB;
        }
    }
}
