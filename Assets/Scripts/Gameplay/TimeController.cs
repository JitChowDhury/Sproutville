using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeController : MonoBehaviour
{
    public static TimeController Instance;

    [Header("Time Settings")]
    [SerializeField] private float dayStart = 7f;
    [SerializeField] private float dayEnd = 26f;
    [SerializeField] private float timeSpeed = 0.15f;

    [Header("State")]
    [SerializeField] private float currentTime;
    [SerializeField] private bool timeActive = true;

    [Header("Day Settings")]
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

    private void Start()
    {
        StartDay();
    }

    private void Update()
    {
        if (!timeActive) return;

        currentTime += Time.deltaTime * timeSpeed;

        if (currentTime >= dayEnd)
        {
            currentTime = dayEnd;
            EndDay();
        }

        if (UIController.Instance != null)
        {
            UIController.Instance.UpdateTimeText(currentTime);
        }
    }

    public void StartDay()
    {
        currentTime = dayStart;
        timeActive = true;
    }

    public void EndDay()
    {
        timeActive = false;
        currentDay++;

        if (GridInfo.Instance != null)
        {
            GridInfo.Instance.GrowCrop();
        }

        PlayerPrefs.SetString("TransitionName", "Wake Up");

        SceneManager.LoadScene(dayEndScene);
    }

    // 🔑 THIS IS WHAT LIGHTING USES
    public float GetDayNormalizedTime()
    {
        return Mathf.Clamp01(
            Mathf.InverseLerp(dayStart, dayEnd, currentTime)
        );
    }

    // Optional helpers
    public bool IsNight()
    {
        float t = GetDayNormalizedTime();
        return t >= 0.75f;
    }

    public void PauseTime() => timeActive = false;
    public void ResumeTime() => timeActive = true;
}
