using UnityEngine;

public class ShopController : MonoBehaviour
{
    public void OpenClose()
    {
        if (UIController.Instance.ic.gameObject.activeSelf == false)
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }

    }
}
