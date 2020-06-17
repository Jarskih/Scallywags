using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Animations;

public class Node
{
    public Node parent;
    public float cost;
    public Dictionary<string, int> state;
    public GAction action;

    public Node(Node parent, float cost, Dictionary<string, int> states, GAction action)
    {
        this.parent = parent;
        this.cost = cost;
        this.state = new Dictionary<string, int>(states);
        this.action = action;
    }
    
    public Node(Node parent, float cost, Dictionary<string, int> states, Dictionary<string, int> beliefStates, GAction action)
    {
        this.parent = parent;
        this.cost = cost;
        this.state = new Dictionary<string, int>(states);
        this.action = action;
        foreach (var state in beliefStates)
        {
            this.state.Add(state.Key, state.Value);
        }
    }
}

public class GPlanner 
{
    public Queue<GAction> Plan(List<GAction> actions, Dictionary<string, int> goal, WorldStates beliefStates)
    {
        List<GAction> usableActions = new List<GAction>();
        foreach (var action in actions)
        {
            if (action.IsAchievable())
            {
                usableActions.Add(action);
            }
        }
        
        List<Node> leaves = new List<Node>();
        Node start = new Node(null, 0, GWorld.Instance.GetWorld().GetStates(), beliefStates.GetStates(), null);

        bool success = BuildGraph(start, leaves, usableActions, goal);

        if (!success)
        {
            Debug.LogError("No plan");
            return null;
        }

        Node cheapest = null;
        foreach (var leaf in leaves)
        {
            if (cheapest == null)
            {
                cheapest = leaf;
            }
            else
            {
                if (leaf.cost < cheapest.cost)
                {
                    cheapest = leaf;
                }
            }
        }

        List<GAction> result = new List<GAction>();
        Node n = cheapest;
        while (n != null)
        {
            if (n.action != null)
            {
                result.Insert(0, n.action);
            }
            n = n.parent;
        }
        
        Queue<GAction> queue = new Queue<GAction>();
        foreach (var action in result)
        {
            queue.Enqueue(action);
        }
        
        Debug.Log("The plan is: ");
        foreach (var action in queue)
        {
            Debug.Log("Q:" + action.actionName);
        }

        return queue;
    }

    private bool BuildGraph(Node parent, List<Node> leaves, List<GAction> usableActions, Dictionary<string, int> goal)
    {
        bool foundPath = false;
        foreach (var action in usableActions)
        {
            if (action.IsAchievableGiven(parent.state))
            {
                // Create copy of world state to simulate state change
                Dictionary<string,int> currentState = new Dictionary<string, int>(parent.state);
                foreach (var effect in action.effects)
                {
                    if (!currentState.ContainsKey(effect.Key))
                    {
                        currentState.Add(effect.Key, effect.Value);
                    }
                }
                
                Node node = new Node(parent, parent.cost + action.cost, currentState, action);

                if (GoalAchieved(goal, currentState))
                {
                    leaves.Add(node);
                    foundPath = true;
                }
                else
                {
                    List<GAction> subset = ActionSubset(usableActions, action);
                    bool found = BuildGraph(node, leaves, subset, goal);
                    if (found)
                    {
                        foundPath = true;
                    }
                }
            }
        }

        return foundPath;
    }

    /// <summary>
    /// Checks if the state can lead to the agent goal. If state is missing goal then goal is not achievable using this action
    /// </summary>
    /// <param name="goals"></param>
    /// <param name="state"></param>
    /// <returns></returns>
    private bool GoalAchieved(Dictionary<string, int> goal, Dictionary<string, int> state)
    {
        foreach (var g in goal)
        {
            if (!state.ContainsKey(g.Key))
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    ///  Remove action from usable actions
    /// </summary>
    /// <param name="usableActions"></param>
    /// <param name="action"></param>
    /// <returns></returns>
    
    private List<GAction> ActionSubset(List<GAction> actions, GAction removeMe)
    {
        List<GAction> subset = new List<GAction>();

        foreach (var action in actions)
        {
            if (action.Equals(removeMe))
            {
                // Do not add the used action to the list
                continue;
            }
            
            subset.Add(action);
        }
        return subset;
    }
}
