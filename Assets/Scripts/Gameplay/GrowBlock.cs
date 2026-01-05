using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class GrowBlock : MonoBehaviour
{
    [Header("Soil Tilemap")]
    [SerializeField] private Tilemap soilMap;
    [SerializeField] private TileBase tilledSoilTile;
    [SerializeField] private TileBase wateredSoilTile;

    [SerializeField] private SpriteRenderer cropSR;

    public bool isPloughed;
    public bool isWatered;
    public bool preventUse = true;

    public int growthIndex = -1; // -1 = no crop
    public CropController.CropType cropType;

    private Vector2Int gridPos;

    void Update()
    {
#if UNITY_EDITOR
        // DEBUG: manual growth step (same purpose as before)
        if (Keyboard.current.nKey.wasReleasedThisFrame)
        {
            AdvanceCropDebug();
        }
#endif
    }

    // ---------- SOIL ----------

    public void PloughSoil()
    {
        if (isPloughed || preventUse) return;

        FindSoilMapIfNeeded();

        isPloughed = true;
        SetSoilTile(tilledSoilTile);
        UpdateGridInfo();
    }

    private void FindSoilMapIfNeeded()
    {
        if (soilMap != null) return;

        GameObject soilObj = GameObject.FindGameObjectWithTag("GroundOverlay");
        if (soilObj == null)
        {
            Debug.LogError("GroundOverlay Tilemap not found in scene.");
            return;
        }

        soilMap = soilObj.GetComponent<Tilemap>();
    }

    public void WaterSoil()
    {
        if (!isPloughed || preventUse) return;

        FindSoilMapIfNeeded();

        isWatered = true;
        SetSoilTile(wateredSoilTile);
        UpdateGridInfo();
    }


    // ---------- CROP ----------

    public bool PlantCrop(CropController.CropType type)
    {
        if (!isPloughed || !isWatered || preventUse || growthIndex >= 0)
            return false;

        cropType = type;
        growthIndex = 0;

        UpdateCropSprite();
        UpdateGridInfo();
        if (TutorialManager.Instance.CurrentState ==
            TutorialManager.TutorialState.SeedsGiven)
        {
            TutorialManager.Instance.SetState(
                TutorialManager.TutorialState.SeedPlanted
            );
        }

        return true;
    }


    public void HarvestCrop()
    {
        if (growthIndex < 0 || preventUse) return;

        CropData crop = CropController.Instance.GetCropInfo(cropType);
        if (growthIndex < crop.TotalGrowthStages - 1) return;

        CropController.Instance.AddCrop(cropType);

        growthIndex = -1;
        cropSR.sprite = null;

        SetSoilTile(tilledSoilTile);
        UpdateGridInfo();
    }

    // ---------- GROWTH ----------

    void AdvanceCropDebug()
    {
        if (!isWatered || preventUse || growthIndex < 0) return;

        CropData crop = CropController.Instance.GetCropInfo(cropType);

        if (growthIndex < crop.TotalGrowthStages - 1)
        {
            if (TutorialManager.Instance.CurrentState ==
    TutorialManager.TutorialState.SeedPlanted)
            {
                TutorialManager.Instance.SetState(
                    TutorialManager.TutorialState.CropGrowing
                );
            }

            growthIndex++;
            isWatered = false;

            SetSoilTile(tilledSoilTile);
            UpdateCropSprite();
        }
        else if (growthIndex == crop.TotalGrowthStages - 1)
        {
            if (TutorialManager.Instance.CurrentState ==
                TutorialManager.TutorialState.CropGrowing)
            {
                TutorialManager.Instance.SetState(
                    TutorialManager.TutorialState.CropFullyGrown
                );
            }

        }
    }

    public void UpdateCropSprite()
    {
        if (growthIndex < 0) return;

        CropData crop = CropController.Instance.GetCropInfo(cropType);
        cropSR.sprite = crop.growthSprites[growthIndex];

        UpdateGridInfo();
    }

    // ---------- GRID ----------

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
        FindSoilMapIfNeeded();
        if (soilMap == null) return;

        Vector3Int cellPos = soilMap.WorldToCell(transform.position);

        if (!isPloughed)
            soilMap.SetTile(cellPos, null);
        else
            soilMap.SetTile(cellPos, isWatered ? wateredSoilTile : tilledSoilTile);

        soilMap.RefreshTile(cellPos);

        if (growthIndex >= 0)
        {
            CropData crop = CropController.Instance.GetCropInfo(cropType);
            cropSR.sprite = crop.growthSprites[growthIndex];
        }
        else
        {
            cropSR.sprite = null;
        }
    }


    void SetSoilTile(TileBase tile)
    {
        FindSoilMapIfNeeded();
        if (soilMap == null) return;

        Vector3Int pos = soilMap.WorldToCell(transform.position);
        soilMap.SetTile(pos, tile);
        soilMap.RefreshTile(pos);
    }
}

