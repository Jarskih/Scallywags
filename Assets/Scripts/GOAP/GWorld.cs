using System.Collections;
using System.Collections.Generic;
using ScallyWags;
using UnityEngine;

public class ResourceQueue
{
    private Queue<GameObject> queue = new Queue<GameObject>();
    private string _tag;
    private string _modState;
    private WorldStates _worldStates;

    public ResourceQueue(string tag, string modState, WorldStates ws)
    {
        _tag = tag;
        _modState = modState;
        _worldStates = ws;

        if (tag != "")
        {
            GameObject[] resources = GameObject.FindGameObjectsWithTag(tag);
            foreach (var resource in resources)
            {
                queue.Enqueue(resource);
            }
        }

        if (modState != "")
        {
            ws.ModifyState(modState, queue.Count);
        }
    }

    public void AddResource(GameObject r)
    {
        queue.Enqueue(r);
        _worldStates.ModifyState(_modState, 1);
    }

    public GameObject RemoveResource()
    {
        if (queue.Count == 0)
        {
            return null;
        }

        var item =  queue.Dequeue();
        _worldStates.ModifyState(_modState, -1);
        return item;
    }
}

public sealed class GWorld
{
    private static readonly GWorld _instance = new GWorld();
    private static WorldStates _world;
    private static ResourceQueue holes;
    private static ResourceQueue fires;
    private static ResourceQueue hammers;
    private static ResourceQueue buckets;
    private static Dictionary<ItemType, ResourceQueue> resources = new Dictionary<ItemType, ResourceQueue>();

    static GWorld()
    {
        _world = new WorldStates();
        
        fires = new ResourceQueue("Fire", "Fires", _world);
        holes = new ResourceQueue("Hole", "Holes", _world);
        hammers = new ResourceQueue("Hammer", "Hammers", _world);
        buckets = new ResourceQueue("Bucket", "Buckets", _world);

        resources.Add(ItemType.Fire, fires);
        resources.Add(ItemType.Hole, holes);
        resources.Add(ItemType.Hammer, hammers);
        resources.Add(ItemType.Bucket, buckets);
    }

    private GWorld()
    {
    }

    public ResourceQueue GetResource(ItemType resource)
    {
        if (resource == ItemType.NotSet)
        {
            return null;
        }
        return resources[resource];
    }

    public static GWorld Instance => _instance;

    public WorldStates GetWorld()
    {
        return _world;
    }
}
