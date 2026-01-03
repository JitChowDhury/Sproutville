using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance;

    [Header("UI")]
    public GameObject dialogueBox;
    public TextMeshProUGUI dialogueText;

    [Header("Timing")]
    public float typingSpeed = 0.04f;
    public float lineDelay = 1.2f;

    public bool IsPlaying { get; private set; }

    Coroutine dialogueRoutine;

    void Awake()
    {
        Instance = this;
        dialogueBox.SetActive(false);
    }

    public void PlayLines(string[] lines)
    {
        if (dialogueRoutine != null)
            StopCoroutine(dialogueRoutine);

        dialogueRoutine = StartCoroutine(PlayDialogue(lines));
    }

    IEnumerator PlayDialogue(string[] lines)
    {
        IsPlaying = true;
        dialogueBox.SetActive(true);

        foreach (string line in lines)
        {
            yield return StartCoroutine(TypeLine(line));
            yield return new WaitForSeconds(lineDelay);
        }

        dialogueBox.SetActive(false);
        IsPlaying = false;
    }

    IEnumerator TypeLine(string line)
    {
        dialogueText.text = "";

        foreach (char c in line)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
