using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeController : MonoBehaviour
{
    public static TimeController Instance;

    [SerializeField] private float currentTime;
    [SerializeField] private float dayStart, dayEnd;

    [SerializeField] private bool timeActive;
    [SerializeField] private float timeSpeed = .15f;
    [SerializeField] private string dayEndScene;
    public int currentDay = 1;


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
        timeActive = false;

    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime * timeSpeed;

        if (currentTime > dayEnd)
        {
            currentTime = dayEnd;
            EndDay();
        }

        if (UIController.Instance != null)
        {
            UIController.Instance.UpdateTimeText(currentTime);
        }
    }

    public void EndDay()
    {
        timeActive = false;
        currentDay++;
        GridInfo.Instance.GrowCrop();

        PlayerPrefs.SetString("TransitionName", "Wake Up");
        StartDay();
        SceneManager.LoadScene(dayEndScene);
    }
    public void StartDay()
    {
        timeActive = true;
        currentTime = dayStart;
    }
}
