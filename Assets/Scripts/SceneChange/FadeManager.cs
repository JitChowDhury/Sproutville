using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeManager : MonoBehaviour
{
    public static FadeManager Instance;
    [SerializeField] private CanvasGroup fadeGroup;
    [SerializeField] private float fadeDuration = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);//destroys if more than one exists
        }
    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += onSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= onSceneLoaded;

    }


    private IEnumerator FadeOutAndLoad(string sceneName)
    {
        yield return Fade(0f, 1f);
        SceneManager.LoadScene(sceneName);
    }
    public void FadeAndLoad(string sceneName)
    {
        StartCoroutine(FadeOutAndLoad(sceneName));
    }
    private void onSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(Fade(1f, 0f));
    }

    private IEnumerator Fade(float from, float to)
    {
        float t = 0f;
        fadeGroup.alpha = from;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeGroup.alpha = Mathf.Lerp(from, to, t / fadeDuration);
            yield return null;
        }
        fadeGroup.alpha = to;
    }

}
