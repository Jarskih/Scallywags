using System.Collections;
using System.Collections.Generic;
using ScallyWags;
using UnityEngine;

public class Repair : GAction
{
    public override bool PrePerform()
    {
        target = GWorld.Instance.GetResource(itemType).RemoveResource();
        if (target == null)
        {
            return false;
        }
        
        Debug.Log("Begin fixing");
        return true;
    }
    
    public override void Perform()
    {
        if (target.activeInHierarchy == false)
        {
            IsDone = true;
            return;
        }
        target.GetComponent<FixableInteraction>().Act();
    }

    public override bool PostPerform()
    {
        Debug.Log("Finished fixing");
        return true;
    }
}
