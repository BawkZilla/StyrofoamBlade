using UnityEngine;

public abstract class StatBehaviour : MonoBehaviour
{
    public float CurrentHP;
    public float MaxHP;

    public float CurrentSkillGauge;
    public float MaxSkillGauge;
    public abstract void TakeDamage(float amount);
    public abstract void Die();
}
