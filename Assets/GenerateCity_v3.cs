using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ph = PlannerHelper;

public class GenerateCity_v3 : MonoBehaviour
{
    public int size = 3;
    int cellSize = 10;
    [Range(0, .49f)] float cellOffset = .1f;

    public List<Vector2> centers;
    public List<Vector2> points;
    public List<EndLine> lines;
    float pointsDistanceFreshhold => cellSize / 5f;

    [Space]
    public float generateEvery = 3f;
    [Space]
    public float sizeP = .1f;
    public float sizeC = .05f;
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

        points.AddRange(centers);
        for (int x = 1; x < size-1; x++)
        {
            for (int y = 1; y < size-1; y++)
            {
                Vector2[] cellMidpoints = new Vector2[8];
                int i = 0;
                cellMidpoints[i++] = ph.GetMidpoint(cellCenters[x, y], cellCenters[x + 0, y + 1]);
                cellMidpoints[i++] = ph.GetMidpoint(cellCenters[x, y], cellCenters[x + 1, y + 1]);
                cellMidpoints[i++] = ph.GetMidpoint(cellCenters[x, y], cellCenters[x + 1, y + 0]);
                cellMidpoints[i++] = ph.GetMidpoint(cellCenters[x, y], cellCenters[x + 1, y - 1]);
                cellMidpoints[i++] = ph.GetMidpoint(cellCenters[x, y], cellCenters[x + 0, y - 1]);
                cellMidpoints[i++] = ph.GetMidpoint(cellCenters[x, y], cellCenters[x - 1, y - 1]);
                cellMidpoints[i++] = ph.GetMidpoint(cellCenters[x, y], cellCenters[x - 1, y + 0]);
                cellMidpoints[i++] = ph.GetMidpoint(cellCenters[x, y], cellCenters[x - 1, y + 1]);

                ILine[] Normals = new ILine[8];
                for (i = 0; i < 8; i++)
                {
                    Normals[i] = ph.GetNormalLine(new EndLine { start = cellMidpoints[i], end = cellCenters[x, y] });
                }

                Vector2[] edgePoints = new Vector2[8 * 2];
                for (i = 1; i < 8 + 1; i++)
                {
                    int edgePointsId = (i * 2) % 16;
                    Vector2 neighbourCrossPoint = ph.GetLineIntersection(Normals[(i - 1) % 8], Normals[(i + 1) % 8]);
                    if (ph.GetDistance(cellCenters[x, y], neighbourCrossPoint) >
                        ph.GetDistance(cellCenters[x, y], cellMidpoints[i % 8]))
                    {
                        edgePoints[edgePointsId + 0] = ph.GetLineIntersection(Normals[i % 8], Normals[(i - 1) % 8]);
                        edgePoints[edgePointsId + 1] = ph.GetLineIntersection(Normals[i % 8], Normals[(i + 1) % 8]);
                    }
                    else
                    {
                        edgePoints[edgePointsId + 0] = neighbourCrossPoint;
                        edgePoints[edgePointsId + 1] = neighbourCrossPoint;
                    }
                }

                points.AddRange(edgePoints);
                for (i = 1; i < 8 + 1; i++)
                {
                    int edgePointsId = i * 2;
                    lines.Add(new EndLine { start = edgePoints[(edgePointsId - 1) % 16], end = edgePoints[(edgePointsId + 0) % 16] });
                    lines.Add(new EndLine { start = edgePoints[(edgePointsId + 0) % 16], end = edgePoints[(edgePointsId + 1) % 16] });
                    lines.Add(new EndLine { start = edgePoints[(edgePointsId + 1) % 16], end = edgePoints[(edgePointsId + 2) % 16] });
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
