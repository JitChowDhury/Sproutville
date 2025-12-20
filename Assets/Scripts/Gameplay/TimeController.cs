using UnityEngine;

public class TimeController : MonoBehaviour
{
    public static TimeController Instance;

    [SerializeField] private float currentTime;
    [SerializeField] private float timeSpeed = .15f;
    [SerializeField] private float dayStart, dayEnd;


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
        currentTime = dayStart;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime * timeSpeed;

        if (currentTime > dayEnd)
        {
            currentTime = dayEnd;
        }

        if (UIController.Instance != null)
        {
            UIController.Instance.UpdateTimeText(currentTime);
        }
    }
}
