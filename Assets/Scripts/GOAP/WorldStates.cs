using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class WorldState
{
    public string key;
    public int value;
}

public class WorldStates
{
    private Dictionary<string, int> _states;

    public WorldStates()
    {
        _states = new Dictionary<string, int>();
    }

    public bool HasState(string key)
    {
        return _states.ContainsKey(key);
    }

    /// <summary>
    /// Override the state value with new one
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void SetState(string key, int value)
    {
        _states[key] = value;
    }

    public Dictionary<string, int> GetStates()
    {
        return _states;
    }

    /// <summary>
    /// Increment or decrement state value. Remove state when value is 0
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    public void ModifyState(string key, int value)
    {
        if (_states.ContainsKey(key))
        {
            _states[key] += value;
            if (_states[key] <= 0)
            {
                RemoveState(key);
            } 
        }
        else
        {
            _states.Add(key, value);
        }
    }

    private void RemoveState(string key)
    {
        if (_states.ContainsKey(key))
        {
            _states.Remove(key);
        }
    }
}
