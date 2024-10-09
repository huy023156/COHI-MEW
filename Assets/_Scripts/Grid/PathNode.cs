using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    private int gCost;
    private int hCost;
    private int fCost;

    private GridSystem<PathNode> gridSystem;
    private Vector2Int gridPosition;

    private bool isWalkable;

    private PathNode moveFormPathNode;

    public PathNode(GridSystem<PathNode> gridSystem, Vector2Int gridPosition)
    {
        this.gridSystem = gridSystem;
        this.gridPosition = gridPosition;
        isWalkable = true;
    }

    public int GetGCost()
    {
        return gCost;
    }

    public int GetHCost()
    {
        return hCost;
    }

    public int GetFCost()
    {
        return fCost;
    }

    public void SetGCost(int gCost)
    {
        this.gCost = gCost;
    }

    public void SetHCost(int hCost)
    {
        this.hCost = hCost;
    }

    public void CaculateFCost()
    {
        fCost = gCost + hCost;
    }

    public PathNode GetMoveFromPathNode()
    {
        return moveFormPathNode;
    }

    public void SetMoveFromPathNode(PathNode moveFromPathNode)
    {
        this.moveFormPathNode = moveFromPathNode;
    }

    public void SetWalkable(bool isWalkable)
    {
        this.isWalkable = isWalkable;
    }

    public bool IsWalkable()
    {
        return isWalkable;
    }

    public Vector2Int GetGridPosition()
    {
        return gridPosition;
    }

    public override string ToString()
    {
        return $"{gridPosition.ToString()}";
    }
}
