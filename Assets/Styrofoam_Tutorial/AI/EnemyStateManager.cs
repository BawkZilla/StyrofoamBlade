using UnityEngine;

public class EnemyStateManager : MonoBehaviour
{
    public IEnemyState CurrentState;
    public EnemyData EnemyData;

    public Transform Avatar;
    [HideInInspector]
    public Transform Player;
    [HideInInspector]
    public Rigidbody2D _rb;

    float _maxHP, _currentHP;





    void Start()
    {
        TransitionToState(new IdleState());
        AllocateComponents();
    }

    void AllocateComponents()
    {
        _rb = GetComponent<Rigidbody2D>();
        Player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        CurrentState?.UpdateState(this);
    }

    private void FixedUpdate()
    {
        _rb.AddForce(Vector2.down * 30);
    }

    public void TransitionToState(IEnemyState newState)
    {
        CurrentState?.ExitState(this);
        CurrentState = newState;
        CurrentState.EnterState(this);
        print($"[TransitionToState ] State transitioned to {newState}");
    }
}
