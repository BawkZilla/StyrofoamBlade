using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillManager : MonoBehaviour
{
    [SerializeField] List<SkillBase> skillSlots = new List<SkillBase>();

    [SerializeField] KeyCode _skill1Key = KeyCode.Q; // ½½·Ô 0
    [SerializeField] KeyCode _skill2Key = KeyCode.E; // ½½·Ô 1

    void Update()
    {
        if (Input.GetKeyDown(_skill1Key))
        {
            ExecuteSkillAt(0);
            return;
        }

        if (Input.GetKeyDown(_skill2Key))
        {
            ExecuteSkillAt(1);
            return;
        }
    }

    void ExecuteSkillAt(int slot)
    {
        if (slot < 0 || slot >= skillSlots.Count) return;

        var skill = skillSlots[slot];
        if (skill == null) return;

        skill.ExecuteSkill();
    }

}
