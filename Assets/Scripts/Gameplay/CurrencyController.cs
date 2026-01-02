using UnityEngine;

public class CurrencyController : MonoBehaviour
{
    public static CurrencyController Instance;
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

    void Start()
    {
        UIController.Instance.UpdateMoneyText(currentMoney);

    }

    public float currentMoney;
    public void SpendMoney(float amountToSpend)
    {
        currentMoney -= amountToSpend;
        UIController.Instance.UpdateMoneyText(currentMoney);

    }

    public void AddMoney(float amountToAdd)
    {
        currentMoney += amountToAdd;
        UIController.Instance.UpdateMoneyText(currentMoney);

    }

    public bool CheckMoney(float amount)
    {
        if (currentMoney >= amount)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
