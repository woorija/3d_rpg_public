#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

[CustomEditor(typeof(BT_ChangeAnimation))]
public class BT_ChangeAnimationEditor : ActionNodeEditor
{
    List<string> stateNames = new List<string>();
    int[] stateHashes;
    int selectedAnimationStateIndex;
    Animator animator;
    protected override void OnEnable()
    {
        base.OnEnable();
        BT_ChangeAnimation node = (BT_ChangeAnimation)target;
        if(animator == null)
        {
            var bt = node.GetComponentInParent<BehaviorTree>();
            if (bt != null)
            {
                animator = node.GetComponentInParent<BehaviorTree>().GetAnimator();
            }
        }
        Init();
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        BT_ChangeAnimation node = (BT_ChangeAnimation)target;

        if (stateHashes != null && stateHashes.Length != 0)
        {
            selectedAnimationStateIndex = System.Array.IndexOf(stateHashes, node.animationStateHash);
            if (selectedAnimationStateIndex < 0)
            {
                selectedAnimationStateIndex = 0;
            }

            if (stateNames != null && stateNames.Count > 0)
            {
                EditorGUI.BeginChangeCheck();
                selectedAnimationStateIndex = EditorGUILayout.Popup("Animation", selectedAnimationStateIndex, stateNames.ToArray());
                if (EditorGUI.EndChangeCheck())
                {
                    node.SetHash(stateHashes[selectedAnimationStateIndex]);
                    EditorUtility.SetDirty(node);
                }
            }
        }
        else
        {
            EditorGUILayout.LabelField("애니메이터가 존재하지 않음");
        }

        EditorGUI.BeginChangeCheck();
        bool useCrossFade = EditorGUILayout.Toggle("Use CrossFade", node.useCrossFade);
        if (EditorGUI.EndChangeCheck())
        {
            node.SetUseCrossFade(useCrossFade);
            EditorUtility.SetDirty(node);
        }

        if (node.useCrossFade)
        {
            EditorGUI.BeginChangeCheck();
            float crossFadeTime = EditorGUILayout.Slider("CrossFade Time", node.crossFadeTime, 0f, 1f);
            if (EditorGUI.EndChangeCheck())
            {
                node.SetCrossFadeTime(crossFadeTime);
                EditorUtility.SetDirty(node);
            }
        }

        base.OnInspectorGUI();
        serializedObject.ApplyModifiedProperties();
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
#endif