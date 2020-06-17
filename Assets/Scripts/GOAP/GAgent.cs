using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using ScallyWags;
using UnityEngine.AI;

public class SubGoal
{
    public Dictionary<string, int> sgoals = new Dictionary<string, int>();
    public bool remove;

    public SubGoal(string key, int value, bool shouldRemove)
    {
        sgoals.Add(key,value);
        remove = shouldRemove;
    }
}

public class GAgent : MonoBehaviour
{
    public List<GAction> actions = new List<GAction>();
    public Dictionary<SubGoal, int> goals = new Dictionary<SubGoal, int>();
    public GInventory inventory = new GInventory();
    public WorldStates beliefs = new WorldStates();
    
    private GPlanner _planner;
    private Queue<GAction> _actionQueue = new Queue<GAction>();
    public GAction CurrentAction => _currentAction;
    private GAction _currentAction;
    private SubGoal _currentGoal;
    private bool _invoked;
    private Player _player;

    protected void Init()
    {
        GAction[] acts = GetComponents<GAction>();
        foreach (var action in acts)
        {
            actions.Add(action);
        }

        _player = GetComponent<Player>();
    }
    
    void Update()
    {
        UpdateItem();
        
        if (_currentAction != null && _currentAction.isRunning)
        {
            if (_currentAction.agent.remainingDistance < 1f)
            {
                _currentAction.Perform();
                if (!_invoked && _currentAction.IsDone)
                {
                    Invoke("CompleteAction", _currentAction.duration);
                    _invoked = true;
                }
            }

            return;
        }
        
        if (_planner == null || _actionQueue == null)
        {
            _planner = new GPlanner();

            var sortedGoals = from entry in goals orderby entry.Value descending select entry;

            foreach (var sg in sortedGoals)
            {
                _actionQueue = _planner.Plan(actions, sg.Key.sgoals, beliefs);
                if (_actionQueue != null)
                {
                    _currentGoal = sg.Key;
                    break;
                }
            }
        }

        if (_actionQueue == null)
        {
            return;
        }

        if (_actionQueue.Count == 0)
        {
            if (_currentGoal.remove)
            {
                goals.Remove(_currentGoal);
            }

            _planner = null;
        }

        if (_actionQueue.Count > 0)
        {
            _currentAction = _actionQueue.Dequeue();
            if (_currentAction.PrePerform())
            {
                if (_currentAction.target == null && _currentAction.targetTag != "")
                {
                    _currentAction.target = GameObject.FindWithTag(_currentAction.targetTag);
                }
                
                if (_currentAction.target != null)
                {
                    _currentAction.isRunning = true;
                    _currentAction.agent.SetDestination(_currentAction.target.transform.position);
                }
            }
            else
            {
                _actionQueue = null;
            }
        }
    }

    private void UpdateItem()
    {
        if (_player.currentItem != null)
        {
            beliefs.ModifyState("HasHammer", 1);
        }
    }

    void CompleteAction()
    {
        _currentAction.isRunning = false;
        _currentAction.PostPerform();
        _invoked = false;
    }
}
