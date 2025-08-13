using UnityEngine;

public class ChaseState : IEnemyState
{
    float _chaseTime = 2f;
    float _chaseTimer = 0f;
    public void EnterState(EnemyStateManager enemy)
    {
        enemy.GetComponent<Animator>().Play("Chase");
        _chaseTimer = 0;
    }

    public void ExitState(EnemyStateManager enemy)
    {
        Debug.Log("[Chase State] : State Exited");
    }

    public void UpdateState(EnemyStateManager enemy)
    {
        _chaseTimer += Time.deltaTime;
        Transform _player = enemy.Player;

        bool _playerVisible = enemy.GetComponent<EnemySight>().IsPlayerInRange();
        if (_playerVisible) _chaseTimer = 0f;

        if(!_playerVisible && _chaseTimer >= _chaseTime)
        {
            if (Random.value < .5f)
                enemy.TransitionToState(new IdleState());
            else
                enemy.TransitionToState(new PatrolState());
            return;
        }

        if (_player != null)
        {
            Vector2 dir = (_player.position - enemy.transform.position).normalized;
            enemy._rb.linearVelocity = new Vector2(dir.x * enemy.GetComponent<EnemyDataManager>()._enemyData.ChaseSpeed, 0f);

            if(dir.x != 0)
            {
                Vector3 scale = enemy.transform.localScale;
                scale.x = -Mathf.Sign(dir.x) * Mathf.Abs(scale.x);
                enemy.transform.localScale = scale;
            }

            float dist = Vector2.Distance(enemy.transform.position, _player.position);
            if(dist <= enemy.GetComponent<EnemyDataManager>()._enemyData.AttackRange)
            {
                enemy._rb.linearVelocity = Vector2.zero;
                enemy.TransitionToState(new AttackState());
            }
        }

    }
}
