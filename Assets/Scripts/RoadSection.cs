using System.Collections.Generic;
using UnityEngine;

public class RoadSection : MonoBehaviour
{
    public RoadNode startNode;  // assign in Inspector
    public RoadNode endNode;    // assign in Inspector
    public float laneOffset = 1.5f;
    public float speed = 10f;
    public bool allowUTurn = false;
}