using UnityEngine;

public class PlayerGuard : MonoBehaviour
{
    [HideInInspector]
    public bool isGuard;
    [HideInInspector]
    public float GuardingTime = 0f;

    void Update()
    {
        isGuard = Input.GetMouseButton(1);
        GetComponent<Animator>().SetBool("_isGuard", isGuard);

        if (isGuard)
        {
            GuardingTime += Time.deltaTime;
        }
        else
        {
            GuardingTime = 0f;
        }
    }
}
