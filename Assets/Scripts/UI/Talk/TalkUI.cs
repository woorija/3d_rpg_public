using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.Events;

public class TalkUI : MonoBehaviour
{
    [SerializeField] GameObject UIObject;
    [SerializeField] TMP_Text npcName;
    [SerializeField] TMP_Text talkText;
    
    bool isTyping = false;

    [SerializeField] UnityEvent onTalkEnd;

    string talk = string.Empty;
    public bool GetObjectActiveSelf()
    {
        return UIObject.activeSelf;
    }

    public void OpenUI()
    {
        UIObject.SetActive(true);
    }
    public void CloseUI()
    {
        UIObject.SetActive(false);
    }
    public void TypingStart(string _talk, int _talkNpcId = -1)
    {
        isTyping = true;
        SetName(_talkNpcId);
        talk = _talk;
        talkText.text = string.Empty;
        talkText.DOText(talk, talk.Length * 0.05f).SetEase(Ease.Linear).OnComplete(TypingComplete);
    }
    public void TypingEnd()
    {
        talkText.DOKill();
        isTyping = false;
        talkText.text = talk;
    }
    void TypingComplete()
    {
        isTyping = false;
        onTalkEnd.Invoke();
    }
    public bool IsTyping()
    {
        return isTyping;
    } 
    void SetName(int _npcId)
    {
        if (_npcId == -1) return;
        npcName.text = NPCDataBase.NPCDB[_npcId].name;
    }
}
