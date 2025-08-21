using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] CinemachineCamera _vcam;
    [SerializeField] AnimationCurve _ease = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f); // 0~1 ¡æ 1~0
    [SerializeField] bool _useUnscaledTime = true;

    CinemachineBasicMultiChannelPerlin _perlin;
    Coroutine _running;

    float _baseAmp, _baseFreq;

    void Awake()
    {
        if (!_vcam) _vcam = GetComponent<CinemachineCamera>();
        _perlin = _vcam.GetComponent<CinemachineBasicMultiChannelPerlin>();
        if (_perlin == null) _perlin = _vcam.gameObject.AddComponent<CinemachineBasicMultiChannelPerlin>();

        _baseAmp = _perlin.AmplitudeGain;
        _baseFreq = _perlin.FrequencyGain;
    }

    void OnDisable()
    {
        if (_perlin != null)
        {
            _perlin.AmplitudeGain = _baseAmp;
            _perlin.FrequencyGain = _baseFreq;
        }
    }

    public void Shake(float targetAmplitude, float duration, float targetFrequency = -1f)
    {
        if (_running != null) StopCoroutine(_running);
        _running = StartCoroutine(CoShake(targetAmplitude, duration, targetFrequency));
    }

    IEnumerator CoShake(float targetAmp, float duration, float targetFreq)
    {
        float startAmp = targetAmp;
        float startFreq = (targetFreq < 0f) ? _perlin.FrequencyGain : targetFreq;

        _perlin.AmplitudeGain = startAmp;
        _perlin.FrequencyGain = startFreq;

        float t = 0f;
        while (t < duration)
        {
            float dt = _useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            t += dt;
            float p = Mathf.Clamp01(t / duration);
            float k = _ease.Evaluate(p);

            _perlin.AmplitudeGain = startAmp * k;
            _perlin.FrequencyGain = startFreq * Mathf.Max(0.2f, k);
            yield return null;
        }

        _perlin.AmplitudeGain = 0f;
        _perlin.FrequencyGain = _baseFreq;
        _running = null;
    }
}
