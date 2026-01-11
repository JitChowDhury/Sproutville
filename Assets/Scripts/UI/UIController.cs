using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController Instance;
    public GameObject[] toolBarActivatorIcons;
    public TMP_Text timeText;
    public TMP_Text moneyText;
    public InventoryController ic;
    public ShopController theShop;
    public Image seedImage;
    public GameObject pauseScreen;

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
    private void Start()
    {
        if (CurrencyController.Instance != null)
        {
            UpdateMoneyText(CurrencyController.Instance.currentMoney);
        }
    }


    void Update()
    {
        if (Keyboard.current.iKey.wasPressedThisFrame)
        {
            ic.OpenClose();
        }
#if UNITY_EDITOR
        if (Keyboard.current.bKey.wasPressedThisFrame)
        {
            theShop.OpenClose();
        }
#endif
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            PauseUnpause();
        }

    }

    public void SwitchTool(int selected)
    {
        foreach (GameObject icon in toolBarActivatorIcons)
        {
            icon.SetActive(false);
        }

        toolBarActivatorIcons[selected].SetActive(true);
    }

    public void UpdateTimeText(float currentTime)
    {
        if (currentTime < 12)
        {
            timeText.text = Mathf.FloorToInt(currentTime) + "AM";
        }
        else if (currentTime < 13)
        {
            timeText.text = "12PM";
        }
        else if (currentTime < 24)
        {
            timeText.text = Mathf.FloorToInt(currentTime) + "PM";
        }
        else if (currentTime < 25)
        {
            timeText.text = "12AM";
        }
        else
        {
            timeText.text = Mathf.FloorToInt(currentTime - 24) + "AM";
        }

    }

    public void SwitchSeed(CropController.CropType crop)
    {
        seedImage.sprite = CropController.Instance.GetCropInfo(crop).seedSprite;
    }

    public void UpdateMoneyText(float currentMoney)
    {
        moneyText.text = currentMoney.ToString();
    }

    public void PauseUnpause()
    {
        if (pauseScreen.activeSelf == false)
        {
            pauseScreen.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            pauseScreen.SetActive(false);
            Time.timeScale = 1f;
        }

    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
        Destroy(gameObject);
        Destroy(GridInfo.Instance.gameObject);
        Destroy(TimeController.Instance.gameObject);
        Destroy(CropController.Instance.gameObject);
        Destroy(CurrencyController.Instance.gameObject);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
