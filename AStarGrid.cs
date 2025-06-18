using System;
using UnityEngine;

public class TileData
{
    private string footstepSoundKey;
    private float friction;
    
    public string FootstepSoundKey => footstepSoundKey;
    public float Friction => friction;

    public TileData(string footstepSoundKey, float friction)
    {
        this.footstepSoundKey = footstepSoundKey;
        this.friction = friction;
    }
}

public class AStarGrid : MonoBehaviour
{

    [SerializeField] private int height = 10;
    [SerializeField] private int width = 10;
    [SerializeField] public float tileSize = 0.5f;
    [SerializeField] private bool drawGrid = false;
    [SerializeField] private Texture2D colorMap;

    private Tile[,] grid;
    private Vector2 origin;
    private Vector2 lowerBounds;
    private Vector2 upperBounds;
    // tile data
    private TileData grassTileData;
    private TileData stoneTileData;
    private TileData sandTileData;
    private TileData soilTileData;
    // colors
    private Color grassColor;
    private Color stoneColor;
    private Color sandColor;
    private Color soilColor;

    public static AStarGrid Instance;
    public Action<Tile[,]> GridComplete;

    public int Width
    {
        get { return width; }
        private set { width = value; }
    }

    public int Height
    {
        get { return height; }
        private set { height = value; }
    }

    public float TileSize
    {
        get { return tileSize; }
        private set { tileSize = value; }
    }

    public Tile[,] Grid
    {
        get { return grid; }
        private set { grid = value; }
    }

    public Vector2 LowerBounds
    {
        get { return lowerBounds; }
        private set { lowerBounds = value; }
    }

    public Vector2 UpperBounds
    {
        get { return upperBounds; }
        private set { upperBounds = value; }
    }

    private void Awake()
    {
        if ((Instance != null) && (Instance != this))
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        origin = transform.position;
        SetUpTileData();
        CreateGrid();
    }

    private void FixedUpdate()
    {
        if (drawGrid)
        {
            DrawGrid();
        }
    }

    // set up colors and tile data
    private void SetUpTileData()
    {
        grassColor = new Color(0f / 255f, 255f / 255f, 0f / 255f, 255f / 255f);
        stoneColor = new Color(128f / 255f, 128f / 255f, 128f / 255f, 255f / 255f);
        sandColor = new Color(237f / 255f, 201f / 255f, 175f / 255f, 255f / 255f);
        soilColor = new Color(101f / 255f, 67f / 255f, 33f / 255f, 255f / 255f);

        grassTileData = new TileData(AudioLabels.Footsteps_Grass, 1f);
        stoneTileData = new TileData(AudioLabels.Footsteps_Stone, 1f);
        sandTileData = new TileData(AudioLabels.Footsteps_Sand, 1f);
        soilTileData = new TileData(AudioLabels.Footsteps_Soil, 1f);
    }

    // function for creation of new grid based on parameters
    private void CreateGrid()
    {
        grid = new Tile[width, height];

        lowerBounds.x = origin.x - (tileSize * ((float)width / 2));
        lowerBounds.y = origin.y - (tileSize * ((float)height / 2));
        //Debug.Log("Lower Bounds: " + lowerBounds.x + ", " + lowerBounds.y);

        upperBounds.x = lowerBounds.x + (tileSize * width);
        upperBounds.y = lowerBounds.y + (tileSize * height);
        //Debug.Log("Upper Bounds: " + upperBounds.x + ", " + upperBounds.y);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                InitializeTile(x, y);
            }
        }

        GridComplete?.Invoke(grid);
    }

    // function for updating tiles in grid
    private void UpdateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                grid[x, y].UpdateTile();
            }
        }
    }

    private void InitializeTile(int x, int y)
    {
        Color tileColor = colorMap.GetPixel(x, y);
        Vector2 position = new Vector2(lowerBounds.x + (x * tileSize), lowerBounds.y + (y * tileSize));

        if (tileColor == null)
        {
            grid[x, y] = new Tile(position);
        }
        else if (tileColor == grassColor)
        {
            grid[x, y] = new Tile(position, grassTileData);
        }
        else if (tileColor == stoneColor)
        {
            grid[x, y] = new Tile(position, stoneTileData);
        }
        else if (tileColor == sandColor)
        {
            grid[x, y] = new Tile(position, sandTileData);
        }
        else if (tileColor == soilColor)
        {
            grid[x, y] = new Tile(position, soilTileData);
        }
        else
        {
            grid[x, y] = new Tile(position);
        }
    }

    // convert the world coordinates to index of Grid
    public Vector2Int MapToCoordinates(Vector2 pos)
    {
        int xIndex = (int)((pos.x - lowerBounds.x) / tileSize);
        int yIndex = (int)((pos.y - lowerBounds.y) / tileSize);

        return new Vector2Int(xIndex, yIndex);
    }

    // convert the index of Grid to world coordinates
    public Vector2 CoordinatesToMap(Vector2 pos)
    {
        float xIndex = (pos.x * tileSize) + lowerBounds.x;
        float yIndex = (pos.y * tileSize) + lowerBounds.y;

        return new Vector2(xIndex, yIndex);
    }

    private void DrawGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                DrawTile(grid[x, y]);
            }
        }
    }

    private void DrawTile(Tile tile)
    {
        float halfTileSize = tileSize / 2f;

        Vector3 tilePosition = new Vector3(tile.Position.x, tile.Position.y, 0);

        Vector3 bottomLeft = tilePosition + new Vector3(-halfTileSize, -halfTileSize, 0);
        Vector3 bottomRight = tilePosition + new Vector3(halfTileSize, -halfTileSize, 0);
        Vector3 topLeft = tilePosition + new Vector3(-halfTileSize, halfTileSize, 0);
        Vector3 topRight = tilePosition + new Vector3(halfTileSize, halfTileSize, 0);

        Color lineColor = tile.IsBlocked ? Color.red : Color.green;

        Debug.DrawLine(bottomLeft, bottomRight, lineColor, 0.1f);
        Debug.DrawLine(bottomRight, topRight, lineColor, 0.1f);
        Debug.DrawLine(topRight, topLeft, lineColor, 0.1f);
        Debug.DrawLine(topLeft, bottomLeft, lineColor, 0.1f);
    }
}
