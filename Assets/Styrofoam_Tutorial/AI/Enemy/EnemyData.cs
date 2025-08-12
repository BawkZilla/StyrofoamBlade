using UnityEngine;

public enum E_SightType
{
    Box, Circle
}

[CreateAssetMenu(menuName = "Enemy/EnemyData")]
public class EnemyData : ScriptableObject
{
    public string EnemyName;
    [TextArea]
    public string EnemyDescription;

    public float MaxHP;
    public float Damage;
    public float MoveSpeed;

    public E_SightType EnemySightType;
    public Vector2 SightBoxSize;
    public float SightRadius;
    [Range(0, 360)]
    public float SightAngle = 120f;

    public float AttackRange;
}
