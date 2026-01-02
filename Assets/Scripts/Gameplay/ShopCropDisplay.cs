using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class ShopCropDisplay : MonoBehaviour
{
    public CropController.CropType crop;
    public Image cropImage;
    public TMP_Text amountText, priceText;

    public void UpdateDisplay()
    {
        CropData info = CropController.Instance.GetCropInfo(crop);
        cropImage.sprite = info.finalCrop;
        amountText.text = "x" + info.cropAmount;

        priceText.text = "$" + info.cropPrice + " each";
    }

    public void SellCrop()
    {
        CropData info = CropController.Instance.GetCropInfo(crop);

        if (info.cropAmount > 0)
        {
            CurrencyController.Instance.AddMoney(info.cropAmount * info.cropPrice);

            CropController.Instance.RemoveCrop(crop);

            UpdateDisplay();
        }

    }
}
