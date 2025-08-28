using UnityEngine;

public class EnemySight : MonoBehaviour
{
    [Header("Sight")]
    public Transform eyeTransform;

    [SerializeField] bool defaultFacingRight = false;

    Transform player;
    EnemyDataManager _dataManager;
    EnemyData _enemyData;

    void Start()
    {
        _dataManager = GetComponent<EnemyDataManager>();
        _enemyData = _dataManager._enemyData;

        var p = FindAnyObjectByType<PlayerMove>();
        if (p != null) player = p.transform;
    }

    public bool IsPlayerInRange()
    {
        if (player == null) return false;

        Vector3 origin = eyeTransform ? eyeTransform.position : transform.position;

        float dist = Vector2.Distance(player.position, origin);
        float dx = player.position.x - origin.x;

        float forward = GetForwardSign();
        bool isFacing = (dx * forward) > 0f;

        return isFacing && dist <= _enemyData.SightRange;
    }
    public void SetDefaultFacingRight(bool v) { defaultFacingRight = v; }

    float GetForwardSign()
    {
        float scl = Mathf.Sign(transform.lossyScale.x);
        float baseDir = defaultFacingRight ? 1f : -1f;
        float f = scl * baseDir;
        return (f >= 0f) ? 1f : -1f;
    }
}
