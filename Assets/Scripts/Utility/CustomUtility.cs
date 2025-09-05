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
    /// y=0인 Vector3을 반환하는 유틸리티 함수
    /// </summary>
    /// <param name="_pos">변환하고자 하는 vector3</param>
    /// <returns>y=0으로 변환된 벡터3</returns>
    public static Vector3 FlattenY(Vector3 _pos)
    {
        return new Vector3(_pos.x, 0f, _pos.z);
    }

    /// <summary>
    /// 두 좌표사이 거리가 특정 값 이상인지 확인하는 유틸리티 함수
    /// </summary>
    /// <param name="_pos1">첫번째 좌표값</param>
    /// <param name="_pos2">두번째 좌표값</param>
    /// <param name="_sqrDistance">특정 거리</param>
    /// <returns>특정값 이상일 경우 true</returns>
    public static bool CheckSqrDistance(Vector3 _pos1, Vector3 _pos2, float _sqrDistance)
    {
        Vector3 pos1 = _pos1;
        Vector3 pos2 = _pos2;
        pos1.y = 0;
        pos2.y = 0;
        return (pos1 - pos2).sqrMagnitude <= _sqrDistance;
    }

    /// <summary>
    /// 두 좌표가 특정 높이 차를 넘기는지 확인하는 유틸리티 함수
    /// </summary>
    /// <param name="_ypos1">첫번째 좌표값</param>
    /// <param name="_ypos2">두번째 좌표값</param>
    /// <param name="_limit">특정 높이</param>
    /// <returns>특정 값 이상일 경우 true</returns>
    public static bool CheckHeightDifference(float _ypos1, float _ypos2, float _limit)
    {
        return Mathf.Abs(_ypos1 - _ypos2) <= _limit;
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
        Vector3 forward = FlattenY(_forward);
        Vector3 dir = FlattenY(_pos2 - _pos1);
        return Vector3.SignedAngle(forward, dir, Vector3.up);
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
    /// 정면을 기준으로 상대와의 각도가 _angle1 과 _angle2 사이인지 확인하는 유틸리티 함수
    /// </summary>
    /// <param name="_angle1">각도1</param>
    /// <param name="_angle2">각도2</param>
    /// <param name="_forward">_pos1의 transform.forward</param>
    /// <param name="_pos1">자신의 좌표</param>
    /// <param name="_pos2">상대의 좌표</param>
    /// <returns>각도차가 _angle1과 _angle2 사이면 true</returns>
    public static bool CheckAngle(float _angle1, float _angle2, Vector3 _forward, Vector3 _pos1, Vector3 _pos2)
    {
        float angle = GetAngle(_forward, _pos1, _pos2);
        return _angle1 <= angle && angle <= _angle2;
    }

    /* 부채꼴범위
     * 플레이어용 = Angle
     * 몬스터용 = Range
     */
    public static bool IsInCircularSectorAngle(Vector3 _forward, Vector3 _controlPos, Vector3 _otherPos, float _halfSector, float _yposLimit)
    { 
        if (!CheckHeightDifference(_controlPos.y, _otherPos.y, _yposLimit)) return false; // ypos차이
        if (!CheckNormalAngle(_halfSector, _forward, _controlPos, _otherPos)) return false;  // 각도
        return true;
    }
    public static bool IsInCircularSectorRange(Vector3 _forward, Vector3 _controlPos, Vector3 _otherPos, float _sqrDistance, float _halfSector, float _yposLimit)
    {
        if (!CheckHeightDifference(_controlPos.y, _otherPos.y, _yposLimit)) return false; // ypos차이
        if (!CheckSqrDistance(_controlPos, _otherPos, _sqrDistance)) return false;  // 거리
        if (!CheckNormalAngle(_halfSector, _forward, _controlPos, _otherPos)) return false;  // 각도
        return true;
    }
    public static bool IsInCircleRange(Vector3 _controlPos, Vector3 _otherPos, float _sqrDistance, float _yposLimit)
    {
        if (!CheckHeightDifference(_controlPos.y, _otherPos.y, _yposLimit)) return false;
        if (!CheckSqrDistance(_controlPos, _otherPos, _sqrDistance)) return false;
        return true;
    }
}
