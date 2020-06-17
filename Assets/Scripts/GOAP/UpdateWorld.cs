using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UpdateWorld : MonoBehaviour
{
    private Text states;

    void Start()
    {
        states = GetComponent<Text>();
    }
    void Update()
    {
        var worldStates = GWorld.Instance.GetWorld().GetStates();
        states.text = "";
        foreach (var state in worldStates)
        {
            states.text += state.Key + ": " + state.Value + "\n";
        }
    }
}
