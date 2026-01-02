using TMPro;
using Unity.VisualScripting;
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
        CropData info = CropController.Instance.GetCropInfo(crop);
        if (CurrencyController.Instance.CheckMoney(info.seedPrice * amount))
        {
            CropController.Instance.AddSeed(crop, amount);

            CurrencyController.Instance.SpendMoney(info.seedPrice * amount);

            UpdateDisplay();
        }
    }
}
