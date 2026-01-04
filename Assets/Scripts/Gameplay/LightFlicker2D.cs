using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightFlicker2D : MonoBehaviour
{
    [Header("Light Reference")]
    [SerializeField] private Light2D light2D;

    [Header("Flicker Settings")]
    [SerializeField] private float minIntensity = 0.8f;
    [SerializeField] private float maxIntensity = 1.2f;
    [SerializeField] private float flickerSpeed = 5f;

    private float targetIntensity;

    void Start()
    {
        if (light2D == null)
            light2D = GetComponent<Light2D>();

        targetIntensity = Random.Range(minIntensity, maxIntensity);
    }

    void Update()
    {
        light2D.intensity = Mathf.Lerp(
            light2D.intensity,
            targetIntensity,
            Time.deltaTime * flickerSpeed
        );

        if (Mathf.Abs(light2D.intensity - targetIntensity) < 0.02f)
        {
            targetIntensity = Random.Range(minIntensity, maxIntensity);
        }
    }
}
