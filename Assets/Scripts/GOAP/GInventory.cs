using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GInventory
{
    private List<GameObject> items = new List<GameObject>();

    public void AddItem(GameObject item)
    {
        items.Add(item);
    }

    public GameObject FindGameObjectWithTag(string tag)
    {
        foreach (var item in items)
        {
            if (item.CompareTag(tag))
            {
                return item;
            }
        }
        return null;
    }

    public void RemoveItem(GameObject itemToRemove)
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
