using UnityEngine;

public abstract class SkillBase : MonoBehaviour, ISkill
{
    [SerializeField] float coolDown = 3f;

    float _nextReadyTime;

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
}
