using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class PlayerMove : MonoBehaviour
{
    float _moveSpeed = 3.5f;
    [SerializeField] float _downForce = 5f;
    [SerializeField] Transform _groundCheck;
    [SerializeField] float _groundCheckRadius = 0.2f;
    [SerializeField] LayerMask _groundLayer;
    bool _isGrounded = false;

    float _animSpeed = 0f;
    bool _isRunning = false;
    Rigidbody2D _rb;
    Animator _anim;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }

    void Update()
    {
        GroundCheck();
        if (Input.GetButtonDown("Jump") && _isGrounded) Jump();

        float _xHor = Input.GetAxis("Horizontal");
        _isRunning = Input.GetButton("Sprint");
        _rb.linearVelocity = new Vector2(_xHor * _moveSpeed, _rb.linearVelocity.y);
        _rb.linearVelocityX = _xHor * _moveSpeed * (_isRunning ? 2 : 1);

        if (_xHor > 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (_xHor < 0)
        {
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

    void Jump() => _rb.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

    private void FixedUpdate()
    {
        AddDownForce();
    }

    void GroundCheck()
    {
        _isGrounded = Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, _groundLayer);
        _anim.SetBool("_isAir", !_isGrounded);
    }

    void AddDownForce()
    {
        _rb.AddForce(Vector2.down * _downForce);
    }

    void OnDrawGizmosSelected()
    {
        if (_groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckRadius);
        }
    }
}
