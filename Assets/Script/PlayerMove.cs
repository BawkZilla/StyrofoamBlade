using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class PlayerMove : MonoBehaviour
{
    float _moveSpeed = 3.5f;
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
        float _xHor = Input.GetAxis("Horizontal");
        _isRunning = Input.GetButton("Sprint");
        _rb.linearVelocity = new Vector2(_xHor * _moveSpeed, 0f);

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
}
