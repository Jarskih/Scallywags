using System.Collections;
using System.Collections.Generic;
using ScallyWags;
using UnityEngine;

public class PickUp : GAction
{ 
    public override bool PrePerform()
    {
        target = GWorld.Instance.GetHammer();
        if (target == null)
        {
            return false;
        }
        return true;
    }

    public override void Perform()
    {
        player.PickUp();
        IsDone = true;
    }

    public override bool PostPerform()
    {
        return true;
    }
}
