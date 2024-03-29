using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    protected BaseState currentState;
    [TextArea][Tooltip("CurrentState")][SerializeField] string _curState;


    void Start()
    {
        currentState = GetInitialState();
        if (currentState != null)
            currentState.Enter();

    }

    void Update()
    {
        if (currentState != null)
            currentState.UpdateLogic();
    }

    void LateUpdate()
    {
        if (currentState != null)
            currentState.UpdatePhysics();
    }

    public void ChangeState(BaseState newState)
    {
        currentState.Exit();

        currentState = newState;
        currentState.Enter();
    }

    protected virtual BaseState GetInitialState()
    {
        return null;
    }

    // private void OnGUI()
    // {
    //     _curState = currentState != null ? currentState.name : "(no current state)";
    //     GUILayout.Label($"<color='black'><size=100>{_curState}</size></color>");
    // }

}
