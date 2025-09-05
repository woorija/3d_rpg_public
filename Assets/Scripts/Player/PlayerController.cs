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
    private Vector3 gravityVelocity = Vector3.zero;

    public bool isRun { get; private set; } = false;
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
        if (!(GameManager.Instance.gameMode != GameMode.ControllMode))
        {
            FSM.StateUpdate();
            if(inputVector != Vector2.zero)
            {
                Move();
            }
        }
        status.StaminaUpdate();
        ApplyGravity();
    }
    #region FSM
    private void FSMInit()
    {
        SetAllStates();
        FSM.Init(StateType.Idle);
    }
    private void SetAllStates()
    {
        BaseState temp;
        temp = GetComponentInChildren<Player_Idle>();
        FSM.SetState(StateType.Idle, temp);
        temp = GetComponentInChildren<Player_Walk>();
        FSM.SetState(StateType.Walk, temp);
        temp = GetComponentInChildren<Player_Jump>();
        FSM.SetState(StateType.Jump, temp);
        temp = GetComponentInChildren<Player_Fall>();
        FSM.SetState(StateType.Fall, temp);
        temp = GetComponentInChildren<Player_Land>();
        FSM.SetState(StateType.Land, temp);
        temp = GetComponentInChildren<Player_Roll>();
        FSM.SetState(StateType.Roll, temp);
        temp = GetComponentInChildren<Player_Attack>();
        FSM.SetState(StateType.Attack, temp);
        temp = GetComponentInChildren<Player_Buff>();
        FSM.SetState(StateType.Buff, temp);
        temp = GetComponentInChildren<Player_ActiveSkill>();
        FSM.SetState(StateType.ActiveSkill, temp);
        temp = GetComponentInChildren<Player_Hit>();
        FSM.SetState(StateType.Hit, temp);
        temp = GetComponentInChildren<Player_Die>();
        FSM.SetState(StateType.Die, temp);
    }
    #endregion
    #region InputSystem
    private void PerformedMovement(InputAction.CallbackContext context)
    {
        inputVector = context.ReadValue<Vector2>();
        if (FSM.CanChangeState(StateType.Walk))
        {
            FSM.ChangeState(StateType.Walk);
        }
        animator.SetBool(AnimationKey.Move, true);
        animator.SetFloat(AnimationKey.DirectionX, inputVector.x);
        animator.SetFloat(AnimationKey.DirectionY, inputVector.y);
    }
    private void CanceledMovement(InputAction.CallbackContext context)
    {
        inputVector = Vector2.zero;
        animator.SetBool(AnimationKey.Move, false);
        if (FSM.GetCurrentStateType() == StateType.Walk)
        {
            FSM.ChangeState(StateType.Idle);
        }
    }
    private void PerformedRun(InputAction.CallbackContext context)
    {
        isRun = true;
        animator.SetBool(AnimationKey.IsRun, isRun);
    }
    private void PerformedRoll(InputAction.CallbackContext context)
    {
        if (status.Stamina < 30f) return;
        if (FSM.CanChangeState(StateType.Roll))
        {
            status.Stamina -= 30f;
            status.ExhaustTime = 2.5f;
            FSM.ChangeState(StateType.Roll);
        }
    }
    private void CanceledRun(InputAction.CallbackContext context)
    {
        isRun = false;
        animator.SetBool(AnimationKey.IsRun, isRun);
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
        if (!animator.GetBool(AnimationKey.IsPlayingSkill) && FSM.CanChangeState(StateType.Attack))
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
        if (!animator.GetBool(AnimationKey.IsPlayingSkill) && FSM.CanChangeState(StateType.ActiveSkill))
        {
            status.Mp -= _useMp;
            CooltimeManager.Instance.AddCooltime(_id, SkillDataBase.SkillDB[_id].coolTime);
            animator.SetInteger(AnimationKey.SkillId, _id);
            FSM.ChangeState(StateType.ActiveSkill);
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
        gravityVelocity.y = PlayerStatus.JumpForce;
    }
    private void ApplyGravity()
    {
        if (!playerController.isGrounded)
        {
            gravityVelocity.y += PlayerStatus.Gravity * Time.deltaTime;
        }
        playerController.Move(gravityVelocity * Time.deltaTime);
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
        if (!playerController.isGrounded && gravityVelocity.y < 0f)
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
        if (isRun)
        {
            if (status.Stamina >= 0.5f)
            {
                status.Stamina -= 0.5f;
                status.ExhaustTime = 0.5f;
                playerController.Move(moveDirection * (PlayerStatus.MoveSpeed + PlayerStatus.RunSpeed) * status.MoveSpeedMultiplier * StateMoveSpeedMultiplier * Time.deltaTime);
            }
            else
            {
                isRun = false;
                animator.SetBool(AnimationKey.IsRun, isRun);
            }
        }
        else
        {
            playerController.Move(moveDirection * PlayerStatus.MoveSpeed * status.MoveSpeedMultiplier * StateMoveSpeedMultiplier * Time.deltaTime);
        }
    }
    public void MoveRoll()
    {
        playerController.Move(moveDirection * 4 * Time.deltaTime);
    }
    public void AnimationEnd()
    {
        animator.SetTrigger(AnimationKey.AnimationEnd);
        FSM.ChangeState(StateType.Idle);
    }
    public void Teleport(Vector3 _pos)
    {
        Vector3 deltaPos = _pos - transform.position;
        gravityVelocity = Vector3.zero;
        playerController.Move(deltaPos);

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
