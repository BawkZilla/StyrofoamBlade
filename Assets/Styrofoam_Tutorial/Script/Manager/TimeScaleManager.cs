using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class TimeScaleManager : MonoBehaviour
{
    public static TimeScaleManager Instance { get; private set; }

    [SerializeField] AnimationCurve _recoverEase = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    Coroutine _running;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void OnDisable()
    {
        Time.timeScale = 1f;
    }

    public void SetScaleForThenRecover(float targetScale, float holdSeconds, float recoverSeconds)
    {
        if (_running != null) StopCoroutine(_running);
        _running = StartCoroutine(CoSetScaleForThenRecover(targetScale, holdSeconds, recoverSeconds, _recoverEase));
    }

    IEnumerator CoSetScaleForThenRecover(float targetScale, float holdSeconds, float recoverSeconds, AnimationCurve ease)
    {
        targetScale = Mathf.Clamp(targetScale, 0.01f, 100f);

        Time.timeScale = targetScale;

        if (holdSeconds > 0f)
        {
            float t = 0f;
            while (t < holdSeconds)
            {
                t += Time.unscaledDeltaTime;
                yield return null;
            }
        }

        if (recoverSeconds > 0f)
        {
            float t = 0f;
            while (t < recoverSeconds)
            {
                t += Time.unscaledDeltaTime;
                float p = Mathf.Clamp01(t / recoverSeconds);
                float k = ease != null ? ease.Evaluate(p) : p;
                float s = Mathf.Lerp(targetScale, 1f, k);
                Time.timeScale = s;
                yield return null;
            }
        }

        Time.timeScale = 1f;
        _running = null;
    }
}
