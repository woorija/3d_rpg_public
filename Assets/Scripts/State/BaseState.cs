using UnityEngine;

public class BaseState : MonoBehaviour, IState
{
    [field: SerializeField] public int priority { get; protected set; }
    protected Animator animator;
    protected StateMachine FSM;
    public virtual void Awake()
    {
        animator = transform.parent.GetComponentInChildren<Animator>();
        FSM = GetComponentInParent<StateMachine>();
    }

    public virtual void StateEnter()
    {
    }

    public virtual void StateExit()
    {
    }

    public virtual void StateUpdate()
    {
    }
}
