using UnityEngine;

public class StunState : IEnemyState
{
    float _idleTime = 2f;
    float _timer = 0f;

    public void EnterState(EnemyStateManager enemy)
    {
        enemy.GetComponent<Animator>().SetTrigger("Stun");
        _timer = 0f;
    }

    public void ExitState(EnemyStateManager enemy)
    {
        enemy.GetComponent<EnemyAttackTimeCheck>().isParrySuccess = false;
        enemy.GetComponent<EnemyAttackTimeCheck>().isNormalGuard = false;

    }

    public void UpdateState(EnemyStateManager enemy)
    {
        if (enemy.GetComponent<StatBehaviour>().CurrentHP <= 0)
        {
            return;
        }

        _timer += Time.deltaTime;
        if (_timer >= _idleTime)
        {
            enemy.TransitionToState(new IdleState());
            return;
        }
    }
}
