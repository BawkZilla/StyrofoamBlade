using UnityEngine;

public class PlayerStat : StatBehaviour
{

    private void Awake()
    {
        CurrentHP = MaxHP;
        CurrentSkillGauge = 0;
    }

    public override void TakeDamage(float amount)
    {
        CurrentHP = Mathf.Max(CurrentHP - amount, 0f);
        if(CurrentHP <= 0f)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        CurrentHP = Mathf.Min(CurrentHP + amount, MaxHP);
    }

    public override void Die()
    {
        print("Die");
    }
}
