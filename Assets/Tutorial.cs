using UnityEngine;

public class Tutorial : MonoBehaviour
{
    int Level = 1;
    float HP = 100f;
    double MP = 100;
    string NickName = "SuperMan";
    char Code = 'A';
    bool isAlive = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Game Started !!");
        Debug.Log($"Hello ! My name is {NickName}");
        Debug.Log($"My HP is {HP}, and My MP is {MP}");
        Debug.Log($"Am i alive ? {isAlive}");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            HP = HP - 20f;
            if (HP > 0)
            {
                print($"Damaged !Current My HP : {HP}");
            }
            else
            {
                isAlive = false;
                print($"HP is less than 0, I'm dead..");
            }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (HP >= 100f)
            {
                HP = 100f;
                return;
            }
            HP = HP + 10f;
            print($"Healed, Current My HP : {HP}");
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (MP <= 0) print("MP is not enough to use skill..");
            else
            {
                MP = MP - 20;
                print($"Cast Skill !! - Current MP : {MP}");
            }
        }
    }
}
