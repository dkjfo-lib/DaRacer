using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ph = PlannerHelper;

public class GenerateCity_v4 : MonoBehaviour
{
    public int size = 3;
    public int cellSize = 10;
    [Range(0, .49f)] float cellOffset = .1f;

    public List<Vector2> centers;
    public List<Vector2> points;
    public List<EndLine> streets;
    public List<EndLine> paths;
    public List<CrossPoint> crossPoints;

    [Space]
    public float generateEvery = 3f;
    [Space]
    public float sizeP = .1f;
    public float sizeC = .05f;
    public bool wire = false;
    [Space]
    public bool hasUpdate = false;

    private void Start()
    {
        StartCoroutine(GenerateOnTimer());
    }

    IEnumerator GenerateOnTimer()
    {
        while (true)
        {
            Generate();
            hasUpdate = true;
            yield return new WaitForSeconds(generateEvery);
        }
    }

    void Generate()
    {
        centers = new List<Vector2>();
        points = new List<Vector2>();
        streets = new List<EndLine>();
        paths = new List<EndLine>();
        crossPoints = new List<CrossPoint>();

        Vector2[,] cellCenters = new Vector2[size, size];
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                cellCenters[x, y] = new Vector2(
                    Random.Range(cellOffset, 1 - cellOffset) + x,
                    Random.Range(cellOffset, 1 - cellOffset) + y);
                centers.Add(cellCenters[x, y]);
                crossPoints.Add(new CrossPoint { point = cellCenters[x, y] });
            }
        }

        points.AddRange(centers);
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                if (y < size - 1)
                {
                    crossPoints[x * size + y].connectedRoads.Add(cellCenters[x + 0, y + 1]);
                    streets.Add(new EndLine { start = cellCenters[x, y], end = cellCenters[x + 0, y + 1] });
                }
                if (x < size - 1)
                {
                    crossPoints[x * size + y].connectedRoads.Add(cellCenters[x + 1, y + 0]);
                    streets.Add(new EndLine { start = cellCenters[x, y], end = cellCenters[x + 1, y + 0] });
                }
                if (y > 0)
                {
                    crossPoints[x * size + y].connectedRoads.Add(cellCenters[x + 0, y - 1]);
                    //streets.Add(new EndLine { start = cellCenters[x, y], end = cellCenters[x + 0, y - 1] });
                }
                if (x > 0)
                {
                    crossPoints[x * size + y].connectedRoads.Add(cellCenters[x - 1, y + 0]);
                    //streets.Add(new EndLine { start = cellCenters[x, y], end = cellCenters[x - 1, y + 0] });
                }
            }
        }
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                if (x < size - 1 && y < size - 1)
                {
                    Vector2[] neighbours = new Vector2[]
                    {
                        cellCenters[x + 0, y + 0],
                        cellCenters[x + 0, y + 1],
                        cellCenters[x + 1, y + 1],
                        cellCenters[x + 1, y + 0],
                    };
                    EndLine left = new EndLine { start = neighbours[0], end = neighbours[1] };
                    EndLine up = new EndLine { start = neighbours[1], end = neighbours[2] };
                    EndLine right = new EndLine { start = neighbours[2], end = neighbours[3] };
                    EndLine down = new EndLine { start = neighbours[3], end = neighbours[0] };

                    /// 0 - no
                    /// 1 - hor
                    /// 2 - vert
                    /// 3 - T hor up
                    /// 4 - T hor down
                    /// 5 - T vert right
                    /// 6 - T vert left
                    /// 7 - X 
                    var type = Random.Range(0, 7);

                    var p1 = ph.GetPointOnLine(left, Random.Range(.2f, .8f));
                    var p2 = ph.GetPointOnLine(right, Random.Range(.2f, .8f));
                    var p3 = ph.GetPointOnLine(up, Random.Range(.2f, .8f));
                    var p4 = ph.GetPointOnLine(down, Random.Range(.2f, .8f));

                    var hor = new EndLine { start = p1, end = p2 };
                    var vert = new EndLine { start = p3, end = p4 };
                    var p5 = ph.GetLineIntersection(hor, vert);

                    var LC = new EndLine { start = p1, end = p5 };
                    var RC = new EndLine { start = p2, end = p5 };
                    var UC = new EndLine { start = p3, end = p5 };
                    var DC = new EndLine { start = p4, end = p5 };

                    if (type == 1 || type == 3 || type == 4 || type == 6 || type == 7)
                        paths.Add(LC);
                    if (type == 1 || type == 3 || type == 4 || type == 5 || type == 7)
                        paths.Add(RC);
                    if (type == 2 || type == 5 || type == 6 || type == 3 || type == 7)
                        paths.Add(UC);
                    if (type == 2 || type == 5 || type == 6 || type == 4 || type == 7)
                        paths.Add(DC);
                }
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
                    Gizmos.DrawWireSphere(new Vector3(p.x, 0, p.y) * cellSize, sizeP);
                else
                    Gizmos.DrawSphere(new Vector3(p.x, 0, p.y) * cellSize, sizeP);
            }
        }
        if (centers != null)
        {
            Gizmos.color = Color.green;
            foreach (var p in centers)
            {
                if (wire)
                    Gizmos.DrawWireSphere(new Vector3(p.x, 0, p.y) * cellSize, sizeC);
                else
                    Gizmos.DrawSphere(new Vector3(p.x, 0, p.y) * cellSize, sizeC);
            }
        }
        if (streets != null)
        {
            Gizmos.color = Color.green;
            foreach (var l in streets)
            {
                Gizmos.DrawLine(new Vector3(l.Start.x, 0, l.Start.y) * cellSize, new Vector3(l.End.x, 0, l.End.y) * cellSize);
            }
        }
        if (paths != null)
        {
            Gizmos.color = Color.cyan;
            foreach (var l in paths)
            {
                Gizmos.DrawLine(new Vector3(l.Start.x, 0, l.Start.y) * cellSize, new Vector3(l.End.x, 0, l.End.y) * cellSize);
            }
        }
    }
}

[System.Serializable]
public class CrossPoint
{
    public Vector2 point;
    public List<Vector2> connectedRoads = new List<Vector2>();
}

[System.Serializable]
public class AdjacentRoad
{
    public Vector2 connectedPoints;
    public StreetType streetTypes;
}

public enum StreetType
{
    street,
    path
}