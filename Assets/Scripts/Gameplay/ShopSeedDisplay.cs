using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopSeedDisplay : MonoBehaviour
{
    public CropController.CropType crop;
    public Image seedImage;
    public TMP_Text seedAmount, priceText;

    public void UpdateDisplay()
    {
        CropData info = CropController.Instance.GetCropInfo(crop);
        seedImage.sprite = info.seedSprite;
        seedAmount.text = "x" + info.seedAmount;
        priceText.text = "$" + info.seedPrice + " each";
    }

    public void BuySeed(int amount)
    {

    }
}
