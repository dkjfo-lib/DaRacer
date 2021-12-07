using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GenerateCity : MonoBehaviour
{
    public CityLevel[] levels;
    public int size = 100;
    public int cellSize = 10;
    public int foundationHeight = 1;
    [Space]
    public GameObject normalRoad;
    public GameObject normalWall;

    int levelsHeight => levels.Sum(s => s.height);
    int realSize => size * cellSize;

    float halfSize => size / 2f;
    float halfRealSize => size / 2f * cellSize;
    Vector2 GetPointOnLine(Vector2 p1, Vector2 p2, float p) => (p1 - p2) * p + p2;

    private void Start()
    {
        // Anchors
        var A_C = new Vector2(0, 0);
        var A_RT = new Vector2(+halfRealSize, +halfRealSize);
        var A_RB = new Vector2(+halfRealSize, -halfRealSize);
        var A_LT = new Vector2(-halfRealSize, +halfRealSize);
        var A_LB = new Vector2(-halfRealSize, -halfRealSize);

        // Enteries
        var E_T = GetPointOnLine(A_RT, A_LT, .5f);
        var E_B = GetPointOnLine(A_RB, A_LB, .5f);
        var E_R = GetPointOnLine(A_RT, A_RB, .5f);
        var E_L = GetPointOnLine(A_LT, A_LB, .5f);

        // Highways
        var H_TC = GetPointOnLine(E_T, A_C, .5f);
        var H_BC = GetPointOnLine(E_B, A_C, .5f);
        var H_RC = GetPointOnLine(E_R, A_C, .5f);
        var H_LC = GetPointOnLine(E_L, A_C, .5f);

        // Highways Inner
        var H_TR = GetPointOnLine(H_TC, H_RC, .5f);
        var H_TL = GetPointOnLine(H_TC, H_LC, .5f);
        var H_BR = GetPointOnLine(H_BC, H_RC, .5f);
        var H_BL = GetPointOnLine(H_BC, H_LC, .5f);

        float height = levels[0].height;
        float width = levels[0].carLength + levels[0].pedestrianLength;

        CityGenerator.GenerateStreet(E_T, H_TC, normalRoad, normalWall, transform, height, width, width / 2f);
        CityGenerator.GenerateStreet(E_B, H_BC, normalRoad, normalWall, transform, height, width, width / 2f);
        CityGenerator.GenerateStreet(E_R, H_RC, normalRoad, normalWall, transform, height, width, width / 2f);
        CityGenerator.GenerateStreet(E_L, H_LC, normalRoad, normalWall, transform, height, width, width / 2f);

        CityGenerator.GenerateStreet(H_TC, H_RC, normalRoad, normalWall, transform, height, width, width / 2f);
        CityGenerator.GenerateStreet(H_TC, H_LC, normalRoad, normalWall, transform, height, width, width / 2f);
        CityGenerator.GenerateStreet(H_BC, H_RC, normalRoad, normalWall, transform, height, width, width / 2f);
        CityGenerator.GenerateStreet(H_BC, H_LC, normalRoad, normalWall, transform, height, width, width / 2f);

        CityGenerator.GenerateIntersection(H_TC, new Vector2[] { H_RC, H_LC, E_T }, normalRoad, normalWall, transform, height, width, width / 2f);
        CityGenerator.GenerateIntersection(H_BC, new Vector2[] { H_LC, H_RC, E_B }, normalRoad, normalWall, transform, height, width, width / 2f);
        CityGenerator.GenerateIntersection(H_RC, new Vector2[] { H_BC, H_TC, E_R }, normalRoad, normalWall, transform, height, width, width / 2f);
        CityGenerator.GenerateIntersection(H_LC, new Vector2[] { H_TC, H_BC, E_L }, normalRoad, normalWall, transform, height, width, width / 2f);
    }

    private void OnDrawGizmos()
    {
        Vector3 dimention = new Vector3(realSize, levelsHeight + foundationHeight, realSize);
        Vector3 center = transform.position + new Vector3(0, levelsHeight / 2f - foundationHeight / 2f, 0);
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(center, dimention);

        Gizmos.color = Color.white;
        for (int x = 0; x < size; x++)
        {
            for (int z = 0; z < size; z++)
            {
                dimention = new Vector3(cellSize, dimention.y, cellSize);
                var offset = new Vector3((x - (size - 1) / 2f) * cellSize, center.y, (z - (size - 1) / 2f) * cellSize);
                center = transform.position + offset;
                Gizmos.DrawWireCube(center, dimention);
            }
        }

        var height = 0f;
        for (int i = 0; i < levels.Length; i++)
        {
            Gizmos.color = new Color(1, (float)i / levels.Length, (float)i / levels.Length);
            dimention = new Vector3(realSize, levels[i].height, realSize);
            center = transform.position + new Vector3(0, height + levels[i].height / 2f, 0);
            Gizmos.DrawWireCube(center, dimention);
            height += levels[i].height;
        }
    }
}

[System.Serializable]
public class CityLevel
{
    public int height = 10;
    public int carLength = 10;
    public int pedestrianLength = 0;
}

public class CityGenerator
{
    public static void GenerateStreet(Vector2 p1, Vector2 p2, GameObject normalizedRoad, GameObject normalizedWall, Transform parent, float wallHeight, float roadWidth, float crosssectionRadius)
    {
        var street = new GameObject("street " + Random.Range(0, 100));
        street.transform.parent = parent;

        Vector2 diff = p1 - p2;
        p1 = p1 - diff.normalized * crosssectionRadius;
        p2 = p2 + diff.normalized * crosssectionRadius;

        GenerateRoad(p1, p2, normalizedRoad, parent, roadWidth).transform.parent = street.transform;
        GenerateStreetWall(p1, p2, normalizedWall, parent, wallHeight, roadWidth, true).transform.parent = street.transform;
        GenerateStreetWall(p1, p2, normalizedWall, parent, wallHeight, roadWidth, false).transform.parent = street.transform;
    }

    static GameObject GenerateRoad(Vector2 p1, Vector2 p2, GameObject normalizedRoad, Transform parent, float roadWidth)
    {
        Vector3 diff = new Vector3(p1.x - p2.x, 0, p1.y - p2.y);
        var newRoad = GameObject.Instantiate(normalizedRoad);
        newRoad.transform.parent = parent;
        newRoad.transform.localPosition = parent.position + new Vector3(diff.x / 2f + p2.x, 0, diff.z / 2f + p2.y);
        newRoad.transform.localScale = new Vector3(roadWidth, 1, diff.magnitude);
        newRoad.transform.localRotation = Quaternion.LookRotation(diff, Vector2.up);
        return newRoad;
    }

    static GameObject GenerateStreetWall(Vector2 p1, Vector2 p2, GameObject normalizedWall, Transform parent, float wallHeight, float roadWidth, bool isRightSide)
    {
        var newWall = GenerateWall(p1, p2, normalizedWall, parent, wallHeight);
        newWall.transform.localPosition += isRightSide ? newWall.transform.right * roadWidth / 2f : -newWall.transform.right * roadWidth / 2f;
        return newWall;
    }

    static GameObject GenerateWall(Vector2 p1, Vector2 p2, GameObject normalizedWall, Transform parent, float wallHeight)
    {
        Vector3 diff = new Vector3(p1.x - p2.x, 0, p1.y - p2.y);
        var newWall = GameObject.Instantiate(normalizedWall);
        newWall.transform.parent = parent;
        newWall.transform.localPosition = parent.position + new Vector3(diff.x / 2f + p2.x, wallHeight / 2f, diff.z / 2f + p2.y);
        newWall.transform.localScale = new Vector3(.1f, wallHeight, diff.magnitude);
        newWall.transform.localRotation = Quaternion.LookRotation(diff, Vector2.up);
        return newWall;
    }

    public static void GenerateIntersection(Vector2 pc, Vector2[] incomingNodesClockwise, GameObject normalizedRoad, GameObject normalizedWall, Transform parent, float wallHeight, float roadWidth, float crosssectionRadius)
    {
        var intersection = new GameObject("intersection " + Random.Range(0, 100));
        intersection.transform.parent = parent;

        Vector2[] rightBorders = new Vector2[incomingNodesClockwise.Length];
        Vector2[] leftBorders = new Vector2[incomingNodesClockwise.Length];
        for (int i = 0; i < incomingNodesClockwise.Length; i++)
        {
            var dir = (incomingNodesClockwise[i] - pc).normalized;
            float halfWidth = roadWidth / 2f;
            var dirNormal = new Vector2(dir.y, -dir.x);
            leftBorders[i] = pc + dir * crosssectionRadius + dirNormal * halfWidth;
            rightBorders[i] = pc + dir * crosssectionRadius - dirNormal * halfWidth;
            //GenerateWall(leftBorders[i], rightBorders[i], normalizedWall, intersection.transform, wallHeight);
        }

        for (int i = 0; i < incomingNodesClockwise.Length - 1; i++)
        {
            GenerateWall(leftBorders[i], rightBorders[i + 1], normalizedWall, parent, wallHeight).transform.parent = intersection.transform;
        }
        GenerateWall(leftBorders[incomingNodesClockwise.Length - 1], rightBorders[0], normalizedWall, parent, wallHeight).transform.parent = intersection.transform;
    }
}

class CityPlanner
{
    public CityPlan GetCityPlan()
    {
        var cityPlan = new CityPlan();
        return cityPlan;
    }
}

public class CityPlan
{
    public Vector2 points;
    public int[][] connectionMask;
}