using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SeedDisplay : MonoBehaviour
{
    public CropController.CropType crop;
    [SerializeField] private Image seedImage;
    [SerializeField] private TMP_Text seedAmount;

    public void UpdateDisplay()
    {
        CropData info = CropController.Instance.GetCropInfo(crop);
        seedImage.sprite = info.seedSprite;
        seedAmount.text = "x" + info.seedAmount;
    }
    public void SelectSeed()
    {

    }
}
