using UnityEngine;
using UnityEngine.UI;

public class PlayerStatUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] Image _hpFillImage;
    [SerializeField] Image _skillFillImage;
    PlayerStat _playerStats;

    [Header("Lerp 속도")]
    [SerializeField] private float hpLerpSpeed = 5f;
    [SerializeField] private float skillLerpSpeed = 5f;


    private void Start()
    {
        _playerStats = FindAnyObjectByType<PlayerStat>();

        if (_playerStats == null)
        {
            Debug.LogWarning("PlayerStats 컴포넌트를 찾을 수 없습니다.");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J)) _playerStats.TakeDamage(10);
        if (Input.GetKeyDown(KeyCode.H)) _playerStats.Heal(10);

        SyncStatUI();
    }

    void SyncStatUI()
    {
        if (_playerStats == null) return;

        float _targetHPFill = _playerStats._currentHP / _playerStats._maxHP;
        float _targetSkillFill = _playerStats._currentSkillGauge / _playerStats._maxSkillGauge;

        _hpFillImage.fillAmount = Mathf.Lerp(_hpFillImage.fillAmount, _targetHPFill, Time.deltaTime * hpLerpSpeed);
        _skillFillImage.fillAmount = Mathf.Lerp(_skillFillImage.fillAmount, _targetSkillFill, Time.deltaTime * skillLerpSpeed);
    }

}
