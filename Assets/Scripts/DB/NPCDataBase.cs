using System.Collections.Generic;
using UnityEngine;

public class NPCDataBase : MonoBehaviour
{
    public static readonly Dictionary<int, (string name, int type)> NPCDB = new Dictionary<int, (string name, int type)>()
    {
         {1, ("테스트NPC1", 0)},
         {2, ("테스트NPC2", 1)},
         {3, ("테스트NPC3", 2)},
         {4, ("전직도우미", 3)},
         {5, ("직업관1", 3)},
         {6, ("직업관2", 3)},
         {7, ("직업관3", 3)},
         {8, ("직업관4", 3)},
         {9, ("직업관5", 3)}
    };
}