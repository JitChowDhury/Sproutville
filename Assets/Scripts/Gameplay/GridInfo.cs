using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// GridInfo is responsible for storing the DATA of the grid.
/// This script does NOT create tiles or visuals.
/// It only remembers state like watering and growth stage.
/// 
/// Think of this as the "memory notebook" of the farm.
/// </summary>
public class GridInfo : MonoBehaviour
{
    // Singleton instance so any tile can update its data easily
    public static GridInfo Instance;

    // Used to ensure grid data is created only once
    public bool hasGridData;

    // 2D grid data stored as rows of cell data
    // gridData[y][x] matches GridController.gridRows[y][x]
    public List<GridDataRow> gridData = new List<GridDataRow>();

    private void Awake()
    {
        // Singleton pattern + persistence across scenes
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // void Update()
    // {
    //     if (Keyboard.current.spaceKey.wasPressedThisFrame)
    //     {
    //         GrowCrop();
    //     }
    // }




    /// <summary>
    /// Creates an empty data grid that mirrors the size of the world grid.
    /// Called only once when the grid is generated for the first time.
    /// </summary>
    public void CreateGridData(GridController gridController)
    {
        hasGridData = true;
        gridData.Clear();

        // Loop through each row in the world grid
        for (int y = 0; y < gridController.gridRows.Count; y++)
        {
            gridData.Add(new GridDataRow());

            // Loop through each cell in that row
            for (int x = 0; x < gridController.gridRows[y].cells.Count; x++)
            {
                // Create empty data for each tile
                gridData[y].cells.Add(new CellData());
            }
        }
    }

    /// <summary>
    /// Updates stored data for a specific tile.
    /// Called when a GrowBlock changes state (watering / growth).
    /// </summary>
    public void UpdateData(GrowBlock theBlock, int xPos, int yPos)
    {
        gridData[yPos].cells[xPos].growthStage = theBlock.currentStage;
        gridData[yPos].cells[xPos].isWatered = theBlock.isWatered;
        gridData[yPos].cells[xPos].cropType = theBlock.cropType;
    }

    public void GrowCrop()
    {
        for (int y = 0; y < gridData.Count; y++)
        {
            for (int x = 0; x < gridData[y].cells.Count; x++)
            {
                if (gridData[y].cells[x].isWatered == true)
                {
                    switch (gridData[y].cells[x].growthStage)
                    {
                        case GrowBlock.GrowthStage.planted:
                            gridData[y].cells[x].growthStage = GrowBlock.GrowthStage.growing1;
                            break;
                        case GrowBlock.GrowthStage.growing1:
                            gridData[y].cells[x].growthStage = GrowBlock.GrowthStage.growing2;
                            break;
                        case GrowBlock.GrowthStage.growing2:
                            gridData[y].cells[x].growthStage = GrowBlock.GrowthStage.growing3;
                            break;
                        case GrowBlock.GrowthStage.growing3:
                            gridData[y].cells[x].growthStage = GrowBlock.GrowthStage.ripe;
                            break;
                        default:
                            break;
                    }
                    gridData[y].cells[x].isWatered = false;

                }
            }
        }
    }
}

/// <summary>
/// Stores the persistent state of a single grid cell.
/// No visuals, no transforms, just pure data.
/// </summary>
[System.Serializable]
public class CellData
{
    public bool isWatered;
    public GrowBlock.GrowthStage growthStage;
    public CropController.CropType cropType;
}

/// <summary>
/// Represents one horizontal row of CellData.
/// Used to simulate a 2D array using lists.
/// </summary>
[System.Serializable]
public class GridDataRow
{
    public List<CellData> cells = new List<CellData>();
}
