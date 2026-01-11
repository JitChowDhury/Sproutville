using UnityEngine;
using UnityEngine.InputSystem;

public class BedController : MonoBehaviour
{
    private bool canSleep;
    [SerializeField] private float sleepAllowedAfter = 20f;

    // Update is called once per frame
    void Update()
    {
        if (!canSleep)
            return;

        if (Keyboard.current.eKey.wasPressedThisFrame ||
            Keyboard.current.spaceKey.wasPressedThisFrame ||
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (TimeController.Instance == null)
                return;

            // Check if sleeping is allowed
            if (!CanSleepNow())
            {
                Debug.Log("You can only sleep at night.");
                return;
            }

            TimeController.Instance.EndDay();
        }
    }

    bool CanSleepNow()
    {
        if (TimeController.Instance == null)
            return false;

        float normalizedTime = TimeController.Instance.GetDayNormalizedTime();

        // Convert normalized time back to absolute time
        float absoluteTime = Mathf.Lerp(
            7f,
            26f,
            normalizedTime
        );

        return absoluteTime >= sleepAllowedAfter;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canSleep = true;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canSleep = false;
        }
    }
}
