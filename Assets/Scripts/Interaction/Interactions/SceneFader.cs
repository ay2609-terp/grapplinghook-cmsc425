using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneFader : MonoBehaviour
{
    public static SceneFader Instance;

    public Image fadeImage;
    public float fadeDuration = 0.75f;

    private bool isTransitioning;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SetAlpha(0f);
    }

    public void LoadSceneWithFade(string sceneName)
    {
        if (isTransitioning)
        {
            return;
        }

        StartCoroutine(LoadSceneRoutine(sceneName));
    }

    private IEnumerator LoadSceneRoutine(string sceneName)
    {
        isTransitioning = true;

        yield return FadeTo(1f);

        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName);
        loadOperation.allowSceneActivation = false;

        while (loadOperation.progress < 0.9f)
        {
            yield return null;
        }

        loadOperation.allowSceneActivation = true;

        while (!loadOperation.isDone)
        {
            yield return null;
        }

        yield return null;

        yield return FadeTo(0f);

        isTransitioning = false;
    }

    private IEnumerator FadeTo(float targetAlpha)
    {
        float startAlpha = fadeImage.color.a;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;

            float t = elapsedTime / fadeDuration;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, t);

            SetAlpha(newAlpha);

            yield return null;
        }

        SetAlpha(targetAlpha);
    }

    private void SetAlpha(float alpha)
    {
        Color color = fadeImage.color;
        color.a = alpha;
        fadeImage.color = color;
    }
}