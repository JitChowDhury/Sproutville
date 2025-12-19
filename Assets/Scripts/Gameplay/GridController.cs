using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridController : MonoBehaviour
{
    public static GridController Instance;
    [Header("Grid Bounds")]
    [SerializeField] private Transform gridMin;
    [SerializeField] private Transform gridMax;

    [Header("Grid Cell")]
    [SerializeField] private GrowBlock cellPrefab;

    [Header("Blocking")]
    [SerializeField] private LayerMask gridBlockers;

    private Vector2Int gridSize;

    public List<GridRow> gridRows = new List<GridRow>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        gridRows.Clear();

        //snap grid bounds to whole numbers
        gridMin.position = new Vector3(Mathf.Round(gridMin.position.x), Mathf.Round(gridMin.position.y), 0f);
        gridMax.position = new Vector3(Mathf.Round(gridMax.position.x), Mathf.Round(gridMax.position.y), 0f);

        gridSize = new Vector2Int(Mathf.RoundToInt(gridMax.position.x - gridMin.position.x), Mathf.RoundToInt(gridMax.position.y - gridMin.position.y));

        //first cell position
        Vector3 startPosition = gridMin.position + new Vector3(0.5f, 0.5f, 0.5f);
        //create grid cells
        for (int y = 0; y < gridSize.y; y++)
        {
            gridRows.Add(new GridRow());
            for (int x = 0; x < gridSize.x; x++)
            {
                GrowBlock cell = Instantiate(cellPrefab, startPosition + new Vector3(x, y, 0f), Quaternion.identity, transform);

                cell.theSR.sprite = null;
                cell.SetGridPosition(x, y);
                gridRows[y].cells.Add(cell);

                if (Physics2D.OverlapBox(cell.transform.position, new Vector2(0.9f, 0.9f), 0f, gridBlockers))
                {
                    cell.preventUse = true;
                }
            }
        }
        // Initialize grid data only once
        if (!GridInfo.Instance.hasGridData)
        {
            GridInfo.Instance.CreateGridData(this);
        }

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

    public Vector2Int GridSize => gridSize;




}
[System.Serializable]
public class GridRow
{
    public List<GrowBlock> cells = new List<GrowBlock>();
}