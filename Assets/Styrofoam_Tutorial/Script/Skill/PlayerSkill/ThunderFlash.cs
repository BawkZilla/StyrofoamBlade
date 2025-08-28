using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class SkillThunderFlash : SkillBase
{
    [Header("Dash")]
    [SerializeField] float _dashDistance = 6f;
    [SerializeField] float _dashTime = 0.08f;
    [SerializeField] bool _clampAtWalls = true;
    [SerializeField] float _skin = 0.05f;
    [SerializeField] LayerMask _blockMask;

    [Header("Damage")]
    [SerializeField] float _damage = 22f;
    [SerializeField] LayerMask _enemyMask;
    [SerializeField] Vector2 _pathHitBox = new Vector2(0.9f, 1.4f);

    [Header("Effect")]
    [SerializeField] GameObject _dashStartEffect;
    [SerializeField] GameObject _dashEndEffect;

    Rigidbody2D _rb;
    TrailRenderer _trail;
    bool _running;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _trail = GetComponent<TrailRenderer>();
    }
    protected override void OnCast()
    {
        StartCoroutine(C_Dash());
    }

    IEnumerator C_Dash()
    {
        if(_dashStartEffect) Instantiate(_dashStartEffect, transform.position, Quaternion.identity);

        _trail.enabled = true;

        _running = true;

        int facing = GetFacing();
        Vector2 dir = Vector2.right * facing;

        float dist = _dashDistance;
        if (_clampAtWalls)
            dist = ClampDistance(_dashDistance, dir);

        RaycastHit2D[] pathHits = Physics2D.BoxCastAll(
            _rb.position, _pathHitBox, 0f, dir, dist, _enemyMask
        );

        ToggleIgnoreEnemyLayers(true);

        Vector2 start = _rb.position;
        Vector2 end = start + dir * dist;

        float t = 0f;
        while (t < _dashTime)
        {
            t += Time.deltaTime;
            float u = Mathf.Clamp01(t / _dashTime);
            _rb.MovePosition(Vector2.Lerp(start, end, u));
            yield return null;
        }

        ToggleIgnoreEnemyLayers(false);
        ApplyPathDamage(pathHits);

        _running = false;

        _trail.enabled = false;

        if (_dashEndEffect) Instantiate(_dashEndEffect, transform.position, Quaternion.identity);
    }

    int GetFacing()
    {
        return transform.localScale.x >= 0f ? 1 : -1;
    }

    float ClampDistance(float maxDist, Vector2 dir)
    {
        var hit = Physics2D.Raycast(_rb.position, dir, maxDist, _blockMask);
        if (hit.collider == null) return maxDist;
        return Mathf.Max(0f, hit.distance - _skin);
    }

    void ApplyPathDamage(RaycastHit2D[] hits)
    {
        if (hits == null || hits.Length == 0) return;

        HashSet<StatBehaviour> uniq = new HashSet<StatBehaviour>();
        foreach (var h in hits)
        {
            if (!h.collider) continue;

            var stat = h.collider.GetComponent<StatBehaviour>() ?? h.collider.GetComponentInParent<StatBehaviour>();
            if (stat == null) continue;

            if (uniq.Add(stat))
                stat.TakeDamage(_damage);
        }
    }

    void ToggleIgnoreEnemyLayers(bool on)
    {
        int playerLayer = gameObject.layer;
        int mask = _enemyMask.value;

        for (int layer = 0; layer < 32; layer++)
        {
            if ((mask & (1 << layer)) == 0) continue;
            Physics2D.IgnoreLayerCollision(playerLayer, layer, on);
        }
    }
}
