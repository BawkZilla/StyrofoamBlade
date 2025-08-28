using UnityEngine;

public class EnemyStateManager : MonoBehaviour
{
    public IEnemyState CurrentState;

    public Transform Avatar;
    [HideInInspector]
    public Transform Player;
    [HideInInspector]
    public Rigidbody2D _rb;

    EnemySight _sight;

    float _maxHP, _currentHP;

    void Start()
    {
        TransitionToState(new IdleState());
        AllocateComponents();
    }

    void AllocateComponents()
    {
        _sight = GetComponent<EnemySight>();
        _rb = GetComponent<Rigidbody2D>();
        Player = FindAnyObjectByType<PlayerMove>()?.transform;
    }

    void Update()
    {
        CurrentState?.UpdateState(this);
        _sight?.SetDefaultFacingRight(Player.position.x > (Avatar ? Avatar.position.x : transform.position.x));

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
