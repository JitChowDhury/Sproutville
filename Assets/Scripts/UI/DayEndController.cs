using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DayEndController : MonoBehaviour
{
    [SerializeField] private TMP_Text dayText;
    [SerializeField] private string wakeUpString;
    void Start()
    {
        if (TimeController.Instance != null)
        {
            dayText.text = "- Day " + TimeController.Instance.currentDay + " -";
        }
    }

    void Update()
    {
        if (Keyboard.current.anyKey.wasPressedThisFrame || Mouse.current.leftButton.wasPressedThisFrame || Mouse.current.rightButton.wasPressedThisFrame)
        {
            TimeController.Instance.StartDay();
            SceneManager.LoadScene(wakeUpString);
        }
    }



}
