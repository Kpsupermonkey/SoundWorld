
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class RoadVisualizer : MonoBehaviour
{
    public RoadNode[] nodes;
    public Color nodeColor = Color.yellow;
    public Color segmentColor = Color.cyan;
    public Color laneColor = Color.green;
    public float nodeSize = 0.5f;
    public float arrowSize = 0.5f;

    void OnDrawGizmos()
    {
        if (nodes == null) return;

        foreach (var node in nodes)
        {
            if (node == null) continue;

            // Draw the node itself
            Gizmos.color = nodeColor;
            Gizmos.DrawSphere(node.transform.position, nodeSize);

            // Draw outgoing segments
            if (node.outgoingSegments != null)
            {
                foreach (var seg in node.outgoingSegments)
                {
                    if (seg == null || seg.endNode == null) continue;

                    Vector3 start = node.transform.position;
                    Vector3 end = seg.endNode.transform.position;

                    // Draw main segment line
                    Gizmos.color = segmentColor;
                    Gizmos.DrawLine(start, end);

                    // Draw lane offset
                    Vector3 dir = (end - start).normalized;
                    Vector3 laneOffsetVec = Vector3.Cross(Vector3.up, dir) * seg.laneOffset;
                    Gizmos.color = laneColor;
                    Gizmos.DrawLine(start + laneOffsetVec, end + laneOffsetVec);

                    // Draw arrowhead at end
                    Vector3 arrowDir = (end - start).normalized * arrowSize;
                    Vector3 right = Vector3.Cross(Vector3.up, arrowDir) * 0.3f;
                    Gizmos.DrawLine(end, end - arrowDir + right);
                    Gizmos.DrawLine(end, end - arrowDir - right);

                    // Highlight issues
                    if (seg.laneOffset == 0f)
                    {
                        Gizmos.color = Color.red;
                        Gizmos.DrawSphere((start + end) / 2, 0.2f);
                    }

                    if (seg.allowUTurn && (node.outgoingSegments.Count == 0))
                    {
                        Gizmos.color = Color.magenta;
                        Gizmos.DrawSphere(end, 0.3f); // U-turn dead-end highlight
                    }
                }
            }
        }
    }
}

public class RoadManager : MonoBehaviour
{
    [Header("Road Network")]
    public RoadNode[] nodes;
    public RoadSection[] segments;

    [Header("Car Spawning")]
    public List<GameObject> carPrefabs;
    public float spawnInterval = 3f;
    public int maxCars = 20;

    private float spawnTimer = 0f;
    private int currentCars = 0;

    void Update()
    {
        // Spawn cars periodically
        if (carPrefabs.Count > 0 && currentCars < maxCars)
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= spawnInterval)
            {
                SpawnCar();
                spawnTimer = 0f;
            }
        }
    }

    void SpawnCar()
    {
        if (segments.Length == 0) return;

        // Pick a random starting segment
        RoadSection startSegment = segments[Random.Range(0, segments.Length)];

        // Instantiate car at start of segment (left lane as example)
        Vector3 dir = (startSegment.endNode.transform.position - startSegment.startNode.transform.position).normalized;
        Vector3 laneOffset = Vector3.Cross(Vector3.up, dir) * startSegment.laneOffset;
        Vector3 spawnPos = startSegment.startNode.transform.position + laneOffset;

        GameObject car = Instantiate(carPrefabs[Random.Range(0,carPrefabs.Count-1)], spawnPos, Quaternion.LookRotation(dir));
        AICarNetwork ai = car.GetComponent<AICarNetwork>();
        if (ai != null)
        {
            ai.currentSegment = startSegment;
        }

        currentCars++;
        // Optional: reduce car count when destroyed
        car.AddComponent<CarDestroyNotifier>().manager = this;
    }

    public void CarDestroyed()
    {
        currentCars = Mathf.Max(0, currentCars - 1);
    }

    // Optional: visualization in Scene view
    void OnDrawGizmos()
    {
        if (nodes == null) return;

        Gizmos.color = Color.yellow;
        foreach (var node in nodes)
        {
            if (node == null) continue;
            Gizmos.DrawSphere(node.transform.position, 0.5f);

            if (node.outgoingSegments != null)
            {
                foreach (var seg in node.outgoingSegments)
                {
                    if (seg == null || seg.endNode == null) continue;
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawLine(node.transform.position, seg.endNode.transform.position);

                    // lane offset line
                    Vector3 dir = (seg.endNode.transform.position - node.transform.position).normalized;
                    Vector3 offset = Vector3.Cross(Vector3.up, dir) * seg.laneOffset;
                    Gizmos.DrawLine(node.transform.position + offset, seg.endNode.transform.position + offset);

                    // arrowhead
                    Vector3 arrowDir = (seg.endNode.transform.position - node.transform.position).normalized * 0.5f;
                    Vector3 right = Vector3.Cross(Vector3.up, arrowDir) * 0.2f;
                    Gizmos.DrawLine(seg.endNode.transform.position, seg.endNode.transform.position - arrowDir + right);
                    Gizmos.DrawLine(seg.endNode.transform.position, seg.endNode.transform.position - arrowDir - right);
                }
            }
        }
    }
}

/// <summary>
/// Notifies the manager when a car is destroyed to reduce the counter
/// </summary>
public class CarDestroyNotifier : MonoBehaviour
{
    public RoadManager manager;

    void OnDestroy()
    {
        if (manager != null)
            manager.CarDestroyed();
    }
}
