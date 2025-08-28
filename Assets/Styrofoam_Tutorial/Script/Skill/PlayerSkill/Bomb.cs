using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Bomb : SkillBase
{
    [Header("AoE")]
    [SerializeField] float _radius = 2.5f;
    [SerializeField] Vector2 _centerOffset = Vector2.zero;
    [SerializeField] float _damage = 12f;
    [SerializeField] LayerMask _enemyMask;

    [Header("Gizmos")]
    [SerializeField] Color _gizmoColor = new Color(1f, 0.5f, 0f, 0.6f);
    [SerializeField] bool _drawGizmoAlways = true;

    [Header("Effect")]
    [SerializeField] GameObject _bombEffect;

    protected override void OnCast()
    {

        Vector2 center = (Vector2)transform.position + _centerOffset;
        var hits = Physics2D.OverlapCircleAll(center, _radius, _enemyMask);

        if (_bombEffect) Instantiate(_bombEffect, center, Quaternion.identity);


        if (hits == null || hits.Length == 0) return;

        HashSet<StatBehaviour> uniq = new HashSet<StatBehaviour>();
        for (int i = 0; i < hits.Length; i++)
        {
            var col = hits[i].GetComponent<Collider2D>();
            if (!col) continue;

            var stat = col.GetComponent<StatBehaviour>() ?? col.GetComponentInParent<StatBehaviour>();
            if (stat == null) continue;

            if (uniq.Add(stat))
                stat.TakeDamage(_damage);
        }
    }

    void OnDrawGizmos()
    {
        if (!_drawGizmoAlways && !UnityEditor.Selection.Contains(gameObject)) return;

        Gizmos.color = _gizmoColor;
        Vector3 center = transform.position + (Vector3)_centerOffset;
        Gizmos.DrawWireSphere(center, _radius);
    }

    void OnDrawGizmosSelected()
    {
        if (_drawGizmoAlways) return;

        Gizmos.color = _gizmoColor;
        Vector3 center = transform.position + (Vector3)_centerOffset;
        Gizmos.DrawWireSphere(center, _radius);
    }
}
