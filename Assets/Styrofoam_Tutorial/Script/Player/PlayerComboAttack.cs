using UnityEngine;

[System.Serializable]
public class ComboStep
{
    public string StateName;
    public float MaxStepTime = 0.8f; 
    public float ComboTimingStart = 0.3f; 
    public float ComboTimingEnd = 0.7f; 
}

public class PlayerComboAttack : MonoBehaviour
{
    [SerializeField] ComboStep[] _steps;


    [SerializeField] float _inputBufferTime = 0.2f;
    Animator _anim;

    int _currentCombo = -1;
    float _comboStartTime;
    bool _queuedNextCombo;
    float _lastInputTime;

    MeleeAttackManager _meleeAttack;

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _meleeAttack = GetComponentInChildren<MeleeAttackManager>();
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            _lastInputTime = Time.time;
            StartComboAttack();
        }

        if (_currentCombo >= 0)
            UpdateCombo();
    }

    public void StartMeleeAttack() => _meleeAttack.BeginAttack();
    public void EndMeleeAttack() => _meleeAttack.EndAttack();

    void StartComboAttack()
    {
        if (_currentCombo < 0)
        {
            StartCombo(0);
            return;
        }

        var step = _steps[_currentCombo];
        float elapsed = Time.time - _comboStartTime;

        bool isInTiming = elapsed >= step.ComboTimingStart && elapsed <= step.ComboTimingEnd;
        bool withinBuffer = Time.time - _lastInputTime <= _inputBufferTime;

        if (isInTiming && withinBuffer)
            _queuedNextCombo = true;
    }

    void StartCombo(int index)
    {
        _currentCombo = Mathf.Clamp(index, 0, _steps.Length - 1);
        _comboStartTime = Time.time;
        _queuedNextCombo = false;

        _anim.CrossFade(_steps[_currentCombo].StateName, 0.05f);
    }

    void UpdateCombo()
    {
        var step = _steps[_currentCombo];
        float elapsed = Time.time - _comboStartTime;

        if (_queuedNextCombo && elapsed >= step.ComboTimingStart)
        {
            int next = _currentCombo + 1;
            if (next < _steps.Length)
                StartCombo(next);
            else
                ResetCombo();
            return;
        }

        if (elapsed >= step.MaxStepTime)
            ResetCombo();
    }

    void ResetCombo()
    {
        _currentCombo = -1;
        _queuedNextCombo = false;
    }
}
