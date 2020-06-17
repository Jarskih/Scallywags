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
        goals.Add(goal1, 1);
    }
}
