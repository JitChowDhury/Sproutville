using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController instance;
    public GameObject[] toolBarActivatorIcons;


    private void Awake()
    {
        instance = this;
    }
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
