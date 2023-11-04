using UnityEngine;
using UnityEngine.Tilemaps;
public enum LevelColor
{
    Red,
    Yellow,
    Blue
};

public class LevelColorController : MonoBehaviour
{
    /**
     * <summary>Returns the currently active color of the level</summary>
     */
    public LevelColor CurrentColor { get => m_currentColor; private set => m_currentColor = value; }

    /**
     * <summary>List of Tilemap layers in the level corresponding to the level colors</summary>
     */
    public GameObject[] Layers { get => m_layers; private set => m_layers = value; }

    /**
     * <summary>List of Tiles corresponding to the level colors</summary>
     */
    public Tile[] Tiles { get => m_tiles; private set => m_tiles = value; }

    /**
     * <summary>Returns if the color blindness effect is turned on</summary>
     */
    public bool IsColorBlinded { get; private set; }

    public Color ColorBlindColorRGB = new Color(0.8f, 0.8f, 0.8f);

    enum TileType
    {
        Filled   = 0,
        Unfilled = 1
    }

    void Awake()
    {
        gameObject.AddComponent<LevelEvents>();
        
        LevelEvents.Instance.ColorSwitch.AddListener(SwitchColor);
        LevelEvents.Instance.ColorBombDetonate.AddListener(ColorBomb);
        LevelEvents.Instance.ColorBlindBegin.AddListener(BeginColorBlind);
        LevelEvents.Instance.ColorBlindEnd.AddListener(EndColorBlind);
        
        ServiceLocator.LevelColorController = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        IsColorBlinded = false;

        LevelEvents.LevelEventsInitialized.Invoke();

        LevelEvents.Instance.ColorSwitch.Invoke(m_currentColor);

        // Convert all layers to use the Tilemaps sorting layer for backward compatbility
        foreach(GameObject layer in m_layers)
        {
            layer.GetComponent<TilemapRenderer>().sortingLayerName = "Tilemaps";
        }
    }

    void SwitchColor(LevelColor color)
    {
        m_currentColor = color;

        for (int i = 0; i < m_layers.Length; i++)
        {
            bool layerEnabled = (i == (int)m_currentColor);

            m_layers[i].GetComponent<TilemapCollider2D>().enabled = layerEnabled;

            // Sorting Order Priority is ascending (1 is on top of 0)
            m_layers[i].GetComponent<TilemapRenderer>().sortingOrder = layerEnabled ? 1 : 0;

            Vector3 tmpPos = m_layers[i].transform.position;
            m_layers[i].transform.position.Set(tmpPos.x, tmpPos.y, layerEnabled ? 1 : 0);

            if (!IsColorBlinded)
            {
                Color tmpColor = m_layers[i].GetComponent<Tilemap>().color;
                tmpColor.a = (i == (int)color) ? 1.0f : 0.2f;
                m_layers[i].GetComponent<Tilemap>().color = tmpColor;
            }
        }
    }

    void ColorBomb(LevelColor targetColor, Vector3 position, float radius)
    {
        Tilemap currentTilemap = GetLayer(CurrentColor).GetComponent<Tilemap>();
        Tilemap targetTilemap = GetLayer(targetColor).GetComponent<Tilemap>();

        var cellPos = currentTilemap.WorldToCell(position);

        for (int x = -(int)radius; x < radius + 1; x++)
        {
            for (int y = -(int)radius; y < radius + 1; y++)
            {
                var cell_pos = new Vector3Int(cellPos.x + x, cellPos.y + y, cellPos.z);
                // Debug.Log("Setting tile (" + cell_pos.x + ", " + cellPos.y + ") to NULL");

                var currentTile = currentTilemap.GetTile(cell_pos);
                currentTilemap.SetTile(cell_pos, null);

                if (currentTile)
                {
                    targetTilemap.SetTile(cell_pos, Tiles[(int)TileType.Filled]);
                }
            }
        }
    }

    void BeginColorBlind()
    {
        IsColorBlinded = true;

        for (int i = 0; i < m_layers.Length; i++)
        {
            m_layers[i].GetComponent<Tilemap>().color = ColorBlindColorRGB;
        }
    }

    void EndColorBlind()
    {
        for (int i = 0; i < m_layers.Length; i++)
        {
            Color tmpColor = m_tileColors[i];
            tmpColor.a = (i == (int)m_currentColor) ? 1.0f : 0.2f;
            m_layers[i].GetComponent<Tilemap>().color = tmpColor;
        }

        IsColorBlinded = false;
    }

    public GameObject GetLayer(LevelColor color)
    {
        return m_layers[(int)color];
    }

    public Color GetTileColorRGB(LevelColor color)
    {
        return m_tileColors[(int)color];
    }
    
    [SerializeField] private LevelColor m_currentColor = LevelColor.Red;
    [SerializeField] private GameObject[] m_layers;
    [SerializeField] private Tile[] m_tiles;
    [SerializeField] private Color[] m_tileColors;
}
