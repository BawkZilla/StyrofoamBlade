using UnityEngine;
public class EnemyStat : StatBehaviour
{
    [Header("HP")]
    public float _maxHP = 100F;
    public float _currentHP;

    MeleeAttackManager _meleeAttack;

    private void Awake()
    {
        _currentHP = _maxHP;
        _meleeAttack = GetComponentInChildren<MeleeAttackManager>();
    }

    public void StartMeleeAttack() => _meleeAttack.BeginAttack();
    public void EndMeleeAttack() => _meleeAttack.EndAttack();
    public override void TakeDamage(float amount)
    {
        _currentHP = Mathf.Max(_currentHP - amount, 0f);
        if (_currentHP <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        GetComponent<Animator>().Play("Die");
        Destroy(gameObject, 1f);
    }

    public void Heal(float amount)
    {
        _currentHP = Mathf.Min(_currentHP + amount, _maxHP);
    }
}
