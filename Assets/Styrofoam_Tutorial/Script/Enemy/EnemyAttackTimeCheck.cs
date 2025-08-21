using UnityEngine;

public class EnemyAttackTimeCheck : MonoBehaviour
{
    [SerializeField] Vector2 _parryingSuccessTimeRange;
    [SerializeField] float _normalGuardTime;
    public bool isParrySuccess = false;
    public bool isNormalGuard = false;
    public void TimeCheck()
    {
        isParrySuccess = false;
        isNormalGuard = false;

        var guardingTime = FindAnyObjectByType<PlayerGuard>().GuardingTime;
        if(guardingTime >= _parryingSuccessTimeRange.x && guardingTime <= _parryingSuccessTimeRange.y)
        {
            isParrySuccess = true;
        }
        else if (guardingTime > _normalGuardTime)
        {
            isNormalGuard = true;
        }
    }
}
