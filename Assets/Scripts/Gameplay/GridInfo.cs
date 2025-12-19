using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridInfo : MonoBehaviour
{

    public static GridInfo Instance;
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

    public bool hasGrid;
    public List<InfoRow> theGrid;

    public void CreateGrid()
    {
        hasGrid = true;
        for (int y = 0; y < GridController.Instance.blockRows.Count; y++)
        {
            theGrid.Add(new InfoRow());

            for (int x = 0; x < GridController.Instance.blockRows[y].blocks.Count; x++)
            {
                theGrid[y].blocks.Add(new BlockInfo());
            }
        }
    }
}

[System.Serializable]
public class BlockInfo
{
    public bool isWatered;
    public GrowBlock.GrowthStage currentStage;
}
[System.Serializable]

public class InfoRow
{
    public List<BlockInfo> blocks = new List<BlockInfo>();
}
