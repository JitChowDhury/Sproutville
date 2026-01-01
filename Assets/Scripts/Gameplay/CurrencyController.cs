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

    public float currentMoney;
    public void SpendMoney(float amountToSpend)
    {
        currentMoney -= amountToSpend;
    }

    public void AddMoney(float amountToAdd)
    {
        currentMoney += amountToAdd;
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
