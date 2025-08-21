using System.Collections.Generic;
using UnityEngine;

public class MeleeAttackManager : MonoBehaviour
{
    [Header("Set Hit Range")]
    public Vector2 boxSize = new Vector2(1.5f, 0.5f);
    public Vector2 boxOffset = new Vector2(1.0f, 0f); 
    public float boxAngle = 0f; 
    public float damage = 10f;

    List<GameObject> _hitObjs = new List<GameObject>();

    [HideInInspector]
    public bool isAttacking { get; private set; }

    void FixedUpdate()
    {
        if (!isAttacking) return;

        Vector2 center = (Vector2)transform.position + boxOffset;

        Collider2D[] hits = Physics2D.OverlapBoxAll(center, boxSize, boxAngle);

        foreach (var col in hits)
        {
            if (col.transform.root == transform.root) continue;

            var hp = col.GetComponent<StatBehaviour>();

            if (hp != null && !_hitObjs.Contains(col.gameObject))
            {
                float currentDamage = damage;

                _hitObjs.Add(col.gameObject);

                if(transform.root.TryGetComponent<EnemyAttackTimeCheck>(out var check))
                {
                    if (check.isParrySuccess)
                    {
                        Camera.main.GetComponent<CameraShake>().Shake(5f, .3f);
                        TimeScaleManager.Instance.SetScaleForThenRecover(.2f, .5f, .5f);
                        EffectManager.instance.SpawnEffect(E_EffectType.Parry, col.transform.position);
                        currentDamage = 0f;
                        GetComponentInParent<EnemyStateManager>().TransitionToState(new StunState());
                    }
                    else if (check.isNormalGuard) currentDamage /= 2f;
                }
                hp.TakeDamage(currentDamage);
            }
        }
    }

    public void BeginAttack()
    {
        isAttacking = true;
        _hitObjs.Clear();
    }
    public void EndAttack()
    {
        isAttacking = false;
        _hitObjs.Clear();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 center = transform.position + (Vector3)boxOffset;

        Gizmos.matrix = Matrix4x4.TRS(center, Quaternion.Euler(0, 0, boxAngle), Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, boxSize);

        Gizmos.matrix = Matrix4x4.identity;
    }
}
