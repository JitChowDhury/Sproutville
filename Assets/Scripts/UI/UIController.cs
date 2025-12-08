using UnityEngine;

public class UIController : MonoBehaviour
{
    public GameObject[] toolBarActivatorIcons;

    void Start()
    {

    }

    public void SwitchTool(int selected)
    {
        foreach (GameObject icon in toolBarActivatorIcons)
        {
            icon.SetActive(false);
        }

        toolBarActivatorIcons[selected].SetActive(true);
    }
}
