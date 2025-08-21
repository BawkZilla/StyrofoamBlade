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
        // 매니저가 비활성화될 때 원복 안전장치
        SetTimeScaleInternal(1f);
    }

    /// <summary>
    /// 특정 timeScale을 holdSeconds(초, 실시간) 동안 유지한 뒤, recoverSeconds 동안 1로 감쇠.
    /// </summary>
    public void SetScaleForThenRecover(float targetScale, float holdSeconds, float recoverSeconds)
    {
        if (_running != null) StopCoroutine(_running);
        _running = StartCoroutine(CoSetScaleForThenRecover(targetScale, holdSeconds, recoverSeconds, _recoverEase));
    }

    /// <summary>
    /// 커스텀 이징 커브 버전
    /// </summary>
    public void SetScaleForThenRecover(float targetScale, float holdSeconds, float recoverSeconds, AnimationCurve recoverEase)
    {
        if (_running != null) StopCoroutine(_running);
        _running = StartCoroutine(CoSetScaleForThenRecover(targetScale, holdSeconds, recoverSeconds, recoverEase));
    }

    /// <summary>
    /// 진행 중인 감쇠를 취소하고 즉시 1로 복귀
    /// </summary>
    public void CancelAndReset()
    {
        if (_running != null) StopCoroutine(_running);
        _running = null;
        SetTimeScaleInternal(1f);
    }

    IEnumerator CoSetScaleForThenRecover(float targetScale, float holdSeconds, float recoverSeconds, AnimationCurve ease)
    {
        // 0은 업데이트 정지 위험. 너무 낮으면 최소값으로 클램프.
        targetScale = Mathf.Clamp(targetScale, 0.01f, 100f);

        // 1) 유지 구간
        SetTimeScaleInternal(targetScale);
        if (holdSeconds > 0f)
        {
            float t = 0f;
            while (t < holdSeconds)
            {
                t += Time.unscaledDeltaTime; // timeScale의 영향을 받지 않도록
                yield return null;
            }
        }

        // 2) 복귀 구간 (targetScale -> 1)
        if (recoverSeconds > 0f)
        {
            float t = 0f;
            while (t < recoverSeconds)
            {
                t += Time.unscaledDeltaTime;
                float p = Mathf.Clamp01(t / recoverSeconds);       // 0→1
                float k = ease != null ? ease.Evaluate(p) : p;     // 보간 계수
                float s = Mathf.Lerp(targetScale, 1f, k);
                SetTimeScaleInternal(s);
                yield return null;
            }
        }

        // 3) 마무리
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
