using System;
using UnityEngine;
using UnityEngine.Assertions;
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

    /**
     * <summary>Returns if the star effect is turned on</summary>
     */
    public bool IsStarActivated { get; private set; }

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
        LevelEvents.Instance.ColorGunHit.AddListener(ColorGun);
        LevelEvents.Instance.ColorBlindBegin.AddListener(BeginColorBlind);
        LevelEvents.Instance.ColorBlindEnd.AddListener(EndColorBlind);
        LevelEvents.Instance.StarCollect.AddListener(ActivateStar);
        
        ServiceLocator.LevelColorController = this;

        m_layerMaskGround = LayerMask.NameToLayer("Ground Layer");
        m_layerMaskInactive = LayerMask.NameToLayer("Inactive");
    }

    // Start is called before the first frame update
    void Start()
    {
        IsColorBlinded = false;

        LevelEvents.LevelEventsInitialized.Invoke();

        m_tilemaps = new Tilemap[m_layers.Length];
        int i = 0;
        foreach(GameObject layer in m_layers)
        {
            layer.GetComponent<TilemapCollider2D>().enabled = true;
            layer.GetComponent<TilemapRenderer>().sortingLayerName = "Tilemaps";
            m_tilemaps[i++] = layer.GetComponent<Tilemap>();
        }

        var playerGO = GameObject.FindGameObjectWithTag("Player");
        Assert.IsNotNull(playerGO);
        m_weaponController = playerGO.GetComponent<WeaponController>();
        Assert.IsNotNull(m_weaponController);

        LevelEvents.Instance.ColorSwitch.Invoke(m_currentColor);
    }

    void SwitchColor(LevelColor color)
    {
        m_currentColor = color;

        for (int i = 0; i < m_layers.Length; i++)
        {
            bool layerEnabled = (i == (int)m_currentColor);

            //m_layers[i].GetComponent<TilemapCollider2D>().enabled = layerEnabled;
            m_layers[i].layer = layerEnabled ? m_layerMaskGround : m_layerMaskInactive;

            // Sorting Order Priority is ascending (1 is on top of 0)
            m_layers[i].GetComponent<TilemapRenderer>().sortingOrder = layerEnabled ? 1 : 0;

            //Vector3 tmpPos = m_layers[i].transform.position;
            //m_layers[i].transform.position.Set(tmpPos.x, tmpPos.y, layerEnabled ? 1 : 0);

            if (!IsColorBlinded)
            {
                Color tmpColor = m_tilemaps[i].color;
                tmpColor.a = (i == (int)color) ? 1.0f : 0.2f;
                m_tilemaps[i].color = tmpColor;
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
    
    void ColorGun(LevelColor targetColor, Vector3 position, float radius)
    {
        Tilemap targetTilemap = GetLayer(targetColor).GetComponent<Tilemap>();
        var cellPos = targetTilemap.WorldToCell(position);

        for (int x = -(int)radius; x < radius + 1; x++)
        {
            for (int y = -(int)radius; y < radius + 1; y++)
            {
                var cell_pos = new Vector3Int(cellPos.x + x, cellPos.y, cellPos.z);

                bool shouldAddTileToTarget = false;
                foreach (var tilemap in m_tilemaps)
                {
                    if (tilemap != targetTilemap)
                    {
                        Debug.Log("(" + cell_pos + ")" + tilemap.name + "] : " + tilemap.GetTile(cell_pos));
                        shouldAddTileToTarget |= tilemap.GetTile(cell_pos);
                        tilemap.SetTile(cell_pos, null);
                    }
                }

                if (shouldAddTileToTarget)
                {
                    targetTilemap.SetTile(cell_pos, Tiles[(int)TileType.Filled]);
                }
            }
        }

        SwitchColor(CurrentColor);
    }

    void BeginColorBlind()
    {
        IsColorBlinded = true;

        for (int i = 0; i < m_layers.Length; i++)
        {
            m_tilemaps[i].color = ColorBlindColorRGB;
        }
    }

    void EndColorBlind()
    {
        for (int i = 0; i < m_layers.Length; i++)
        {
            Color tmpColor = m_tileColors[i];
            tmpColor.a = (i == (int)m_currentColor) ? 1.0f : 0.2f;
            m_tilemaps[i].color = tmpColor;
        }

        IsColorBlinded = false;
    }

    void ActivateStar()
    {
        IsStarActivated = true;

        //for (int i = 0; i < m_layers.Length; i++)
        //{
        //    bool layerEnabled = (i == (int)m_currentColor);
        //    // Sorting Order Priority is ascending (1 is on top of 0)
        //    m_layers[i].GetComponent<TilemapRenderer>().sortingOrder = layerEnabled ? 1 : 2;
        //    m_layers[i].GetComponent<TilemapCollider2D>().enabled = true;
        //    m_layers[i].layer = m_layerMaskInactive;
            
        //    if (!layerEnabled)
        //    {
        //        m_tilemaps[i].SwapTile(Tiles[0], Tiles[1]);
        //    }

        //    Color tmpColor = m_tilemaps[i].color;
        //    tmpColor.a = layerEnabled ? 1 : 0.7f;
        //    m_tilemaps[i].color = tmpColor;
        //}
        //m_layers[(int)m_currentColor].layer  = m_layerMaskGround;

        LevelEvents.Instance.StarActivate.Invoke();
    }

    void DeactivateStar()
    {
        IsStarActivated = false;

        //for (int i = 0; i < m_layers.Length; i++)
        //{
        //    bool layerEnabled = (i == (int)m_currentColor);

        //    m_layers[i].GetComponent<TilemapCollider2D>().enabled = layerEnabled;
        //    m_layers[i].layer = m_layerMaskGround;
        //    m_tilemaps[i].SwapTile(Tiles[1], Tiles[0]);
        //}

        LevelEvents.Instance.StarDeactivate.Invoke();
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
    private Tilemap[] m_tilemaps;
    private int m_layerMaskGround;
    private int m_layerMaskInactive;
    private WeaponController m_weaponController;
}
