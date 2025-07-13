using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class PlayerMove : MonoBehaviour
{
    float _moveSpeed = 3.5f;
    Rigidbody2D _rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float _xHor = Input.GetAxis("Horizontal");
        float _yVer = Input.GetAxis("Vertical");
        _rb.linearVelocity = new Vector2(_xHor * _moveSpeed, _yVer * _moveSpeed); 
    }
}
