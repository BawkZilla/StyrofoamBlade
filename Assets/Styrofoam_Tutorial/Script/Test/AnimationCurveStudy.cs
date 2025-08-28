using UnityEngine;

public class AnimationCurveStudy : MonoBehaviour
{
    [SerializeField] AnimationCurve _linearCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    [SerializeField] AnimationCurve _easeInOutCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    [SerializeField] AnimationCurve _constantCurve = AnimationCurve.Constant(0f, 1f, 1f);
    [SerializeField] AnimationCurve _customCurve;

}
