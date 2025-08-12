using UnityEngine;

public class EnemySight : MonoBehaviour
{
    [Header("Sight")]
    public Transform eyeTransform;

    EnemyDataManager _dataManager;
    EnemyData EnemyData;

    void Start()
    {
        _dataManager = GetComponent<EnemyDataManager>();
        EnemyData = _dataManager._enemyData;
    }

    private void FixedUpdate()
    {
        if (IsPlayerInRange()) return;
    }

    public bool IsPlayerInRange()
    {
        Vector2 origin = transform.position;
        LayerMask playerMask = LayerMask.GetMask("Player");
        Collider2D hit = null;

        if (EnemyData.EnemySightType == E_SightType.Box)
        {
            hit = Physics2D.OverlapBox(origin + GetForwardOffset(), EnemyData.SightBoxSize, 0f, playerMask);
        }
        else if (EnemyData.EnemySightType == E_SightType.Circle)
        {
            hit = Physics2D.OverlapCircle(origin + GetForwardOffset(), EnemyData.SightRadius, playerMask);
        }
        if (hit != null)
        {
            bool inFOV = IsTargetInSightAngle(hit.transform, EnemyData.SightAngle);
            bool hasLOS = HasLineOfSight(hit.transform);

            return inFOV && hasLOS;
        }

        return false;
    }

    bool HasLineOfSight(Transform target)
    {
        Vector2 eye = GetEyePosition();
        Vector2 targetPos = target.position;

        RaycastHit2D hit = Physics2D.Linecast(eye, targetPos);

        if (hit.collider == null)
            return true;

        if (hit.collider.gameObject == gameObject)
            return true;

        if (hit.collider.transform == target)
            return true;

        return false;
    }

    private Vector2 GetEyePosition()
    {
        if (eyeTransform != null)
        {
            Debug.DrawLine(transform.position, eyeTransform.position, Color.magenta);
            return eyeTransform.position;
        }

        return transform.position;
    }

    Vector2 GetForwardOffset()
    {
        float offset = EnemyData.EnemySightType == E_SightType.Box
            ? EnemyData.SightBoxSize.x * 0.5f
            : EnemyData.SightRadius;

        Vector2 dir = transform.localScale.x >= 0 ? Vector2.right : Vector2.left;
        return dir * offset * 0.8f;
    }

    bool IsTargetInSightAngle(Transform target, float fov)
    {
        Vector2 toTarget = ((Vector2)target.position + GetEyePosition()).normalized;
        Vector2 forward = transform.localScale.x >= 0 ? Vector2.right : Vector2.left;

        float dot = Vector2.Dot(forward, toTarget);
        float cosThreshold = Mathf.Cos(fov * 0.5f * Mathf.Deg2Rad);
        return dot >= cosThreshold;
    }


    private void OnDrawGizmos()
    {
        if (!Application.isPlaying || EnemyData == null || eyeTransform == null) return;

        Collider2D player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Collider2D>();

        if (player != null)
        {
            Vector2 eye = GetEyePosition();
            Vector2 target = player.transform.position;

            RaycastHit2D hit = Physics2D.Linecast(eye, target);
            Gizmos.color = hit.collider == null ? Color.green : Color.gray;
            Gizmos.DrawLine(eye, target);
        }
    }


    private void OnDrawGizmosSelected()
    {
        if (EnemyData == null || eyeTransform == null)
            return;

        Vector2 origin = GetEyePosition();

        Gizmos.color = Color.red;
        if (EnemyData.EnemySightType == E_SightType.Box)
        {
            Gizmos.DrawWireCube(origin + GetForwardOffset(), EnemyData.SightBoxSize);
        }
        else if (EnemyData.EnemySightType == E_SightType.Circle)
        {
            Gizmos.DrawWireSphere(origin - GetForwardOffset(), EnemyData.SightRadius);
        }

        float angle = EnemyData.SightAngle;
        float viewDist = EnemyData.SightRadius;
        Vector2 forward = transform.localScale.x >= 0 ? Vector2.right : Vector2.left;

        Vector2 leftDir = Quaternion.Euler(0, 0, -angle / 2f) * forward;
        Vector2 rightDir = Quaternion.Euler(0, 0, angle / 2f) * forward;

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(origin, origin + leftDir * viewDist);
        Gizmos.DrawLine(origin, origin + rightDir * viewDist);
    }

}
