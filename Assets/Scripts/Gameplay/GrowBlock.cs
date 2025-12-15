using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class GrowBlock : MonoBehaviour
{
    [Header("Soil Tilemap")]
    [SerializeField] private Tilemap soilMap;
    [SerializeField] private RuleTile tilledSoilTile;
    [SerializeField] private RuleTile wateredSoilTile;
    [SerializeField] private Sprite soilTilled, soilWatered;
    public SpriteRenderer theSR;

    public bool isWatered;
    public bool preventUse;
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

    private GrowthStage currentStage;

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

    public void SetSoilSprite()
    {
        if (currentStage == GrowthStage.barren)
        {
            theSR.sprite = null;
        }
        else
        {
            if (isWatered == true)
            {
                theSR.sprite = soilWatered;
            }
            else
            {
                theSR.sprite = soilTilled;
            }
        }
    }

    public void PloughSoil()
    {
        if (currentStage == GrowthStage.barren && preventUse == false)
        {
            currentStage = GrowthStage.ploughed;
            Vector3Int cellPos = soilMap.WorldToCell(transform.position);
            soilMap.SetTile(cellPos, tilledSoilTile);
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
        soilMap.SetTile(cellPos, wateredSoilTile);

    }

    public void PlantCrop()
    {
        if (currentStage == GrowthStage.ploughed && isWatered == true && preventUse == false)
        {
            currentStage = GrowthStage.planted;
            UpdateCropSprite();
        }
    }


    void UpdateCropSprite()
    {
        switch (currentStage)
        {
            case GrowthStage.planted:
                cropSR.sprite = cropPlanted;
                break;
            case GrowthStage.growing1:
                cropSR.sprite = cropGrowth1;
                break;
            case GrowthStage.growing2:
                cropSR.sprite = cropGrowth2;
                break;
            case GrowthStage.growing3:
                cropSR.sprite = cropGrowth3;
                break;
            case GrowthStage.ripe:
                cropSR.sprite = cropRipe;
                break;
        }
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

                UpdateCropSprite();
            }
        }
    }

    public void HarvestCrop()
    {
        if (currentStage == GrowthStage.ripe && preventUse == false)
        {
            currentStage = GrowthStage.ploughed;
            SetSoilSprite();
            cropSR.sprite = null;
        }
    }
}
