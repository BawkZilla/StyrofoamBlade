using UnityEngine;
using UnityEngine.UI;

public class EnemyStatUI : MonoBehaviour
{
    [Header("UI Reference")]
    [SerializeField] Image _hpFillImage;
    EnemyStat _enemyStat;

    private void Start()
    {
        _enemyStat = GetComponentInParent<EnemyStat>();
        if(_enemyStat == null)
        {
            Debug.LogWarning("Can't find EnemyStat Component");
        }
    }

    void Update()
    {
        SyncStatUI();
    }

    void SyncStatUI()
    {
        if (_enemyStat == null) return;

        float _targetHPFill = _enemyStat._currentHP / _enemyStat._maxHP;

        _hpFillImage.fillAmount = Mathf.Lerp(_hpFillImage.fillAmount, _targetHPFill, Time.deltaTime * 5);
    }
}
