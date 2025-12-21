using System.Collections.Generic;
using UnityEngine;

public class CropController : MonoBehaviour
{

    public static CropController Instance;
    private void Awake()
    {
        if (Instance = null)
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

}
[System.Serializable]
public class CropData
{
    public CropController.CropType cropType;
    public Sprite finalCrop, seedType, planted, growing1, growing2, growing3, ripe;

    public int seedAmount, cropAmount;

}

