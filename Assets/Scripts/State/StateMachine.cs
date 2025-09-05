using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [SerializeField] BaseState currentState;
    Dictionary<StateType, BaseState> States;
    Queue<BaseState> stateQueue = new Queue<BaseState>();
    Queue<StateType> stateTypeQueue = new Queue<StateType>();
    public StateType currentStateType;

    private void Awake()
    {
        States = new Dictionary<StateType, BaseState>();
    }
    public void Init(StateType _type)
    {
        if (States.ContainsKey(_type))
        {
            currentState = States[_type];
            currentStateType = _type;
        }
    }
    public void ChangeState(StateType _newState)
    {
        
        if (States.ContainsKey(_newState) && currentState != States[_newState])
        {
            stateQueue.Enqueue(States[_newState]);
            stateTypeQueue.Enqueue(_newState);
        }
    }
    public bool CanChangeState(StateType _newState)
    {
        if (GameManager.Instance.gameMode != GameMode.ControllMode)
        {
            return false;
        }
        if (currentState == States[_newState] || currentState.priority > States[_newState].priority)
        {
            return false;
        }
        return true;
    }
    public void SetState(StateType _type, BaseState _state)
    {
        States.Add(_type, _state);
    }
    public void StateUpdate()
    {
        if(stateQueue.Count > 0)
        {
            currentState.StateExit();
            currentState = stateQueue.Dequeue();
            currentStateType = stateTypeQueue.Dequeue();
            currentState.StateEnter();
        }
        currentState.StateUpdate();
    }
    public StateType GetCurrentStateType()
    {
        return currentStateType;
    }
}
