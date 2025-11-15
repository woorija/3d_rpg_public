using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, IInputBindable
{
    private StateMachine FSM;
    private Animator animator;
    private PlayerStatus status;
    private CharacterController playerController;

    private Vector2 inputVector;
    private Vector3 moveDirection;
    private float gravityVelocity;
    private Vector3 moveVector = Vector3.zero;

    public bool isRun { get; private set; } = false;
    bool jumpTriggered = false;
    public float StateMoveSpeedMultiplier { get; set; }
    public int currentPlaySkillId { get; private set; }
    Action[] onUseQuickSlots;

    Action<InputAction.CallbackContext> quickSlot1Handler;
    Action<InputAction.CallbackContext> quickSlot2Handler;
    Action<InputAction.CallbackContext> quickSlot3Handler;
    Action<InputAction.CallbackContext> quickSlot4Handler;
    Action<InputAction.CallbackContext> quickSlot5Handler;
    Action<InputAction.CallbackContext> quickSlot6Handler;
    Action<InputAction.CallbackContext> quickSlot7Handler;
    Action<InputAction.CallbackContext> quickSlot8Handler;
    private void Awake()
    {
        FSM = GetComponent<StateMachine>();
        animator = GetComponentInChildren<Animator>();
        status = GetComponent<PlayerStatus>();
        playerController = GetComponent<CharacterController>();

        onUseQuickSlots = new Action[8];
    }
    private void Start()
    {
        FSMInit();
        InputInit();
        EventInit();
        moveDirection = Vector3.forward;
    }
    private void InputInit()
    {
        InitInputHandlers();
        BindAllInputActions();
    }
    private void EventInit()
    {
        for (int i = 0; i < onUseQuickSlots.Length; i++)
        {
            int index = i;
            onUseQuickSlots[index] += () => QuickSlotData.Instance.Use(index);
        }
        QuickSlotData.Instance.SkillAddlistener(UseSkill);
        CustomSceneManager.Instance.playerTeleportEvent += Teleport;
        status.OnHit += ChangeHitState;
        status.OnDie += ChangeDieState;
        status.onMoveSpeedMultiplierChanged += SetAnimatorMoveSpeedMultiplier;
        status.onActionSpeedMultiplierChanged += SetAnimatorActionSpeedMultiplier;
    }
    private void Update()
    {
        if (GameManager.Instance.gameMode == GameMode.ControllMode)
        {
            FSM.StateUpdate();
            if(inputVector != Vector2.zero)
            {
                Move();
            }
            else
            {
                moveVector = Vector3.zero;
            }
        }

        if (jumpTriggered)
        {
            gravityVelocity = PlayerStatus.JumpForce;
            moveVector.y += gravityVelocity * Time.deltaTime;
            jumpTriggered = false;
        }
        else
        {
            ApplyGravity();
        }

        status.StaminaUpdate();
        playerController.Move(moveVector);
    }
    #region FSM
    private void FSMInit()
    {
        SetAllStates();
        FSM.Init(StateType.Idle);
    }
    private void SetAllStates()
    {
        SetState<Player_Idle>(StateType.Idle);
        SetState<Player_Walk>(StateType.Walk);
        SetState<Player_Run>(StateType.Run);
        SetState<Player_Jump>(StateType.Jump);
        SetState<Player_Fall>(StateType.Fall);
        SetState<Player_Land>(StateType.Land);
        SetState<Player_Roll>(StateType.Roll);
        SetState<Player_Attack>(StateType.Attack);
        SetState<Player_Buff>(StateType.Buff);
        SetState<Player_ActiveSkillEnter>(StateType.ActiveSkillEnter);
        SetState<Player_ActiveSkill>(StateType.ActiveSkill);
        SetState<Player_Hit>(StateType.Hit);
        SetState<Player_Die>(StateType.Die);
    }
    private void SetState<T>(StateType _type) where T : BaseState
    {
        var state = GetComponentInChildren<T>();
        FSM.SetState(_type, state);
    }
    #endregion
    #region InputSystem
    private void PerformedMovement(InputAction.CallbackContext context)
    {
        inputVector = context.ReadValue<Vector2>();
        if (IsGround())
        {
            if (isRun && FSM.CanChangeState(StateType.Run))
            {
                FSM.ChangeState(StateType.Run);
            }
            else if (FSM.CanChangeState(StateType.Walk))
            {
                FSM.ChangeState(StateType.Walk);
            }
        }
    }
    private void CanceledMovement(InputAction.CallbackContext context)
    {
        inputVector = Vector2.zero;
        if (FSM.GetCurrentStateType() == StateType.Walk || FSM.GetCurrentStateType() == StateType.Run)
        {
            FSM.ChangeState(StateType.Idle);
        }
    }
    private void PerformedRun(InputAction.CallbackContext context)
    {
        isRun = true;
        if (IsGround() && FSM.GetCurrentStateType() == StateType.Walk)
        {
            FSM.ChangeState(StateType.Run);
        }
    }
    private void CanceledRun(InputAction.CallbackContext context)
    {
        isRun = false;
        if (IsGround())
        {
            if (IsMove() && FSM.CanChangeState(StateType.Walk))
            {
                FSM.ChangeState(StateType.Walk);
            }
            else if (FSM.CanChangeState(StateType.Idle))
            {
                FSM.ChangeState(StateType.Idle);
            }
        }
    }
    private void PerformedRoll(InputAction.CallbackContext context)
    {
        if (!IsGround()) return;
        if (status.Stamina < 30f) return;
        if (FSM.CanChangeState(StateType.Roll))
        {
            status.Stamina -= 30f;
            status.ExhaustTime = 2.5f;
            FSM.ChangeState(StateType.Roll);
        }
    }
    private void PerformedJump(InputAction.CallbackContext context)
    {
        if (FSM.CanChangeState(StateType.Jump))
        {
            FSM.ChangeState(StateType.Jump);
        }
    }
    private void PerformedAttack(InputAction.CallbackContext context)
    {
        if (FSM.CanChangeState(StateType.Attack))
        {
            currentPlaySkillId = status.playerClass * 10 + status.classRank;
            FSM.ChangeState(StateType.Attack);
        }
    }
    private void PerformedUseQuickSlot(InputAction.CallbackContext context,int _index)
    {
        onUseQuickSlots[_index].Invoke();
    }
    private void SetAnimatorMoveSpeedMultiplier(float _value)
    {
        animator.SetFloat(AnimationKey.MoveSpeed, _value);
    }
    private void SetAnimatorActionSpeedMultiplier(float _value)
    {
        animator.SetFloat(AnimationKey.ActionSpeed, _value);
    }

    private void UseSkill(int _id)
    {
        int useMp = SkillData.Instance.GetSkillUseMp(_id);
        if(status.Mp <  useMp) return;
        if (CooltimeManager.Instance.IsCooltime(_id)) return;

        DevelopUtility.Log($"사용스킬ID:{_id}");

        if (BuffDataBase.skillBuffDB.ContainsKey(_id))
        {
            UseBuffSkill(_id, useMp);
            DevelopUtility.Log("버프");
        }
        else
        {
            UseActiveSkill(_id, useMp);
            DevelopUtility.Log("액티브");
        }
    }
    void UseActiveSkill(int _id, int _useMp)
    {
        currentPlaySkillId = _id; // 스킬관련 계산용 추가 변수
        Skill skill = SkillDataBase.SkillDB[_id];
        if (FSM.GetPriority() <= skill.entryPriority && GameManager.Instance.gameMode == GameMode.ControllMode)
        {
            status.Mp -= _useMp;
            CooltimeManager.Instance.AddCooltime(_id, skill.coolTime);
            FSM.ChangeState(StateType.ActiveSkillEnter);
        }
    }
    void UseBuffSkill(int _id, int _useMp)
    {
        if (FSM.CanChangeState(StateType.Buff))
        {
            FSM.ChangeState(StateType.Buff);
            BuffManager.Instance.ApplyBuff(_id);
            status.Mp -= _useMp;
        }
    }
    #endregion
    public void Jump()
    {
        jumpTriggered = true;
    }
    private void ApplyGravity()
    {
        if (!playerController.isGrounded)
        {
            gravityVelocity += PlayerStatus.Gravity * Time.deltaTime;
            moveVector.y += gravityVelocity * Time.deltaTime;
        }
        else
        {
            gravityVelocity = 0f;
        }
    }
    public bool IsMove()
    {
        if (inputVector == Vector2.zero)
        {
            return false;
        }
        return true;
    }
    public bool IsFall()
    {
        if (!playerController.isGrounded && gravityVelocity < -1f)
        {
            return true;
        }
        return false;
    }
    public bool IsGround()
    {
        if (playerController.isGrounded)
        {
            return true;
        }
        return false;
    }
    public void ChangeHitState()
    {
        if (FSM.CanChangeState(StateType.Hit))
        {
            FSM.ChangeState(StateType.Hit);
        }
    }
    public void ChangeDieState()
    {
        if (FSM.CanChangeState(StateType.Die))
        {
            FSM.ChangeState(StateType.Die);
        }
    }
    public void Rotate()
    {
        if (GameManager.Instance.gameMode != GameMode.ControllMode) return;
        transform.rotation = Quaternion.LookRotation(moveDirection);
    }
    public void RotateToWalk()
    {
        if (GameManager.Instance.gameMode != GameMode.ControllMode) return;
        CalculateMoveDirection(inputVector);
        transform.rotation = Quaternion.LookRotation(moveDirection);
    }
    public void LookForward()
    {
        if (GameManager.Instance.gameMode != GameMode.ControllMode) return;
        Vector3 lookVector = Camera.main.transform.forward;
        lookVector.y = 0f;
        transform.rotation = Quaternion.LookRotation(lookVector);
    }
    void CalculateMoveDirection(Vector2 _dir)
    {
        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0;
        Quaternion cameraRotation = Quaternion.LookRotation(-cameraForward);

        moveDirection = cameraRotation * new Vector3(-_dir.x, 0, -_dir.y);
    }
    public void Move()
    {
        float moveSpeed = PlayerStatus.MoveSpeed;
        if (isRun && FSM.currentStateType == StateType.Run)
        {
            if (status.Stamina >= 0.5f)
            {
                status.Stamina -= 0.5f;
                status.ExhaustTime = 0.5f;

                moveSpeed = PlayerStatus.MoveSpeed + PlayerStatus.RunSpeed;
            }
            else
            {
                isRun = false;
            }
        }
        moveVector = moveDirection * moveSpeed * status.MoveSpeedMultiplier * StateMoveSpeedMultiplier * Time.deltaTime;
    }
    public void MoveRoll()
    {
        playerController.Move(moveDirection * 4 * Time.deltaTime);
    }
    public void AnimationEnd()
    {
        FSM.ChangeState(StateType.Idle);
    }
    public void Teleport(Vector3 _pos)
    {
        Vector3 deltaPos = _pos - transform.position;
        moveVector = Vector3.zero;
        gravityVelocity = 0f;
        playerController.enabled = false;
        
        transform.position = _pos;
        
        playerController.enabled = true;
        Debug.Log(deltaPos);
        GameManager.Instance.CameraTeleport(transform, deltaPos);
    }
    public void SetInvincible(bool _value)
    {
        status.IsInvincible = _value;
    }

    public void InitInputHandlers()
    {
        quickSlot1Handler = ctx => PerformedUseQuickSlot(ctx, 0);
        quickSlot2Handler = ctx => PerformedUseQuickSlot(ctx, 1);
        quickSlot3Handler = ctx => PerformedUseQuickSlot(ctx, 2);
        quickSlot4Handler = ctx => PerformedUseQuickSlot(ctx, 3);
        quickSlot5Handler = ctx => PerformedUseQuickSlot(ctx, 4);
        quickSlot6Handler = ctx => PerformedUseQuickSlot(ctx, 5);
        quickSlot7Handler = ctx => PerformedUseQuickSlot(ctx, 6);
        quickSlot8Handler = ctx => PerformedUseQuickSlot(ctx, 7);
    }

    public void BindAllInputActions()
    {
        var PlayerAction = CustomInputManager.Instance.Player;

        PlayerAction.Movement.performed += PerformedMovement;
        PlayerAction.Movement.canceled += CanceledMovement;
        PlayerAction.RunModifier.performed += PerformedRun;
        PlayerAction.RunModifier.canceled += CanceledRun;
        PlayerAction.Roll.performed += PerformedRoll;
        PlayerAction.Jump.performed += PerformedJump;
        PlayerAction.Attack.performed += PerformedAttack;

        PlayerAction.QuickSlot1.performed += quickSlot1Handler;
        PlayerAction.QuickSlot2.performed += quickSlot2Handler;
        PlayerAction.QuickSlot3.performed += quickSlot3Handler;
        PlayerAction.QuickSlot4.performed += quickSlot4Handler;
        PlayerAction.QuickSlot5.performed += quickSlot5Handler;
        PlayerAction.QuickSlot6.performed += quickSlot6Handler;
        PlayerAction.QuickSlot7.performed += quickSlot7Handler;
        PlayerAction.QuickSlot8.performed += quickSlot8Handler;
    }

    public void UnbindAllInputActions()
    {
        var PlayerAction = CustomInputManager.Instance.Player;

        PlayerAction.Movement.performed -= PerformedMovement;
        PlayerAction.Movement.canceled -= CanceledMovement;
        PlayerAction.RunModifier.performed -= PerformedRun;
        PlayerAction.RunModifier.canceled -= CanceledRun;
        PlayerAction.Roll.performed -= PerformedRoll;
        PlayerAction.Jump.performed -= PerformedJump;
        PlayerAction.Attack.performed -= PerformedAttack;

        PlayerAction.QuickSlot1.performed -= quickSlot1Handler;
        PlayerAction.QuickSlot2.performed -= quickSlot2Handler;
        PlayerAction.QuickSlot3.performed -= quickSlot3Handler;
        PlayerAction.QuickSlot4.performed -= quickSlot4Handler;
        PlayerAction.QuickSlot5.performed -= quickSlot5Handler;
        PlayerAction.QuickSlot6.performed -= quickSlot6Handler;
        PlayerAction.QuickSlot7.performed -= quickSlot7Handler;
        PlayerAction.QuickSlot8.performed -= quickSlot8Handler;
    }
}
