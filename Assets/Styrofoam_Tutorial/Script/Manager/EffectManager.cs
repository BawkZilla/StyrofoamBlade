using UnityEngine;

public enum E_EffectType
{
    Parry,
    Hit,
    Heal
}

public class EffectManager : MonoBehaviour
{
    public static EffectManager instance { get; private set; }

    public GameObject ParryEffect;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SpawnEffect(E_EffectType type, Vector2 spawnPos)
    {
        switch(type)
        {
            case E_EffectType.Parry:
                Instantiate(ParryEffect, spawnPos, Quaternion.identity);
                break;
        }
    }
}
