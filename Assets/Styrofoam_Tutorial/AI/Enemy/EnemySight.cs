using UnityEngine;

public class EnemySight : MonoBehaviour
{
    [Header("Sight")]
    public Transform eyeTransform;

    Transform player;
    EnemyDataManager _dataManager;
    EnemyData EnemyData;

    void Start()
    {
        _dataManager = GetComponent<EnemyDataManager>();
        player = FindAnyObjectByType<PlayerMove>().transform;
        EnemyData = _dataManager._enemyData;
    }

    public bool IsPlayerInRange()
    {
        float dist = Vector2.Distance(player.position, transform.position);

        float dx = player.position.x - transform.position.x;
        float forward = -Mathf.Sign(transform.localScale.x);
        bool isFacing = dx * forward > 0f;

        return isFacing && dist <= GetComponent<EnemyDataManager>()._enemyData.SightRange;
    }
}
