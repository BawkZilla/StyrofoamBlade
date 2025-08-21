using System.Collections;
using UnityEngine;

public class TimeScaleManager : MonoBehaviour
{
    public static TimeScaleManager Instance { get; private set; }

    [Header("Options")]
    [SerializeField] bool _scaleFixedDeltaTime = true;
    [SerializeField] AnimationCurve _recoverEase = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

    float _baseFixedDelta;
    Coroutine _running;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        _baseFixedDelta = Time.fixedDeltaTime;
    }

    void OnDisable()
    {
        // �Ŵ����� ��Ȱ��ȭ�� �� ���� ������ġ
        SetTimeScaleInternal(1f);
    }

    /// <summary>
    /// Ư�� timeScale�� holdSeconds(��, �ǽð�) ���� ������ ��, recoverSeconds ���� 1�� ����.
    /// </summary>
    public void SetScaleForThenRecover(float targetScale, float holdSeconds, float recoverSeconds)
    {
        if (_running != null) StopCoroutine(_running);
        _running = StartCoroutine(CoSetScaleForThenRecover(targetScale, holdSeconds, recoverSeconds, _recoverEase));
    }

    /// <summary>
    /// Ŀ���� ��¡ Ŀ�� ����
    /// </summary>
    public void SetScaleForThenRecover(float targetScale, float holdSeconds, float recoverSeconds, AnimationCurve recoverEase)
    {
        if (_running != null) StopCoroutine(_running);
        _running = StartCoroutine(CoSetScaleForThenRecover(targetScale, holdSeconds, recoverSeconds, recoverEase));
    }

    /// <summary>
    /// ���� ���� ���踦 ����ϰ� ��� 1�� ����
    /// </summary>
    public void CancelAndReset()
    {
        if (_running != null) StopCoroutine(_running);
        _running = null;
        SetTimeScaleInternal(1f);
    }

    IEnumerator CoSetScaleForThenRecover(float targetScale, float holdSeconds, float recoverSeconds, AnimationCurve ease)
    {
        // 0�� ������Ʈ ���� ����. �ʹ� ������ �ּҰ����� Ŭ����.
        targetScale = Mathf.Clamp(targetScale, 0.01f, 100f);

        // 1) ���� ����
        SetTimeScaleInternal(targetScale);
        if (holdSeconds > 0f)
        {
            float t = 0f;
            while (t < holdSeconds)
            {
                t += Time.unscaledDeltaTime; // timeScale�� ������ ���� �ʵ���
                yield return null;
            }
        }

        // 2) ���� ���� (targetScale -> 1)
        if (recoverSeconds > 0f)
        {
            float t = 0f;
            while (t < recoverSeconds)
            {
                t += Time.unscaledDeltaTime;
                float p = Mathf.Clamp01(t / recoverSeconds);       // 0��1
                float k = ease != null ? ease.Evaluate(p) : p;     // ���� ���
                float s = Mathf.Lerp(targetScale, 1f, k);
                SetTimeScaleInternal(s);
                yield return null;
            }
        }

        // 3) ������
        SetTimeScaleInternal(1f);
        _running = null;
    }

    void SetTimeScaleInternal(float scale)
    {
        Time.timeScale = scale;
        if (_scaleFixedDeltaTime)
            Time.fixedDeltaTime = _baseFixedDelta * scale;
    }
}
