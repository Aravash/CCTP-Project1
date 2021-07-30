using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node>
{
    public enum State
    {
        UNKNOWN,
        UNWALKABLE,
        KNOWN,
        INFORMED
    }

    public State state = State.UNKNOWN;
    public bool walkable;
    public bool known;
    public Vector3 worldPos;
    public int gridX, gridY;
    public int movePenalty;
    
    public int gCost;
    public int hCost;
    public Node parent;
    private int heapIndex;

    public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY, int _penalty)
    {
        if (!_walkable) state = State.UNWALKABLE;
        worldPos = _worldPos;
        gridX = _gridX;
        gridY = _gridY;
        movePenalty = _penalty;
    }

    public int fCost
    {
        get { return gCost + hCost; }
    }

    public int HeapIndex
    {
        get { return heapIndex;  }
        set { heapIndex = value; }
    }

    public int CompareTo(Node nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }

        return -compare;
    }
}
