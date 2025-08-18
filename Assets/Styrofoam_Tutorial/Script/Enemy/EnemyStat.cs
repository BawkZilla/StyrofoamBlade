using UnityEngine;
public class EnemyStat : StatBehaviour
{
    MeleeAttackManager _meleeAttack;

    private void Awake()
    {
        CurrentHP = MaxHP;
        _meleeAttack = GetComponentInChildren<MeleeAttackManager>();
    }

    public void StartMeleeAttack() => _meleeAttack.BeginAttack();
    public void EndMeleeAttack() => _meleeAttack.EndAttack();
    public override void TakeDamage(float amount)
    {
        CurrentHP = Mathf.Max(CurrentHP - amount, 0f);
        if (CurrentHP <= 0f)
        {
            Die();
        }
    }

    public override void Die()
    {
        GetComponent<Animator>().Play("Die");
        Destroy(gameObject, 1f);
    }

    public void Heal(float amount)
    {
        CurrentHP = Mathf.Min(CurrentHP + amount, MaxHP);
    }
}
