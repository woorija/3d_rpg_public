#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CustomPropertyDrawer(typeof(BaseStatusCorrection))]
public class BaseStatusCorrectionDrawer : PropertyDrawer
{
    // 정적 캐시로 컴파일 시 한 번만 초기화
    private static readonly StatusType[] allowedSubStatusTypes = new[] {
        StatusType.PhysicalAttackPower,
        StatusType.MagicAttackPower,
        StatusType.PhysicalDefensePower,
        StatusType.MagicDefensePower,
        StatusType.HpRegen,
        StatusType.MpRegen,
        StatusType.Accuracy,
        StatusType.Evasion,
        StatusType.Hp,
        StatusType.Mp,
        StatusType.Stamina,
        StatusType.CriticalRate,
        StatusType.CriticalDamage,
        StatusType.WeaponMastery,
        StatusType.AttackSpeed,
        StatusType.MoveSpeed,
        StatusType.CooltimeReduce
    };

    // 재사용 가능한 임시 컬렉션
    private readonly HashSet<int> tempUsedTypes = new HashSet<int>();

    // PropertyHeight 오버라이드로 GUI 레이아웃 계산 문제 방지
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float height = EditorGUIUtility.singleLineHeight;

        // 펼침 상태인 경우에만 추가 높이 계산
        if (property.isExpanded)
        {
            SerializedProperty correctionListProp = property.FindPropertyRelative("correctionList");

            // 각 항목의 높이와 추가 버튼 높이 계산
            height += (correctionListProp.arraySize * EditorGUIUtility.singleLineHeight) +
                     (correctionListProp.arraySize * EditorGUIUtility.standardVerticalSpacing) +
                     EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        }

        return height;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty statusTypeProp = property.FindPropertyRelative("statusType");
        SerializedProperty correctionListProp = property.FindPropertyRelative("correctionList");

        // StatusType의 이름으로 라벨 변경
        string statusName = statusTypeProp.enumDisplayNames[statusTypeProp.enumValueIndex];

        // 레이아웃 계산
        float lineHeight = EditorGUIUtility.singleLineHeight;
        float verticalSpace = EditorGUIUtility.standardVerticalSpacing;

        // 폴드아웃 버튼 영역
        Rect foldoutRect = new Rect(position.x, position.y, position.width, lineHeight);
        property.isExpanded = GUI.Toggle(foldoutRect, property.isExpanded, statusName, GUI.skin.button);

        // 펼쳐진 상태일 때만 내용 표시
        if (property.isExpanded)
        {
            // 상위 항목에서 좀 더 들여쓰기
            float indent = 15f;
            Rect contentRect = new Rect(position.x + indent, position.y + lineHeight + verticalSpace,
                                        position.width - indent, lineHeight);

            // 하위 항목 표시
            for (int i = 0; i < correctionListProp.arraySize; i++)
            {
                SerializedProperty itemProp = correctionListProp.GetArrayElementAtIndex(i);

                // 항목의 높이는 항상 lineHeight로 고정
                EditorGUI.PropertyField(contentRect, itemProp, GUIContent.none);
                contentRect.y += lineHeight + verticalSpace;
            }

            // 추가 버튼 영역
            Rect buttonRect = new Rect(contentRect.x, contentRect.y, position.width - 35, lineHeight);

            // 효율적인 사용 가능 타입 목록 계산
            tempUsedTypes.Clear();
            for (int i = 0; i < correctionListProp.arraySize; i++)
            {
                var statusTypePropItem = correctionListProp.GetArrayElementAtIndex(i).FindPropertyRelative("statusType");
                tempUsedTypes.Add(statusTypePropItem.enumValueIndex);
            }

            // 사용 가능한 타입이 있는지 확인 (미리 계산하여 불필요한 반복 제거)
            bool hasAvailableTypes = allowedSubStatusTypes.Any(t => !tempUsedTypes.Contains((int)t));

            // 사용 가능한 타입이 있을 때만 버튼 활성화
            EditorGUI.BeginDisabledGroup(!hasAvailableTypes);
            if (GUI.Button(buttonRect, "Add Sub Status"))
            {
                // 새 항목 추가
                int newItemIndex = correctionListProp.arraySize;
                correctionListProp.arraySize++;
                var newItem = correctionListProp.GetArrayElementAtIndex(newItemIndex);
                var newItemStatusType = newItem.FindPropertyRelative("statusType");
                var newItemValuePerPoint = newItem.FindPropertyRelative("valuePerPoint");

                // 첫 번째 사용 가능한 타입으로 초기화
                foreach (var type in allowedSubStatusTypes)
                {
                    if (!tempUsedTypes.Contains((int)type))
                    {
                        newItemStatusType.enumValueIndex = (int)type;
                        break;
                    }
                }

                newItemValuePerPoint.floatValue = 0f;
                property.serializedObject.ApplyModifiedProperties();
            }
            EditorGUI.EndDisabledGroup();
        }

        EditorGUI.EndProperty();
    }
}
#endif