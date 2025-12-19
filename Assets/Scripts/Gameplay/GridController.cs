using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// GridController is responsible for generating the physical grid in the world.
/// It creates GrowBlock objects and places them in a structured grid.
/// 
/// Think of this as the "builder" of the farm.
/// </summary>
public class GridController : MonoBehaviour
{
    // Singleton for easy access from other systems
    public static GridController Instance;

    [Header("Grid Bounds")]
    // Bottom-left corner of the grid
    [SerializeField] private Transform gridMin;

    // Top-right corner of the grid
    [SerializeField] private Transform gridMax;

    [Header("Grid Cell")]
    // Prefab used to create each grid cell
    [SerializeField] private GrowBlock cellPrefab;

    [Header("Blocking")]
    // Layer used to detect blocked tiles (rocks, walls, etc.)
    [SerializeField] private LayerMask gridBlockers;

    // Stores width (x) and height (y) of the grid
    private Vector2Int gridSize;

    // 2D grid structure stored as rows of cells
    public List<GridRow> gridRows = new List<GridRow>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // Build the grid when the scene starts
        GenerateGrid();
    }

    /// <summary>
    /// Generates the entire grid based on gridMin and gridMax.
    /// This creates all GrowBlock instances and restores saved data if available.
    /// </summary>
    private void GenerateGrid()
    {
        // Clear any old grid data (safety)
        gridRows.Clear();

        // Snap grid bounds to whole numbers to avoid floating-point offsets
        gridMin.position = new Vector3(
            Mathf.Round(gridMin.position.x),
            Mathf.Round(gridMin.position.y),
            0f
        );

        gridMax.position = new Vector3(
            Mathf.Round(gridMax.position.x),
            Mathf.Round(gridMax.position.y),
            0f
        );

        // Calculate grid size based on bounds
        gridSize = new Vector2Int(
            Mathf.RoundToInt(gridMax.position.x - gridMin.position.x),
            Mathf.RoundToInt(gridMax.position.y - gridMin.position.y)
        );

        // First cell is placed at the center of the first grid square
        Vector3 startPosition = gridMin.position + new Vector3(0.5f, 0.5f, 0f);

        // Create grid row by row
        for (int y = 0; y < gridSize.y; y++)
        {
            gridRows.Add(new GridRow());

            for (int x = 0; x < gridSize.x; x++)
            {
                // Instantiate a new cell
                GrowBlock cell = Instantiate(
                    cellPrefab,
                    startPosition + new Vector3(x, y, 0f),
                    Quaternion.identity,
                    transform
                );

                // Clear sprite initially
                cell.theSR.sprite = null;

                // Store grid position inside the cell itself
                cell.SetGridPosition(x, y);

                // Add cell to grid structure
                gridRows[y].cells.Add(cell);

                // Check if this tile is blocked by an object
                if (Physics2D.OverlapBox(
                    cell.transform.position,
                    new Vector2(0.9f, 0.9f),
                    0f,
                    gridBlockers))
                {
                    cell.preventUse = true;
                }

                // Restore saved data if grid data already exists
                if (GridInfo.Instance.hasGridData)
                {
                    CellData storedData = GridInfo.Instance.gridData[y].cells[x];

                    cell.currentStage = storedData.growthStage;
                    cell.isWatered = storedData.isWatered;

                    // Apply visuals based on restored state
                    cell.ApplyVisualState();
                }
            }
        }

        // Create grid data if this is the first time generating the grid
        if (!GridInfo.Instance.hasGridData)
        {
            GridInfo.Instance.CreateGridData(this);
        }

        // Disable prefab so it does not appear in the scene
        cellPrefab.gameObject.SetActive(false);
    }

    /// <summary>
    /// Converts a world position into a grid cell.
    /// Used for mouse clicks, player interaction, etc.
    /// </summary>
    public GrowBlock GetCellFromWorldPosition(float worldX, float worldY)
    {
        int x = Mathf.RoundToInt(worldX - gridMin.position.x);
        int y = Mathf.RoundToInt(worldY - gridMin.position.y);

        // Bounds check
        if (x < 0 || y < 0 || x >= gridSize.x || y >= gridSize.y)
            return null;

        return gridRows[y].cells[x];
    }

    // Public access to grid size if needed by other systems
    public Vector2Int GridSize => gridSize;
}

/// <summary>
/// Represents one horizontal row of GrowBlock cells.
/// Used to simulate a 2D grid using lists.
/// </summary>
[System.Serializable]
public class GridRow
{
    public List<GrowBlock> cells = new List<GrowBlock>();
}
