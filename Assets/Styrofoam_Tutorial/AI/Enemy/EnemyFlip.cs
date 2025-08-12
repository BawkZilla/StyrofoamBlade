using UnityEngine;

public class EnemyFlip : MonoBehaviour
{
    public void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
