using UnityEngine;

public static class CustomUtility
{   
    /// <summary>
    /// int형 변수의 자리수를 구하는 유틸리티 함수
    /// </summary>
    /// <param name="_num">자리수를 구하고 싶은 int형 변수</param>
    /// <returns>자리수</returns>
    public static int GetDigitCount(int _num)
    {
        if (_num == 0)
            return 1;

        _num = Mathf.Abs(_num);

        if (_num < 100000)
        {
            if (_num < 100)
            {
                return _num < 10 ? 1 : 2;
            }
            return _num < 1000 ? 3 : (_num < 10000 ? 4 : 5);
        }
        else
        {
            if (_num < 10000000)
            {
                return _num < 1000000 ? 6 : 7;
            }
            return _num < 100000000 ? 8 : (_num < 1000000000 ? 9 : 10);
        }
    }

    /// <summary>
    /// 두 좌표사이 거리가 특정 값 사이인지 확인하는 유틸리티 함수
    /// </summary>
    /// <param name="_pos1">첫번째 좌표값</param>
    /// <param name="_pos2">두번째 좌표값</param>
    /// <param name="_outerSqrDistance">특정 거리 제곱</param>
    /// <param name="_innerSqrDistance">내부 예외 거리 제곱</param>
    /// <returns>특정값 사이일 경우 true</returns>
    public static bool CheckSqrDistance(Vector3 _pos1, Vector3 _pos2, float _outerSqrDistance, float _innerSqrDistance = 0f)
    {
        _pos1.y = 0;
        _pos2.y = 0;

        float sqrDist = (_pos1 - _pos2).sqrMagnitude;

        // Dead Zone
        if (_innerSqrDistance > 0f && sqrDist < _innerSqrDistance)
            return false;

        return sqrDist <= _outerSqrDistance;
    }

    /// <summary>
    /// 두 좌표가 특정 높이 차를 넘기는지 확인하는 유틸리티 함수
    /// </summary>
    /// <param name="_ypos1">첫번째 좌표값</param>
    /// <param name="_ypos2">두번째 좌표값</param>
    /// <param name="_limit">특정 높이</param>
    /// <returns>특정 값 이내일 경우 true</returns>
    public static bool CheckHeightDifference(float _ypos1, float _ypos2, float _limit)
    {
        return Mathf.Abs(_ypos1 - _ypos2) <= _limit;
    }
    /// <summary>
    /// target의 y좌표가 base의 y좌표에 대하여 min~max사이에 있는지 확인하는 유틸리티 함수 
    /// </summary>
    /// <param name="_targetY">타겟 y좌표</param>
    /// <param name="_baseY">비교대상 y좌표</param>
    /// <param name="_minOffset">하단범위</param>
    /// <param name="_maxOffset">상단범위</param>
    /// <returns></returns>
    public static bool CheckHeightInRange(float _baseY, float _targetY, float _minOffset, float _maxOffset)
    {
        float minY = _baseY + _minOffset;
        float maxY = _baseY + _maxOffset;
        return _targetY >= minY && _targetY <= maxY;
    }
    /// <summary>
    /// 정면을 기준으로 각도차를 구하는 유틸리티 함수
    /// </summary>
    /// <param name="_forward">_pos1의 transform.forward</param>
    /// <param name="_pos1">자신의 좌표</param>
    /// <param name="_pos2">상대의 좌표</param>
    /// <returns>정면 0도 ~ 후방 180도</returns>
    public static float GetAngle(Vector3 _forward, Vector3 _pos1, Vector3 _pos2) // 기준: 정면 0도 후방 180도
    {
        // Y축을 0으로 만든 2D 벡터로 계산
        float fwdX = _forward.x;
        float fwdZ = _forward.z;

        float dirX = _pos2.x - _pos1.x;
        float dirZ = _pos2.z - _pos1.z;

        // 내적과 외적을 직접 계산
        float dot = fwdX * dirX + fwdZ * dirZ;
        float cross = fwdX * dirZ - fwdZ * dirX;

        // Atan2로 각도 계산 (정규화 불필요, -180~180 자동 처리)
        return Mathf.Atan2(cross, dot) * Mathf.Rad2Deg;
    }

    /// <summary>
    /// 정면을 기준으로 상대와의 각도가 _angle 이내인지 확인하는 유틸리티 함수
    /// </summary>
    /// <param name="_angle">각도차</param>
    /// <param name="_forward">_pos1의 transform.forward</param>
    /// <param name="_pos1">자신의 좌표</param>
    /// <param name="_pos2">상대의 좌표</param>
    /// <returns>각도가 _angle이내면 true</returns>
    public static bool CheckNormalAngle(float _angle, Vector3 _forward, Vector3 _pos1, Vector3 _pos2)
    {
        return Mathf.Abs(GetAngle(_forward, _pos1, _pos2)) <= _angle;
    }

    /// <summary>
    /// 정면을 기준으로 상대와의 각도가 _minAngle과 _maxAngle사이인지 확인하는 유틸리티 함수
    /// </summary>
    /// <param name="_angle1">각도1</param>
    /// <param name="_angle2">각도2</param>
    /// <param name="_forward">_pos1의 transform.forward</param>
    /// <param name="_pos1">자신의 좌표</param>
    /// <param name="_pos2">상대의 좌표</param>
    /// <returns>각도차가 _angle1과 _angle2 사이면 true</returns>
    public static bool CheckAngle(float _minAngle, float _maxAngle, Vector3 _forward, Vector3 _pos1, Vector3 _pos2)
    {
        float angle = GetAngle(_forward, _pos1, _pos2);
        if(_minAngle <= _maxAngle)
        {
            return _minAngle <= angle && angle <= _maxAngle;
        }
        else
        {
            return angle >= _minAngle || angle <= _maxAngle;
        }
    }
    /// <summary>
    /// angle이 min~max사이인지 확인하는 유틸리티 함수
    /// </summary>
    /// <param name="_minAngle">각도1</param>
    /// <param name="_maxAngle">각도2</param>
    /// <param name="_angle">확인하고자 하는 각도</param>
    /// <returns></returns>
    public static bool CheckAngle(float _minAngle, float _maxAngle, float _angle)
    {
        if (_minAngle <= _maxAngle)
        {
            return _minAngle <= _angle && _angle <= _maxAngle;
        }
        else
        {
            return _angle >= _minAngle || _angle <= _maxAngle;
        }
    }
    /* 
     * 플레이어용 부채꼴 범위 계산
     */
    public static bool IsInCircularSectorAngle(Vector3 _forward, Vector3 _controlPos, Vector3 _otherPos, float _minAngle, float _maxAngle, float _yLower, float _yUpper)
    {
        if (!CheckHeightInRange(_controlPos.y, _otherPos.y, _yLower, _yUpper)) return false; // ypos차이
        if (!CheckAngle(_minAngle, _maxAngle, _forward, _controlPos, _otherPos)) return false;  // 각도
        return true;
    }
}
