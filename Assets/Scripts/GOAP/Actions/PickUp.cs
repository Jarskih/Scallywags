﻿using System.Collections;
using System.Collections.Generic;
using ScallyWags;
using UnityEngine;

public class PickUp : GAction
{ 
    public override bool PrePerform()
    {
        target = GWorld.Instance.GetResource(ItemType.Hammer).RemoveResource();
        if (target == null)
        {
            return false;
        }

        Debug.Log("Started to pick up");
        return true;
    }

    public override void Perform()
    {
        player.PickUp();
        IsDone = true;
    }

    public override bool PostPerform()
    {
        if (itemType == ItemType.Bucket)
        {
            beliefs.ModifyState("HasBucket", 1);
        }
        if (itemType == ItemType.Hammer)
        {
            beliefs.ModifyState("HasHammer", 1);
        }
        
        Debug.Log("Finished to pick up");
        return true;
    }
}