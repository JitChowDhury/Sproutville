using System.Collections.Generic;
using UnityEngine;

public class CropController : MonoBehaviour
{
    public static CropController Instance;

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

    public enum CropType
    {
        chilli,
        tomato,
        pumpkin,
        wheat,
        onion,
        broccoli,
        lettuce,
        cauliflower,
        grapes,
        carrot
    }

    public List<CropData> cropList = new List<CropData>();

    public CropData GetCropInfo(CropType type)
    {
        foreach (var crop in cropList)
        {
            if (crop.cropType == type)
                return crop;
        }
        return null;
    }

    public void UseSeed(CropType type)
    {
        CropData crop = GetCropInfo(type);
        if (crop != null && crop.seedAmount > 0)
            crop.seedAmount--;
    }

    public void AddCrop(CropType type)
    {
        CropData crop = GetCropInfo(type);
        if (crop != null)
            crop.cropAmount++;
    }
}

[System.Serializable]
public class CropData
{
    public CropController.CropType cropType;

    [Tooltip("Ordered growth sprites (day 1 → ripe)")]
    public List<Sprite> growthSprites;

    public int seedAmount;
    public int cropAmount;
    public Sprite seedSprite;
    public int TotalGrowthStages => growthSprites.Count;
}
