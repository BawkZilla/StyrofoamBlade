using UnityEngine;

public class EnemyGroundCheck : MonoBehaviour
{
    EnemyStateManager _stateManager;

    [Header("GroundChecker")]
    public LayerMask GroundLayer;
    public Transform GroundChecker;
    public float GroundCheckDistance = .2f;

    private void Start()
    {
        _stateManager = GetComponent<EnemyStateManager>();
    }

    void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    bool IsGroundAhead()
    {
        if (GroundChecker == null)
            return true;

        Vector2 origin = GroundChecker.position;
        Vector2 dir = transform.localScale.x >= 0 ? Vector2.right : Vector2.left;

        RaycastHit2D hit = Physics2D.Raycast(origin + dir * 0.2f, Vector2.down, GroundCheckDistance, GroundLayer);
        Debug.DrawLine(origin + dir * 0.2f, origin + dir * 0.2f + Vector2.down * GroundCheckDistance, hit.collider ? Color.green : Color.red);

        return hit.collider != null;
    }

}
