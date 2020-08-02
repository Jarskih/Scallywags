using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAgent : GAgent
{
    private void Start()
    {
        Init();
        
        SubGoal goal1 = new SubGoal("Repair", 1, false);
        goals.Add(goal1, 2);
        
        SubGoal goal2 = new SubGoal("NoTool", 1, false);
        goals.Add(goal2, 1);
    }
}
