using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ph = PlannerHelper;

public class GenerateCity_v2 : MonoBehaviour
{
    int size = 3;
    int cellSize = 10;
    [Range(0, .49f)] float cellOffset = .1f;

    public List<Vector2> centers;
    public List<Vector2> points;
    public List<EndLine> lines;
    float pointsDistanceFreshhold => cellSize / 5f;

    [Space]
    public float generateEvery = 5f;
    [Space]
    public float sizeP = .3f;
    public float sizeC = .25f;
    public bool wire = false;

    private void Start()
    {
        StartCoroutine(GenerateOnTimer());
    }

    IEnumerator GenerateOnTimer()
    {
        while (true)
        {
            Generate();
            yield return new WaitForSeconds(generateEvery);
        }
    }

    void Generate()
    {
        centers = new List<Vector2>();
        points = new List<Vector2>();
        lines = new List<EndLine>();

        Vector2[,] cellCenters = new Vector2[size, size];
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                cellCenters[x, y] = new Vector2(
                    Random.Range(cellOffset, 1 - cellOffset) + x, 
                    Random.Range(cellOffset, 1 - cellOffset) + y);
                centers.Add(cellCenters[x, y]);
            }
        }
        for (int x = 1; x < size - 1; x++)
        {
            for (int y = 1; y < size - 1; y++)
            {
                Vector2[] cellMidpoints = new Vector2[8];
                int i = 0;
                cellMidpoints[i++] = ph.GetMidpoint(cellCenters[x, y], cellCenters[x + 1, y + 0]);
                cellMidpoints[i++] = ph.GetMidpoint(cellCenters[x, y], cellCenters[x - 1, y + 0]);
                cellMidpoints[i++] = ph.GetMidpoint(cellCenters[x, y], cellCenters[x + 0, y + 1]);
                cellMidpoints[i++] = ph.GetMidpoint(cellCenters[x, y], cellCenters[x + 0, y - 1]);

                cellMidpoints[i++] = ph.GetMidpoint(cellCenters[x, y], cellCenters[x + 1, y + 1]);
                cellMidpoints[i++] = ph.GetMidpoint(cellCenters[x, y], cellCenters[x + 1, y - 1]);
                cellMidpoints[i++] = ph.GetMidpoint(cellCenters[x, y], cellCenters[x - 1, y + 1]);
                cellMidpoints[i++] = ph.GetMidpoint(cellCenters[x, y], cellCenters[x - 1, y - 1]);

                ILine[] Normals = new ILine[8];
                for (i = 0; i < 8; i++)
                {
                    Normals[i] = ph.GetNormalLine(new EndLine { start = cellMidpoints[i], end = cellCenters[x, y] });
                }

                Vector2[] primariIntersections = new Vector2[4];
                primariIntersections[0] = ph.GetLineIntersection(Normals[0], Normals[2]);
                primariIntersections[1] = ph.GetLineIntersection(Normals[0], Normals[3]);
                primariIntersections[2] = ph.GetLineIntersection(Normals[1], Normals[2]);
                primariIntersections[3] = ph.GetLineIntersection(Normals[1], Normals[3]);

                Vector2[] secondary = new Vector2[8];
                if (ph.GetDistance(cellCenters[x, y], primariIntersections[0]) >
                    ph.GetDistance(cellCenters[x, y], cellMidpoints[4]))
                {
                    secondary[0] = ph.GetLineIntersection(Normals[4], Normals[0]);
                    secondary[1] = ph.GetLineIntersection(Normals[4], Normals[2]);
                }
                else
                {
                    secondary[0] = primariIntersections[0];
                    secondary[1] = primariIntersections[0];
                }

                if (ph.GetDistance(cellCenters[x, y], primariIntersections[1]) >
                    ph.GetDistance(cellCenters[x, y], cellMidpoints[5]))
                {
                    secondary[2] = ph.GetLineIntersection(Normals[5], Normals[0]);
                    secondary[3] = ph.GetLineIntersection(Normals[5], Normals[3]);
                }
                else
                {
                    secondary[2] = primariIntersections[1];
                    secondary[3] = primariIntersections[1];
                }

                if (ph.GetDistance(cellCenters[x, y], primariIntersections[2]) >
                    ph.GetDistance(cellCenters[x, y], cellMidpoints[6]))
                {
                    secondary[4] = ph.GetLineIntersection(Normals[6], Normals[1]);
                    secondary[5] = ph.GetLineIntersection(Normals[6], Normals[2]);
                }
                else
                {
                    secondary[4] = primariIntersections[2];
                    secondary[5] = primariIntersections[2];
                }

                if (ph.GetDistance(cellCenters[x, y], primariIntersections[3]) >
                    ph.GetDistance(cellCenters[x, y], cellMidpoints[7]))
                {
                    secondary[6] = ph.GetLineIntersection(Normals[7], Normals[1]);
                    secondary[7] = ph.GetLineIntersection(Normals[7], Normals[3]);
                }
                else
                {
                    secondary[6] = primariIntersections[3];
                    secondary[7] = primariIntersections[3];
                }

                points.AddRange(secondary);
                lines.Add(new EndLine { start = secondary[0], end = secondary[2] });
                lines.Add(new EndLine { start = secondary[4], end = secondary[6] });

                lines.Add(new EndLine { start = secondary[3], end = secondary[7] });
                lines.Add(new EndLine { start = secondary[1], end = secondary[5] });

                lines.Add(new EndLine { start = secondary[0], end = secondary[1] });
                lines.Add(new EndLine { start = secondary[2], end = secondary[3] });
                lines.Add(new EndLine { start = secondary[4], end = secondary[5] });
                lines.Add(new EndLine { start = secondary[6], end = secondary[7] });
            }
        }
    }
    private void OnDrawGizmos()
    {
        if (points != null)
        {
            Gizmos.color = Color.yellow;
            foreach (var p in points)
            {
                if (wire)
                    Gizmos.DrawWireSphere(new Vector3(p.x, 0, p.y), sizeP);
                else
                    Gizmos.DrawSphere(new Vector3(p.x, 0, p.y), sizeP);
            }
        }
        if (centers != null)
        {
            Gizmos.color = Color.green;
            foreach (var p in centers)
            {
                if (wire)
                    Gizmos.DrawWireSphere(new Vector3(p.x, 0, p.y), sizeC);
                else
                    Gizmos.DrawSphere(new Vector3(p.x, 0, p.y), sizeC);
            }
        }
        if (lines != null)
        {
            Gizmos.color = Color.green;
            foreach (var l in lines)
            {
                Gizmos.DrawLine(new Vector3(l.Start.x, 0, l.Start.y), new Vector3(l.End.x, 0, l.End.y));
            }
        }
    }
}

public interface ILine
{
    Vector2 Start { get; }
    Vector2 End { get; }
    Vector2 Direction { get; }
}
[System.Serializable]
public class Line : ILine
{
    public Vector2 start;
    public Vector2 direction;

    public Vector2 Start => start;
    public Vector2 End => start + direction;
    public Vector2 Direction => direction;
}
[System.Serializable]
public class EndLine : ILine
{
    public Vector2 start;
    public Vector2 end;

    public Vector2 Start => start;
    public Vector2 End => end;
    public Vector2 Direction => (end - start).normalized;
}
public static class PlannerHelper
{

    public static Vector2 GetNormalDirection(Vector2 v) => new Vector2(v.y, -v.x);
    public static ILine GetNormalLine(ILine l) => new Line
    {
        start = l.Start,
        direction = new Vector2(l.Direction.y, -l.Direction.x)
    };

    public static Vector2 GetMidpoint(EndLine l) => GetPointOnLine(l, .5f);
    public static Vector2 GetMidpoint(Vector2 p1, Vector2 p2) => GetPointOnLine(p1, p2, .5f);

    public static Vector2 GetPointOnLine(EndLine l, float p) => (l.end - l.start) * p + l.start;
    public static Vector2 GetPointOnLine(Vector2 p1, Vector2 p2, float p) => (p1 - p2) * p + p2;

    public static float GetDistance(Vector2 p1, Vector2 p2) => Vector2.Distance(p1, p2);

    public static Vector2 GetLineIntersection(ILine l1, ILine l2)
    {
        // Line AB represented as a1x + b1y = c1
        float a1 = l1.End.y - l1.Start.y;
        float b1 = l1.Start.x - l1.End.x;
        float c1 = a1 * (l1.Start.x) + b1 * (l1.Start.y);

        // Line CD represented as a2x + b2y = c2
        float a2 = l2.End.y - l2.Start.y;
        float b2 = l2.Start.x - l2.End.x;
        float c2 = a2 * (l2.Start.x) + b2 * (l2.Start.y);

        float determinant = a1 * b2 - a2 * b1;

        if (determinant == 0)
        {
            // The lines are parallel. This is simplified
            // by returning a pair of FLT_MAX
            return new Vector2(float.MaxValue, float.MaxValue);
        }
        else
        {
            float x = (b2 * c1 - b1 * c2) / determinant;
            float y = (a1 * c2 - a2 * c1) / determinant;
            return new Vector2(x, y);
        }
    }
}