using System.Collections;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class PlayerMove : MonoBehaviour
{
    float _xHor;
    float _animSpeed = 0f;
    [SerializeField] float _moveSpeed = 3.5f;
    [SerializeField] float _downForce = 10f;
    [SerializeField] float _jumpForce = 5f;
    bool _isRunning = false;
    Rigidbody2D _rb;
    Animator _anim;

    [Header("GroundCheck")]
    [SerializeField] Transform _groundCheck;
    [SerializeField] float _groundCheckRadius = .2f;
    [SerializeField] LayerMask _groundLayer;
    bool _isGrounded = false;

    [Header("Roll")]
    [SerializeField] float _rollPower = 15f;
    [SerializeField] float _rollDuration = .4f;
    [SerializeField] float _rollCooldown = .5f;

    int _playerDir = 1;

    bool _isRolling = false;
    bool _canRoll = true;


    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }

    void Update()
    {
        GroundCheck();
        Move();
        Roll();
        Jump();
    }

    private void FixedUpdate()
    {
        AddDownForce();
    }

    void GroundCheck()
    {
        _isGrounded = Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, _groundLayer);
        _anim.SetBool("_isAir", !_isGrounded);
    }

    void Move()
    {
        if (_isRolling) return;

        _xHor = Input.GetAxis("Horizontal");
        _isRunning = Input.GetButton("Sprint");
        _rb.linearVelocity = new Vector2(_xHor * _moveSpeed * (_isRunning ? 2 : 1), _rb.linearVelocity.y);

        if (_xHor > 0)
        {
            _playerDir = 1;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (_xHor < 0)
        {
            _playerDir = -1;
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        if (_xHor != 0)
        {
            _animSpeed = _isRunning ? 1f : 0.5f;
        }
        else
        {
            _animSpeed = 0f;
        }
        _anim.SetFloat("_speed", _animSpeed);
    }

    void Roll()
    {
        if(Input.GetKeyDown(KeyCode.LeftControl) && _canRoll)
        {
            StartCoroutine(RollCoroutine());
        }
    }

    IEnumerator RollCoroutine()
    {
        _anim.Play("Roll");

        _isRolling = true;
        _canRoll = false;

        _rb.linearVelocityX = _playerDir * _rollPower;

        yield return new WaitForSeconds(_rollDuration);

        _rb.linearVelocity = Vector2.zero;
        _isRolling = false;

        yield return new WaitForSeconds(_rollCooldown);

        _canRoll = true;

    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            _rb.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        }
    }
    void AddDownForce()
    {
        _rb.AddForce(Vector2.down * _downForce);
    }

    private void OnDrawGizmosSelected()
    {
        if (_groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckRadius);
        }
    }
}
