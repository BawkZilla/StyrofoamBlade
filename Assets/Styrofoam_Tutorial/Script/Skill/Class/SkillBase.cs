using UnityEngine;

public abstract class SkillBase : MonoBehaviour, ISkill
{
    [SerializeField] protected float coolDown = 4f;

    protected float _nextReadyTime;

    public float CoolDown
    {
        get { return coolDown; }
    }

    public bool IsReady
    {
        get { return Time.time >= _nextReadyTime; }
    }

    public virtual void ExecuteSkill()
    {
        if (!IsReady)
        {
            print($"[{name}] Skill is not ready : {( _nextReadyTime - Time.time):0.00}s");
            return;
        }

        OnCast();
        _nextReadyTime = Time.time + coolDown;
    }

    protected abstract void OnCast(); 

    public float GetCooldownRemain()
    {
        return Mathf.Max(0f, _nextReadyTime - Time.time);
    }
}
