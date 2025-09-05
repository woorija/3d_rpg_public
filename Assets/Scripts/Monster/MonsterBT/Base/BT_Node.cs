using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BT_Node : MonoBehaviour
{
    //모든 노드는 최상위 몬스터 오브젝트의 BT를 가지고있어야함
    [field: SerializeField] public BehaviorTree BT { get; protected set; }
    [field: SerializeField] public int Priority { get; protected set; } //running상태의 노드 실행을 끊을때의 우선순위를 파악하기 위한 변수
    public abstract BTResult Execute();
    protected virtual void Awake()
    {
        BT = GetComponentInParent<BehaviorTree>();
    }
}
