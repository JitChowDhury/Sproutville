using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public static GridController Instance;

    [SerializeField] private Transform gridMin;
    [SerializeField] private Transform gridMax;
    [SerializeField] private GrowBlock cellPrefab;
    [SerializeField] private LayerMask gridAllow;

    public List<GridRow> gridRows = new List<GridRow>();
    private Vector2Int gridSize;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        gridRows.Clear();

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

        gridSize = new Vector2Int(
            Mathf.RoundToInt(gridMax.position.x - gridMin.position.x),
            Mathf.RoundToInt(gridMax.position.y - gridMin.position.y)
        );

        Vector3 startPosition = gridMin.position + new Vector3(0.5f, 0.5f);

        for (int y = 0; y < gridSize.y; y++)
        {
            gridRows.Add(new GridRow());

            for (int x = 0; x < gridSize.x; x++)
            {
                GrowBlock cell = Instantiate(
                    cellPrefab,
                    startPosition + new Vector3(x, y, 0f),
                    Quaternion.identity,
                    transform
                );

                cell.SetGridPosition(x, y);
                gridRows[y].cells.Add(cell);

                bool isAllowed = Physics2D.OverlapBox(
                    cell.transform.position,
                    new Vector2(0.9f, 0.9f),
                    0f,
                    gridAllow
                );

                cell.preventUse = !isAllowed;

                Debug.DrawLine(
    cell.transform.position + Vector3.left * 0.45f,
    cell.transform.position + Vector3.right * 0.45f,
    cell.preventUse ? Color.red : Color.green,
    5f
);

                if (GridInfo.Instance.hasGridData)
                {
                    CellData data = GridInfo.Instance.gridData[y].cells[x];
                    cell.growthIndex = data.growthIndex;
                    cell.isWatered = data.isWatered;
                    cell.isPloughed = data.isPloughed;
                    cell.cropType = data.cropType;
                    cell.ApplyVisualState();
                }
            }
        }

        if (!GridInfo.Instance.hasGridData)
            GridInfo.Instance.CreateGridData(this);

        cellPrefab.gameObject.SetActive(false);
    }

    public GrowBlock GetCellFromWorldPosition(float worldX, float worldY)
    {
        int x = Mathf.RoundToInt(worldX - gridMin.position.x);
        int y = Mathf.RoundToInt(worldY - gridMin.position.y);

        if (x < 0 || y < 0 || x >= gridSize.x || y >= gridSize.y)
            return null;

        return gridRows[y].cells[x];
    }
}

[System.Serializable]
public class GridRow
{
    public List<GrowBlock> cells = new List<GrowBlock>();
}
