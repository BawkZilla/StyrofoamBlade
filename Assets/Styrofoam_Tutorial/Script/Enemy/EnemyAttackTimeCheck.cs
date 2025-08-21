using UnityEngine;

public class EnemyAttackTimeCheck : MonoBehaviour
{
    [SerializeField] Vector2 _parryingSucessTimeRange;
    [SerializeField] float _normalGuardTime;

    public bool isParrySuccess = false;
    public bool isNormalGuard = false;

    public void TimeCheck()
    {
        isParrySuccess = false;
        isNormalGuard = false;

        var guardingTime = FindAnyObjectByType<PlayerGuard>().GuardingTime;
        if(guardingTime >= _parryingSucessTimeRange.x && guardingTime <= _parryingSucessTimeRange.y)
        {
            isParrySuccess = true;
        }
        else if (guardingTime > _normalGuardTime)
        {
            isNormalGuard = true;
        }
    }
}
