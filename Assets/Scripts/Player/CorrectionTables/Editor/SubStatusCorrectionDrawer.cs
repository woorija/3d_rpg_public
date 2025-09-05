#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

[CustomPropertyDrawer(typeof(SubStatusCorrection))]
public class SubStatusCorrectionDrawer : PropertyDrawer
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
    private readonly List<string> tempDisplayOptions = new List<string>(32);
    private readonly List<int> tempOptionValues = new List<int>(32);
    private readonly List<GUIContent> tempGuiContents = new List<GUIContent>(32);

    // 속도 향상을 위한 GUIContent 캐시
    private static readonly GUIContent[] cachedGuiContents = new GUIContent[System.Enum.GetValues(typeof(StatusType)).Length];

    // 초기화 메서드 (static 생성자로 대체 가능)
    static SubStatusCorrectionDrawer()
    {
        // GUIContent 캐시 초기화
        foreach (StatusType type in System.Enum.GetValues(typeof(StatusType)))
        {
            cachedGuiContents[(int)type] = new GUIContent(System.Enum.GetName(typeof(StatusType), type));
        }
    }

    // 현재 사용 중인 타입을 효율적으로 가져오는 메서드
    private HashSet<int> GetUsedStatusTypes(SerializedProperty property)
    {
        tempUsedTypes.Clear();

        // 상위 배열 찾기
        SerializedProperty arrayProp = GetParentArrayProperty(property);
        if (arrayProp == null) return tempUsedTypes;

        // 현재 요소의 인덱스 찾기
        int currentIndex = FindElementIndex(property);
        if (currentIndex < 0) return tempUsedTypes;

        // 현재 요소를 제외한 모든 타입 수집
        for (int i = 0; i < arrayProp.arraySize; i++)
        {
            if (i != currentIndex)
            {
                var statusTypeProp = arrayProp.GetArrayElementAtIndex(i).FindPropertyRelative("statusType");
                tempUsedTypes.Add(statusTypeProp.enumValueIndex);
            }
        }

        return tempUsedTypes;
    }

    // 속한 배열에서 현재 요소의 인덱스를 찾는 메서드
    private int FindElementIndex(SerializedProperty property)
    {
        SerializedProperty arrayProp = GetParentArrayProperty(property);
        if (arrayProp == null) return -1;

        string propertyPath = property.propertyPath;

        // 경로 비교 대신 직접 참조 비교를 통한 개선
        for (int i = 0; i < arrayProp.arraySize; i++)
        {
            if (arrayProp.GetArrayElementAtIndex(i).propertyPath == propertyPath)
            {
                return i;
            }
        }

        return -1;
    }

    // 현재 요소가 속한 배열 프로퍼티를 반환하는 메서드
    private SerializedProperty GetParentArrayProperty(SerializedProperty property)
    {
        string propertyPath = property.propertyPath;
        int lastDotIndex = propertyPath.LastIndexOf('.');

        if (lastDotIndex < 0) return null;

        // 최적화: 반복적 프로퍼티 탐색 대신 직접 경로 유도
        string parentPath = propertyPath.Substring(0, lastDotIndex);
        SerializedProperty parentProp = property.serializedObject.FindProperty(parentPath);

        // 배열 프로퍼티 찾기
        while (parentProp != null && !parentProp.isArray)
        {
            lastDotIndex = parentPath.LastIndexOf('.');
            if (lastDotIndex < 0) return null;

            parentPath = parentPath.Substring(0, lastDotIndex);
            parentProp = property.serializedObject.FindProperty(parentPath);
        }

        return parentProp;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        SerializedProperty statusTypeProp = property.FindPropertyRelative("statusType");
        SerializedProperty valuePerPointProp = property.FindPropertyRelative("valuePerPoint");

        // 현재 타입 값 가져오기
        int currentTypeIndex = statusTypeProp.enumValueIndex;

        // 레이아웃 계산
        float lineHeight = EditorGUIUtility.singleLineHeight;
        float buttonWidth = 20f;

        // 드롭다운 옵션 준비
        tempDisplayOptions.Clear();
        tempOptionValues.Clear();
        tempGuiContents.Clear();

        // 현재 선택된 값 추가
        string statusName = statusTypeProp.enumDisplayNames[currentTypeIndex];
        tempDisplayOptions.Add(statusName);
        tempOptionValues.Add(currentTypeIndex);
        tempGuiContents.Add(cachedGuiContents[currentTypeIndex] ?? new GUIContent(statusName));

        // 현재 사용 중인 타입 목록 가져오기
        var usedTypes = GetUsedStatusTypes(property);

        // 사용 가능한 타입 추가
        foreach (var type in allowedSubStatusTypes)
        {
            int enumIndex = (int)type;
            if (enumIndex != currentTypeIndex && !usedTypes.Contains(enumIndex))
            {
                tempDisplayOptions.Add(System.Enum.GetName(typeof(StatusType), type));
                tempOptionValues.Add(enumIndex);
                tempGuiContents.Add(cachedGuiContents[enumIndex]);
            }
        }

        // 컨트롤 영역 계산
        Rect statusTypeRect = new Rect(position.x, position.y, position.width * 0.65f, lineHeight);
        Rect valueRect = new Rect(
            position.x + position.width * 0.65f + 5f,
            position.y,
            position.width * 0.35f - 5f - buttonWidth,
            lineHeight);
        Rect buttonRect = new Rect(
            position.x + position.width - buttonWidth,
            position.y,
            buttonWidth,
            lineHeight);

        // 드롭다운 표시
        EditorGUI.BeginChangeCheck();
        int popupIndex = EditorGUI.Popup(statusTypeRect, 0, tempGuiContents.ToArray());

        // 값이 변경된 경우에만 처리
        if (EditorGUI.EndChangeCheck() && popupIndex > 0)
        {
            statusTypeProp.enumValueIndex = tempOptionValues[popupIndex];
            property.serializedObject.ApplyModifiedProperties();
        }

        // 값 필드 표시
        EditorGUI.BeginChangeCheck();
        float newValue = EditorGUI.FloatField(valueRect, valuePerPointProp.floatValue);
        if (EditorGUI.EndChangeCheck())
        {
            valuePerPointProp.floatValue = newValue;
            property.serializedObject.ApplyModifiedProperties();
        }

        // X 버튼 표시
        if (GUI.Button(buttonRect, "X"))
        {
            // 나중에 삭제를 위해 필요한 정보 저장
            SerializedProperty parentArray = GetParentArrayProperty(property);
            int elementIndex = FindElementIndex(property);

            if (parentArray != null && elementIndex >= 0)
            {
                // 배열에서 요소 제거
                parentArray.DeleteArrayElementAtIndex(elementIndex);
                property.serializedObject.ApplyModifiedProperties();
            }
        }

        EditorGUI.EndProperty();
    }
}
#endif