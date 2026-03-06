using UnityEngine;

[DisallowMultipleComponent]
public class AttackRangeGizmoDrawer : MonoBehaviour
{
    [SerializeField] private AttackDataSO attackData;

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (attackData == null)
        {
            return;
        }

        Vector3 origin = GetAttackOrigin();

        switch (attackData.AttackType)
        {
            case AttackType.Circle:
                DrawCircle(origin);
                break;

            case AttackType.Sector:
                DrawSector(origin);
                break;

            case AttackType.Box:
                DrawBox(origin);
                break;
        }
    }

    // =========================================================
    // Origin
    // =========================================================

    private Vector3 GetAttackOrigin()
    {
        return transform.position
            + transform.right * attackData.Pos.x
            + transform.up * attackData.Pos.y
            + transform.forward * attackData.Pos.z;
    }

    // =========================================================
    // Circle
    // =========================================================

    private void DrawCircle(Vector3 origin)
    {
        DrawCircleRings(origin);
        DrawHeightLines(origin);
    }

    private void DrawCircleRings(Vector3 origin)
    {
        Gizmos.color = Color.green;

        Vector3 lower = origin + Vector3.up * attackData.YLower;
        Vector3 upper = origin + Vector3.up * attackData.YUpper;

        // outer
        DrawWireCircle(lower, attackData.OuterRadius);
        DrawWireCircle(upper, attackData.OuterRadius);

        // inner
        if (attackData.InnerRadius > 0f)
        {
            DrawWireCircle(lower, attackData.InnerRadius);
            DrawWireCircle(upper, attackData.InnerRadius);
        }
    }

    private void DrawHeightLines(Vector3 origin)
    {
        Gizmos.color = Color.green;

        Vector3 lower = origin + Vector3.up * attackData.YLower;
        Vector3 upper = origin + Vector3.up * attackData.YUpper;

        Vector3[] dirs =
        {
            transform.forward,
            -transform.forward,
            transform.right,
            -transform.right
        };

        foreach (Vector3 dir in dirs)
        {
            Vector3 offset = dir.normalized * attackData.OuterRadius;
            Gizmos.DrawLine(lower + offset, upper + offset);
        }
    }

    // =========================================================
    // Sector
    // =========================================================

    private void DrawSector(Vector3 origin)
    {
        // Circle 기반 표현
        DrawCircle(origin);

        // 각도 경계선
        DrawSectorAngleLines(origin);
    }

    private void DrawSectorAngleLines(Vector3 origin)
    {
        if (attackData.Angles == null || attackData.Angles.Count == 0)
        {
            return;
        }

        Gizmos.color = Color.yellow;

        Vector3 lowerPos = origin + Vector3.up * attackData.YLower;
        Vector3 upperPos = origin + Vector3.up * attackData.YUpper;

        foreach (AngleRange angle in attackData.Angles)
        {
            DrawAngleLine(lowerPos, angle.minAngle);
            DrawAngleLine(lowerPos, angle.maxAngle);
            DrawAngleLine(upperPos, angle.minAngle);
            DrawAngleLine(upperPos, angle.maxAngle);
        }
    }

    private void DrawAngleLine(Vector3 origin, float angle)
    {
        Vector3 dir =
            Quaternion.Euler(0f, angle, 0f) * transform.forward;

        float startRadius = Mathf.Max(attackData.InnerRadius, 0f);

        Vector3 from = origin + dir * startRadius;
        Vector3 to = origin + dir * attackData.OuterRadius;

        Gizmos.DrawLine(from, to);
    }

    // =========================================================
    // Box
    // =========================================================

    private void DrawBox(Vector3 origin)
    {
        Gizmos.color = Color.cyan;

        float height = attackData.YUpper - attackData.YLower;

        Vector3 size = new Vector3(
            attackData.Left + attackData.Right,
            height,
            attackData.Front + attackData.Back
        );

        Vector3 center =
            origin
            + transform.right * (attackData.Right - attackData.Left) * 0.5f
            + transform.forward * (attackData.Front - attackData.Back) * 0.5f
            + Vector3.up * (attackData.YLower + height * 0.5f);

        Gizmos.DrawWireCube(center, size);
    }

    // =========================================================
    // Utility
    // =========================================================

    private void DrawWireCircle(Vector3 center, float radius)
    {
        const int segmentCount = 64;
        float angleStep = 360f / segmentCount;

        Vector3 prevPoint = center + transform.forward * radius;

        for (int i = 1; i <= segmentCount; i++)
        {
            float angle = angleStep * i;
            Vector3 nextPoint =
                center
                + Quaternion.Euler(0f, angle, 0f) * transform.forward * radius;

            Gizmos.DrawLine(prevPoint, nextPoint);
            prevPoint = nextPoint;
        }
    }
#endif
}