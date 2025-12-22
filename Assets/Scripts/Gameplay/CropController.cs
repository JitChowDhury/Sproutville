using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Rendering;

public class CropController : MonoBehaviour
{

    public static CropController Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
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

    public CropData GetCropInfo(CropType cropToGet)
    {
        int position = -1;
        for (int i = 0; i < cropList.Count; i++)
        {
            if (cropList[i].cropType == cropToGet)
            {
                position = i;
            }
        }

        if (position >= 0)
        {
            return cropList[position];
        }
        else
        {
            return null;
        }
    }
    public void UseSeed(CropType seedToUse)
    {
        foreach (CropData data in cropList)
        {
            if (data.cropType == seedToUse)
            {
                data.seedAmount--;
            }
        }
    }

    public void AddCrop(CropType cropToAdd)
    {
        foreach (CropData data in cropList)
        {
            if (data.cropType == cropToAdd)
            {
                data.cropAmount++;
            }
        }
    }




}
[System.Serializable]
public class CropData
{
    public CropController.CropType cropType;
    public Sprite finalCrop, seedType, planted, growing1, growing2, growing3, ripe;

    public int seedAmount, cropAmount;

}

