using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class GWorld
{
    private static readonly GWorld _instance = new GWorld();
    private static WorldStates _world;
    private static Queue<GameObject> fixable = new Queue<GameObject>();
    private static Queue<GameObject> hammers = new Queue<GameObject>();

    static GWorld()
    {
        _world = new WorldStates();
        _world.ModifyState("Fixable", 0);

        var hammerObjects = GameObject.FindGameObjectsWithTag("Hammer");

        foreach (var hammer in hammerObjects)
        {
            hammers.Enqueue(hammer);
        }
        _world.ModifyState("Hammers", hammers.Count);
    }

    private GWorld()
    {
        Debug.Log("Instance constructor");
    }
    
    public void AddFixable(GameObject gameObject)
    {
        fixable.Enqueue(gameObject);
    }

    public GameObject GetFixable()
    {
        if (fixable.Count == 0)
        {
            return null;
        }
        return fixable.Dequeue();
    }

    public void AddHammer(GameObject gameObject)
    {
        hammers.Enqueue(gameObject);
    }
    
    public GameObject GetHammer()
    {
        if (hammers.Count == 0)
        {
            return null;
        }
        return hammers.Dequeue();
    }

    public static GWorld Instance => _instance;

    public WorldStates GetWorld()
    {
        return _world;
    }
}
