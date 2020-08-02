using System.Collections;
using System.Collections.Generic;
using ScallyWags;
using UnityEngine;

public class GInventory
{
    private List<PickableItem> items = new List<PickableItem>();

    public void AddItem(PickableItem item)
    {
        items.Add(item);
    }

    public PickableItem FindGameObjectWithTag(ItemType itemType)
    {
        foreach (var item in items)
        {
            if (item.itemType == itemType)
            {
                return item;
            }
        }
        return null;
    }

    public void RemoveItem(PickableItem itemToRemove)
    {
        int index = -1;
        foreach (var item in items)
        {
            index += 1;
            if (item == itemToRemove)
            {
                break;
            }
        }
        items.RemoveAt(index);
    }
}
