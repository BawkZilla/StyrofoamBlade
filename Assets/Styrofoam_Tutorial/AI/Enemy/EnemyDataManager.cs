using UnityEngine;

public class EnemyDataManager : MonoBehaviour
{

    [SerializeField] EnemyData _enemyData;
    void Start()
    {
        GetEnemyData();
    }

    void GetEnemyData()
    {
        print($"Name : {_enemyData.EnemyName}\n" +
            $"HP: {_enemyData.MaxHP}\n" +
            $"Damage : {_enemyData.Damage}\n" +
            $"MoveSpeed : {_enemyData.MoveSpeed}\n" +
            $"Description: {_enemyData.EnemyDescription}");
    }


}
