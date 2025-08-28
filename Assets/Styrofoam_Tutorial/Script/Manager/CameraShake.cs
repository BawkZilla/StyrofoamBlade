using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] CinemachineCamera _vcam;
    [SerializeField] AnimationCurve _shakeCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);

    CinemachineBasicMultiChannelPerlin _perlin;
    Coroutine _running;

    float _baseAmp, _baseFreq;

    void Awake()
    {
        _perlin = _vcam.GetComponent<CinemachineBasicMultiChannelPerlin>();

        _baseAmp = _perlin.AmplitudeGain;
        _baseFreq = _perlin.FrequencyGain;
    }

    public void Shake(float targetAmplitude, float duration)
    {
        if (_running != null) StopCoroutine(_running);
        _running = StartCoroutine(CoShake(targetAmplitude, duration));
    }

    IEnumerator CoShake(float targetAmp, float duration)
    {
        float startAmp = targetAmp;

        _perlin.AmplitudeGain = startAmp;

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float p = Mathf.Clamp01(t / duration);
            float k = _shakeCurve.Evaluate(p);

            _perlin.AmplitudeGain = startAmp * k;
            _perlin.FrequencyGain = Mathf.Max(0.2f, k);
            yield return null;
        }

        _perlin.AmplitudeGain = 0f;
        _perlin.FrequencyGain = _baseFreq;
        _running = null;
    }

    void OnDisable()
    {
        if (_perlin != null)
        {
            _perlin.AmplitudeGain = _baseAmp;
            _perlin.FrequencyGain = _baseFreq;
        }
    }
}
