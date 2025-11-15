using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

[CustomEditor(typeof(Player_Base), true)]
public class PlayerStateEditor : Editor
{
    List<string> stateNames = new List<string>();
    int[] stateHashes;
    int selectedIndex;
    Animator animator;
    void OnEnable()
    {
        Player_Base baseState = (Player_Base)target;
        if (animator == null)
        {
            animator = baseState.transform.parent.GetComponentInChildren<Animator>();
        }
        Init();
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        Player_Base baseState = (Player_Base)target;

        selectedIndex = System.Array.IndexOf(stateHashes, baseState.animationStateHash);
        if (selectedIndex < 0)
        {
            selectedIndex = 0;
        }

        if (stateNames != null && stateNames.Count > 0)
        {
            EditorGUI.BeginChangeCheck();
            selectedIndex = EditorGUILayout.Popup("Animation", selectedIndex, stateNames.ToArray());
            if (EditorGUI.EndChangeCheck())
            {
                baseState.SetHash(stateHashes[selectedIndex]);
                EditorUtility.SetDirty(baseState);
            }
        }
        base.OnInspectorGUI();
    }
    void Init()
    {
        stateNames.Clear();
        if (animator != null)
        {
            AnimatorController controller = (AnimatorController)animator.runtimeAnimatorController;

            if (controller != null)
            {
                foreach (var layer in controller.layers)
                {
                    foreach (var state in layer.stateMachine.states)
                    {
                        GetStateNames(layer.stateMachine, stateNames);
                    }
                }
            }

            stateNames = stateNames.Distinct().ToList();
            stateHashes = stateNames.Select(Animator.StringToHash).ToArray();
        }
    }
    void GetStateNames(AnimatorStateMachine _stateMachine, List<string> _stateNames)
    {
        foreach (var state in _stateMachine.states)
        {
            stateNames.Add(state.state.name);
        }

        foreach (var subStateMachine in _stateMachine.stateMachines)
        {
            GetStateNames(subStateMachine.stateMachine, _stateNames);
        }
    }
}
