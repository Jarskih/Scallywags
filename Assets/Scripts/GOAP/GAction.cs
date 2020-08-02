using System;
using System.Collections;
using System.Collections.Generic;
using ScallyWags;
using UnityEngine;
using UnityEngine.AI;

public abstract class GAction : MonoBehaviour
{
    public string actionName = "Action";
    public float cost = 1.0f;
    public GameObject target;
    public string targetTag;
    public ItemType itemType;
    public float duration = 0;
    public WorldState[] preConditionData;
    public WorldState[] afterEffectData;
    public NavMeshAgent agent;
    public Player player;
    
    public Dictionary<string, int> preconditions = new Dictionary<string, int>();
    public Dictionary<string, int> effects = new Dictionary<string, int>();

    [SerializeField] private WorldStates agentBeliefs;
    public GInventory inventory;
    public WorldStates beliefs;

    public bool isRunning = false;
    public bool IsDone = false;

    public void Awake()
    {
        player = GetComponent<Player>();
        target = null;
        agent = GetComponent<NavMeshAgent>();
        
        foreach (var condition in preConditionData)
        {
            preconditions.Add(condition.key, condition.value);
        }
        
        foreach (var condition in afterEffectData)
        {
            effects.Add(condition.key, condition.value);
        }

        inventory = GetComponent<GAgent>().inventory;
        beliefs = GetComponent<GAgent>().beliefs;
    }

    public bool IsAchievable()
    {
        return true;
    }

    public bool IsAchievableGiven(Dictionary<string, int> requirements)
    {
        foreach (var precondition in preconditions)
        {
            // Check if all requirements are met by this action
            if (!requirements.ContainsKey(precondition.Key))
            {
                return false;
            }
        }
        return true;
    }

    public abstract bool PrePerform();
    public abstract void Perform();
    public abstract bool PostPerform();
}
