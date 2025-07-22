using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class PlayerMove : MonoBehaviour
{
    float _moveSpeed = 3.5f;
    float _animSpeed = 0f;
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


    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }

    void Update()
    {
        GroundCheck();

        float _xHor = Input.GetAxis("Horizontal");
        _isRunning = Input.GetButton("Sprint");

        if (Input.GetButtonDown("Jump")) Jump();

        _rb.linearVelocity = new Vector2(_xHor * _moveSpeed * (_isRunning ? 2 : 1), _rb.linearVelocity.y);

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

    private void FixedUpdate()
    {
        AddDownForce();
    }

    void GroundCheck()
    {
        _isGrounded = Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, _groundLayer);
        _anim.SetBool("_isAir", !_isGrounded);
    }

    private void OnDrawGizmosSelected()
    {
        if(_groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckRadius);
        }
    }

    void Jump()
    {
        _rb.AddForce(Vector2.up * _jumpForce , ForceMode2D.Impulse);
    }
    void AddDownForce()
    {
        _rb.AddForce(Vector2.down * _downForce);
    }
}
