using UnityEngine;
using UnityEngine.UI;

public class StatUI : MonoBehaviour
{
    [Header("UI Reference")]
    [SerializeField] Image _hpFillImage;
    [SerializeField] Image _skillFillImage;

    [SerializeField] StatBehaviour _stat;

    void Update()
    {
        SyncStatUI();
    }

    void SyncStatUI()
    {
        if (_stat == null) return;

        float _targetHPFill = _stat.CurrentHP / _stat.MaxHP;

        _hpFillImage.fillAmount = Mathf.Lerp(_hpFillImage.fillAmount, _targetHPFill, Time.deltaTime * 5);

        //Conditional statement to check if _skillFillImage is null
        if (_skillFillImage)
        {
            float _targetSkillFill = _stat.CurrentSkillGauge / _stat.MaxSkillGauge;
            _skillFillImage.fillAmount = Mathf.Lerp(_skillFillImage.fillAmount, _targetSkillFill, Time.deltaTime * 5);
        }
    }
}
