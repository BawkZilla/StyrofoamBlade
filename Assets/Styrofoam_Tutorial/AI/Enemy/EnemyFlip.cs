using UnityEngine;

public class EnemyFlip : MonoBehaviour
{
    [SerializeField] Transform _avatar;
    public void Flip()
    {
        Vector3 scale = _avatar.transform.localScale;
        scale.x *= -1;
        _avatar.transform.localScale = scale;
    }
}
