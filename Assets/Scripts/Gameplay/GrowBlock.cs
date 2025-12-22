using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class GrowBlock : MonoBehaviour
{
    [Header("Soil Tilemap")]
    [SerializeField] private Tilemap soilMap;
    [SerializeField] private TileBase tilledSoilTile;

    [SerializeField] private TileBase wateredSoilTile;
    // [SerializeField] private Sprite soilTilled, soilWatered;
    public SpriteRenderer theSR;

    public bool isWatered;
    public bool preventUse;

    private Vector2Int gridPos;
    public enum GrowthStage
    {
        barren,
        ploughed,
        planted,
        growing1,
        growing2,
        growing3,
        ripe
    }

    public GrowthStage currentStage;
    public CropController.CropType cropType;

    [SerializeField] private SpriteRenderer cropSR;
    [SerializeField] private Sprite cropPlanted, cropGrowth1, cropGrowth2, cropGrowth3, cropRipe;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        /* if (Keyboard.current.eKey.wasPressedThisFrame)
         {
             AdvanceStage();

             SetSoilSprite();
         }
         */

#if UNITY_EDITOR
        if (Keyboard.current.nKey.wasReleasedThisFrame)
        {
            AdvanceCrop();
        }
#endif

    }

    void AdvanceStage()
    {
        currentStage = currentStage + 1;
        if ((int)currentStage >= 6)
        {
            currentStage = GrowthStage.barren;
        }
    }

    // public void SetSoilSprite()
    // {
    //     if (currentStage == GrowthStage.barren)
    //     {
    //         theSR.sprite = null;
    //     }
    //     else
    //     {
    //         if (isWatered == true)
    //         {
    //             theSR.sprite = soilWatered;
    //         }
    //         else
    //         {
    //             theSR.sprite = soilTilled;
    //         }
    //     }
    // }

    public void PloughSoil()
    {
        if (currentStage == GrowthStage.barren && preventUse == false)
        {
            currentStage = GrowthStage.ploughed;
            Vector3Int cellPos = soilMap.WorldToCell(transform.position);
            UpdateGridInfo();
            soilMap.SetTile(cellPos, tilledSoilTile);
            soilMap.RefreshTile(cellPos);
        }
    }

    public void WaterSoil()
    {
        if (preventUse) return;

        if (currentStage != GrowthStage.ploughed &&
        currentStage != GrowthStage.planted &&
        currentStage != GrowthStage.growing1 &&
        currentStage != GrowthStage.growing2 &&
        currentStage != GrowthStage.growing3)
            return;

        Vector3Int cellPos = soilMap.WorldToCell(transform.position);
        if (!soilMap.HasTile(cellPos))
        {
            return;
        }

        isWatered = true;
        UpdateGridInfo();
        soilMap.SetTile(cellPos, wateredSoilTile);
        soilMap.RefreshTile(cellPos);


    }

    public void PlantCrop(CropController.CropType cropToPlant)
    {
        if (currentStage == GrowthStage.ploughed && isWatered == true && preventUse == false)
        {
            currentStage = GrowthStage.planted;
            cropType = cropToPlant;
            UpdateCropSprite();
        }
    }


    public void UpdateCropSprite()
    {

        CropData activeCrop = CropController.Instance.GetCropInfo(cropType);
        switch (currentStage)
        {
            case GrowthStage.planted:
                // cropSR.sprite = cropPlanted;
                cropSR.sprite = activeCrop.planted;
                break;
            case GrowthStage.growing1:
                // cropSR.sprite = cropGrowth1;
                cropSR.sprite = activeCrop.growing1;
                break;
            case GrowthStage.growing2:
                // cropSR.sprite = cropGrowth2;
                cropSR.sprite = activeCrop.growing2;
                break;
            case GrowthStage.growing3:
                // cropSR.sprite = cropGrowth3;
                cropSR.sprite = activeCrop.growing3;
                break;
            case GrowthStage.ripe:
                // cropSR.sprite = cropRipe;
                cropSR.sprite = activeCrop.ripe;
                break;
        }
        UpdateGridInfo();
    }

    void AdvanceCrop()
    {
        if (isWatered == true && preventUse == false)
        {
            if (currentStage == GrowthStage.planted || currentStage == GrowthStage.growing1 || currentStage == GrowthStage.growing2 || currentStage == GrowthStage.growing3)
            {
                currentStage++;
                isWatered = false;

                Vector3Int cellPos = soilMap.WorldToCell(transform.position);
                soilMap.SetTile(cellPos, tilledSoilTile);
                soilMap.RefreshTile(cellPos);
                UpdateCropSprite();
            }
        }

    }

    public void HarvestCrop()
    {
        if (currentStage == GrowthStage.ripe && preventUse == false)
        {
            currentStage = GrowthStage.ploughed;
            cropSR.sprite = null;

            Vector3Int cellPos = soilMap.WorldToCell(transform.position);
            soilMap.SetTile(cellPos, tilledSoilTile);
            CropController.Instance.AddCrop(cropType);
            soilMap.RefreshTile(cellPos);
        }
    }

    public void SetGridPosition(int x, int y)
    {
        gridPos = new Vector2Int(x, y);
    }

    void UpdateGridInfo()
    {
        GridInfo.Instance.UpdateData(this, gridPos.x, gridPos.y);
    }

    public void ApplyVisualState()
    {
        Vector3Int cellPos = soilMap.WorldToCell(transform.position);

        // ----- Soil Tile -----
        if (currentStage == GrowthStage.barren)
        {
            soilMap.SetTile(cellPos, null);
        }
        else
        {
            soilMap.SetTile(
                cellPos,
                isWatered ? wateredSoilTile : tilledSoilTile
            );
        }

        soilMap.RefreshTile(cellPos);
        CropData activeCrop = CropController.Instance.GetCropInfo(cropType);
        switch (currentStage)
        {
            case GrowthStage.planted:
                // cropSR.sprite = cropPlanted;
                cropSR.sprite = activeCrop.planted;
                break;
            case GrowthStage.growing1:
                // cropSR.sprite = cropGrowth1;
                cropSR.sprite = activeCrop.growing1;
                break;
            case GrowthStage.growing2:
                // cropSR.sprite = cropGrowth2;
                cropSR.sprite = activeCrop.growing2;
                break;
            case GrowthStage.growing3:
                // cropSR.sprite = cropGrowth3;
                cropSR.sprite = activeCrop.growing3;
                break;
            case GrowthStage.ripe:
                // cropSR.sprite = cropRipe;
                cropSR.sprite = activeCrop.ripe;
                break;
        }
    }

}
