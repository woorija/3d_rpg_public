using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TalkManager : SingletonBehaviour<TalkManager>, IInputBindable
{
    [SerializeField] TalkUI talkUi;
    [SerializeField] NpcCamera npcCamera;

    List<InteractButton> interactButtons;
    [SerializeField] InteractButton questButton;
    [SerializeField] InteractButton shopButton;
    [SerializeField] InteractButton craftButton;
    [SerializeField] InteractButton teleportButton;

    [SerializeField] Button exitButton;

    [SerializeField] TeleportInteractsUI teleportUI;
    [SerializeField] QuestInteractsUI questUI;

    bool isLastInteract = false;
    public int npcId {  get; private set; } // 상점 오픈을 위해 외부에서 사용
    int lastTalkNpcId = 0;
    int questId = 0;
    int questProgress = 0;
    int talkIndex;
    bool isSubInteract;

    public NpcData currentInteractNPCData { get; private set; }

    Dictionary<(int npcId, int questId, int questProgress, int talkIndex), (int talkNpcId, string talk)> talkDB;

    const string questTalkText = "수락 할 퀘스트를 선택하세요.";
    const string teleportTalkText = "이동하고 싶은 장소를 선택하세요.";

    Action<InputAction.CallbackContext> SubInteract1Handler;
    Action<InputAction.CallbackContext> SubInteract2Handler;
    Action<InputAction.CallbackContext> SubInteract3Handler;
    Action<InputAction.CallbackContext> SubInteract4Handler;
    private void Start()
    {
        interactButtons = new List<InteractButton>();
        InputInit();
        talkDB = new Dictionary<(int npcId, int questId, int questProgress, int talkIndex), (int talkNpcId, string talk)>();
    }
    public void InitInputHandlers()
    {
        SubInteract1Handler = ctx => PerformedSubInteract(ctx, 0);
        SubInteract2Handler = ctx => PerformedSubInteract(ctx, 1);
        SubInteract3Handler = ctx => PerformedSubInteract(ctx, 2);
        SubInteract4Handler = ctx => PerformedSubInteract(ctx, 3);
    }

    public void BindAllInputActions()
    {
        var InteractAction = CustomInputManager.Instance.Manager;

        InteractAction.Interact.performed += PerformedInteract;

        InteractAction.SubInteract1.performed += SubInteract1Handler;
        InteractAction.SubInteract2.performed += SubInteract2Handler;
        InteractAction.SubInteract3.performed += SubInteract3Handler;
        InteractAction.SubInteract4.performed += SubInteract4Handler;
    }

    public void UnbindAllInputActions()
    {
        var InteractAction = CustomInputManager.Instance.Manager;

        InteractAction.Interact.performed -= PerformedInteract;

        InteractAction.SubInteract1.performed -= SubInteract1Handler;
        InteractAction.SubInteract2.performed -= SubInteract2Handler;
        InteractAction.SubInteract3.performed -= SubInteract3Handler;
        InteractAction.SubInteract4.performed -= SubInteract4Handler;
    }
    private void InputInit()
    {
        InitInputHandlers();
        BindAllInputActions();
    }
    public void SetNpc(NpcData _npc)
    {
        currentInteractNPCData = _npc;
        if(currentInteractNPCData.teleportData != null)
        {
            teleportUI.SetUI(currentInteractNPCData.teleportData);
        }
    }
    public void ResetNpc()
    {
        currentInteractNPCData = null;
    }
    private void PerformedInteract(InputAction.CallbackContext context)
    {
        if (currentInteractNPCData != null)
        {
            npcId = currentInteractNPCData.npcId;
            if (!talkUi.GetObjectActiveSelf() && npcId != 0) // 대화 시작
            {
                if (npcId != lastTalkNpcId)
                {
                    TalkDataConverter(currentInteractNPCData.talkDataSO.talkDatas);
                    lastTalkNpcId = npcId;
                }
                talkIndex = 0;
                isSubInteract = false;
                talkUi.OpenUI();
                if(talkDB.TryGetValue((npcId, questId, questProgress, talkIndex), out var talkTuple))
                {
                    talkUi.TypingStart(talkTuple.talk, talkTuple.talkNpcId);
                }
                npcCamera.SetCameraPos(currentInteractNPCData.camLookAtTransform);
            }
            else if (talkUi.IsTyping()) // 대사 출력중이면 바로 완료
            {
                talkUi.TypingEnd();
                SetNextTalkId();
            }
            else if (!isSubInteract && talkDB.TryGetValue((npcId, questId, questProgress, talkIndex), out var talkTuple)) // 다음대사가 존재하면 타이핑 시작
            {
                DevelopUtility.Log("대사시작" + (npcId, questId, questProgress, talkIndex));
                talkUi.TypingStart(talkTuple.talk, talkTuple.talkNpcId);
            }
            else if (isSubInteract)
            {
                //퀘스트 선택 및 이동 선택에서 아무것도 하지 않기 위함
            }
            else
            {
                //대화 종료
                CloseUI();
            }
        }
    }
    private void PerformedSubInteract(InputAction.CallbackContext context, int _index)
    {
        if (isLastInteract)
        {
            if (_index < interactButtons.Count && interactButtons[_index].gameObject.activeSelf)
            {
                interactButtons[_index].onClick();
            }
        }
    }
    public void SetNextTalkId() // TalkUI의 유니티액션에 등록
    {
        talkIndex++;

        if (isSubInteract || questId != 0) return;
        if (talkDB.ContainsKey((npcId, questId, questProgress, talkIndex))) return;

        // 마지막 일반대사에 호출
        SetInteractButtons();
    }
    void SetInteractButtons()
    {
        if (currentInteractNPCData.hasQuest)
        {
            questButton.gameObject.SetActive(true);
            interactButtons.Add(questButton);
        }
        if (currentInteractNPCData.ShopDataSO != null)
        {
            shopButton.gameObject.SetActive(true);
            interactButtons.Add(shopButton);
        }
        if (currentInteractNPCData.hasCraft)
        {
            craftButton.gameObject.SetActive(true);
            interactButtons.Add(craftButton);
        }
        if (currentInteractNPCData.teleportData != null)
        {
            teleportButton.gameObject.SetActive(true);
            interactButtons.Add(teleportButton);
        }
        exitButton.gameObject.SetActive(true);
        isLastInteract = true;
    }
    public void SetQuestTalkId()
    {
        questId = 0;
        talkIndex = 1;
        isSubInteract = true;
        talkUi.TypingStart(questTalkText);
        CloseInteractButtons();
        interactButtons.Clear();
        questUI.SetUI(npcId);
    }
    public void SetTeleportTalkId()
    {
        questId = 0;
        talkIndex = 3;
        isSubInteract = true;
        talkUi.TypingStart(teleportTalkText);
        CloseInteractButtons();
        interactButtons.Clear();
        teleportUI.OpenUI();
    }
    public void SetQuestTalk(int _questId, int _questProgress)
    {
        questId = _questId;
        questProgress = _questProgress;
        talkIndex = 0;
        isSubInteract = false;
        talkUi.TypingEnd();
        if (talkDB.TryGetValue((npcId, questId, questProgress, talkIndex), out var talkTuple))
        {
            talkUi.TypingStart(talkTuple.talk, talkTuple.talkNpcId);
        }
        interactButtons.Clear();
        questUI.CloseUI();
    }
    public void TalkToQuest(int _questId)
    {
        QuestManager.Instance.Talk(_questId, npcId);
    }
    public void CloseUI()
    {
        CloseInteractButtons();
        questId = 0;
        questProgress = 0;
        interactButtons.Clear();
        teleportUI.CloseUI();
        questUI.CloseUI();
        talkUi.CloseUI();
        npcCamera.CloseNpcCam();
        GameManager.Instance.GameModeChange(GameMode.ControllMode);
    }
    void CloseInteractButtons()
    {
        questButton.gameObject.SetActive(false);
        shopButton.gameObject.SetActive(false);
        craftButton.gameObject.SetActive(false);
        teleportButton.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);
    }

    public void TalkDataConverter(List<TalkData> _talkData)
    {
        talkDB.Clear();
        for(int i = 0; i < _talkData.Count; i++)
        {
            talkDB.Add((npcId, _talkData[i].questId, _talkData[i].questProgress, _talkData[i].talkIndex), (_talkData[i].talkNpcId, _talkData[i].talk));
        }
    }

    
}
