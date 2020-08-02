using System.Collections;
using System.Collections.Generic;
using ScallyWags;
using UnityEngine;

public class Drop : GAction
{
    public override bool PrePerform()
    {
        if (player.currentItem == null)
        {
            return false;
        }
        return true;
    }

    public override void Perform()
    {
        GWorld.Instance.GetResource(itemType)?.AddResource(player.currentItem.gameObject);
        player.Drop();
        IsDone = true;
    }

    public override bool PostPerform()
    {
        if (itemType == ItemType.Bucket)
        {
            beliefs.ModifyState("HasBucket", -1);
        }
        if (itemType == ItemType.Hammer)
        {
            beliefs.ModifyState("HasHammer", -1);
        }
        return true;
    }
}
