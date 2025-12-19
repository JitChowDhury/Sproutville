using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class GridInfo : MonoBehaviour
{

    public static GridInfo Instance;
    public bool hasGridData;

    // Data grid (state only)
    public List<GridDataRow> gridData = new List<GridDataRow>();

    private void Awake()
    {
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

    public void CreateGridData(GridController gridController)
    {
        hasGridData = true;
        gridData.Clear();

        for (int y = 0; y < gridController.gridRows.Count; y++)
        {
            gridData.Add(new GridDataRow());

            for (int x = 0; x < gridController.gridRows[y].cells.Count; x++)
            {
                gridData[y].cells.Add(new CellData());
            }
        }
    }

    public void UpdateData(GrowBlock theBlock, int xPos, int yPos)
    {
        gridData[yPos].cells[xPos].growthStage = theBlock.currentStage;
        gridData[yPos].cells[xPos].isWatered = theBlock.isWatered;
    }
}

[System.Serializable]
public class CellData
{
    public bool isWatered;
    public GrowBlock.GrowthStage growthStage;
}

[System.Serializable]
public class GridDataRow
{
    public List<CellData> cells = new List<CellData>();
}