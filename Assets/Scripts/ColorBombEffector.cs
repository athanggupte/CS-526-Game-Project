using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;

public class ColorBombEffector : MonoBehaviour
{
    public LevelColorController.Level color;
    public int radius;
    public LevelColorController colorController;
    public float timeToExplode;

    public Tile[] tiles;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Deploy()
    {
        StartCoroutine(BlowUp());
    }

    void BlowUpTiles()
    {
        Assert.IsNotNull(colorController);

        Tilemap currentTilemap = colorController.GetLayer(colorController.level).GetComponent<Tilemap>();
        Tilemap targetTilemap = colorController.GetLayer(color).GetComponent<Tilemap>();

        var cellPos = currentTilemap.WorldToCell(transform.position);

        for (int x = -radius; x < radius + 1; x++)
        {
            for (int y = -radius; y < radius + 1; y++)
            {
                var cell_pos = new Vector3Int(cellPos.x + x, cellPos.y + y, cellPos.z);
                Debug.Log("Setting tile (" + cell_pos.x + ", " + cellPos.y + ") to NULL");
                
                var currentTile = currentTilemap.GetTile(cell_pos);
                currentTilemap.SetTile(cell_pos, null);

                if (currentTile)
                {
                    targetTilemap.SetTile(cell_pos, tiles[(int)color]);
                }
            }
        }
    }

    IEnumerator BlowUp()
    {
        yield return new WaitForSeconds(timeToExplode);
        BlowUpTiles();
        Destroy(gameObject);
    }
}
