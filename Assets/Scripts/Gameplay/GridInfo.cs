using System.Collections.Generic;
using UnityEngine;

public class GridInfo : MonoBehaviour
{
    public static GridInfo Instance;

    public bool hasGridData;
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

    public void CreateGridData(GridController grid)
    {
        hasGridData = true;
        gridData.Clear();

        for (int y = 0; y < grid.gridRows.Count; y++)
        {
            GridDataRow row = new GridDataRow();
            for (int x = 0; x < grid.gridRows[y].cells.Count; x++)
            {
                row.cells.Add(new CellData());
            }
            gridData.Add(row);
        }
    }

    public void UpdateData(GrowBlock block, int x, int y)
    {
        CellData cell = gridData[y].cells[x];
        cell.growthIndex = block.growthIndex;
        cell.isWatered = block.isWatered;
        cell.isPloughed = block.isPloughed;
        cell.cropType = block.cropType;
    }

    public void GrowCrop()
    {
        for (int y = 0; y < gridData.Count; y++)
        {
            for (int x = 0; x < gridData[y].cells.Count; x++)
            {
                CellData cell = gridData[y].cells[x];
                if (!cell.isWatered || cell.growthIndex < 0) continue;

                CropData crop = CropController.Instance.GetCropInfo(cell.cropType);
                int maxIndex = crop.TotalGrowthStages - 1;

                if (cell.growthIndex < maxIndex)
                    cell.growthIndex++;

                cell.isWatered = false;

            }
        }
    }
}

[System.Serializable]
public class CellData
{
    public int growthIndex = -1;
    public bool isWatered;
    public bool isPloughed;
    public CropController.CropType cropType;
}

[System.Serializable]
public class GridDataRow
{
    public List<CellData> cells = new List<CellData>();
}
