using UnityEngine;

public class AICarNetwork : MonoBehaviour
{
    public RoadSection currentSegment;
    public float speed = 8f;
    private bool forward = true;

    [Header("Headlights")]
    public Light leftHeadlight;
    public Light rightHeadlight;

    void Update()
    {
        if (currentSegment == null) return;

        MoveAlongSegment();
        UpdateHeadlights();
    }

    void MoveAlongSegment()
    {
        Vector3 dir = (forward
            ? currentSegment.endNode.transform.position - currentSegment.startNode.transform.position
            : currentSegment.startNode.transform.position - currentSegment.endNode.transform.position).normalized;

        Vector3 offset = Vector3.Cross(Vector3.up, dir) * currentSegment.laneOffset;
        Vector3 target = (forward ? currentSegment.endNode.transform.position : currentSegment.startNode.transform.position) + offset;

        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 5f);

        if (Vector3.Distance(transform.position, target) < 0.5f)
            PickNextSegment();
    }

    void PickNextSegment()
    {
        RoadNode node = forward ? currentSegment.endNode : currentSegment.startNode;

        if (node.outgoingSegments.Count > 0)
        {
            currentSegment = node.outgoingSegments[Random.Range(0, node.outgoingSegments.Count)];
            forward = true;
        }
        else if (currentSegment.allowUTurn)
        {
            forward = !forward; // U-turn
        }
        else
        {
            Destroy(gameObject, 0.1f); // dead-end
        }
    }

    void UpdateHeadlights()
    {
        if (leftHeadlight == null || rightHeadlight == null) return;

        // Get the DayNightCycle instance (assuming one exists in scene)
        DayNightCycle cycle = FindObjectOfType<DayNightCycle>();
        if (cycle != null)
        {
            bool nightTime = cycle.IsNightTime(); // Implement IsNightTime() in DayNightCycle
            leftHeadlight.enabled = nightTime;
            rightHeadlight.enabled = nightTime;
        }
    }
}
