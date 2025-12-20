using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class DayNightLightController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Light2D globalLight;

    [Header("Day Night Settings")]
    [SerializeField] private Gradient lightColorOverDay;
    [SerializeField] private AnimationCurve intensityOverDay;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindGlobalLight();
    }

    private void FindGlobalLight()
    {
        globalLight = FindFirstObjectByType<Light2D>();

        if (globalLight == null)
        {
            Debug.LogWarning("DayNightLightController: No Global Light 2D found in scene.");
        }
    }

    private void Update()
    {
        if (TimeController.Instance == null || globalLight == null)
            return;

        float t = TimeController.Instance.GetDayNormalizedTime();

        globalLight.color = lightColorOverDay.Evaluate(t);
        globalLight.intensity = intensityOverDay.Evaluate(t);
    }
}
