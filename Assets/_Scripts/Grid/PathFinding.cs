using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PathFinding : Singleton<PathFinding>
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    [SerializeField] private Transform pathFindingGridDebugObjectTransform;
    [SerializeField] private LayerMask obstaclesLayerMask;

    [SerializeField] private bool showDebugObject;

    private GridSystem<PathNode> gridSystem;
    private int width;
    private int height;
    private float cellSize;
    private Vector2 origin;

    protected override void Awake()
    {
        base.Awake();
        SetUp(65, 35, 0.3f, new Vector2(-9.5f, -6.5f));
        //SetUp(36, 30, 1f, new Vector2(-7.5f, -6.5f));
    }

    public void SetUp(int width, int height, float cellSize, Vector2 origin)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.origin = origin;

        gridSystem = new GridSystem<PathNode>(width, height, cellSize, origin,
            (GridSystem<PathNode> g, Vector2Int gridPosition) => new PathNode(g, gridPosition));

        if (showDebugObject)
        {
            gridSystem.CreateDebugObjects(pathFindingGridDebugObjectTransform);
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                PathNode pathnode = gridSystem.GetGridObject(new Vector2Int(x, y));
                Vector3 originPosition = gridSystem.GetWorldPosition(pathnode.GetGridPosition());

                float rayCastOffsetAmount = 2f;

                bool rayCastHit = Physics.Raycast(
                    originPosition + new Vector3(0, 0, 1) * rayCastOffsetAmount,
                    new Vector3(0, 0, -1),
                    rayCastOffsetAmount * 2,
                    obstaclesLayerMask);

                if (rayCastHit)
                {
                    pathnode.SetWalkable(false);
                }
            }
        }
    }

    public List<Vector2Int> FindPath(Vector2Int startPosition, Vector2Int endPosition, out int pathLength)
    {
        PathNode startNode = gridSystem.GetGridObject(startPosition);
        PathNode endNode = gridSystem.GetGridObject(endPosition);
        List<PathNode> openList = new List<PathNode>();
        List<PathNode> closedList = new List<PathNode>();

        for (int x = 0; x < gridSystem.GetWidth(); x++)
        {
            for (int z = 0; z < gridSystem.GetHeight(); z++)
            {
                PathNode pathNode = gridSystem.GetGridObject(new Vector2Int(x, z));
                pathNode.SetGCost(int.MaxValue);
                pathNode.SetHCost(0);
                pathNode.CaculateFCost();
                pathNode.SetMoveFromPathNode(null);
            }
        }

        startNode.SetGCost(0);
        startNode.SetHCost(CaculateDistance(startPosition, endPosition));
        startNode.CaculateFCost();

        openList.Add(startNode);

        while (openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostPathNode(openList);

            if (currentNode == endNode)
            {
                pathLength = endNode.GetGCost();
                return CalculatePath(currentNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (PathNode neighbor in GetNeighborPathNodeList(currentNode))
            {
                if (closedList.Contains(neighbor))
                {
                    continue;
                }

                if (!neighbor.IsWalkable())
                {
                    closedList.Add(neighbor);
                    continue;
                }

                int tentativeGCost = currentNode.GetGCost() +
                    CaculateDistance(currentNode.GetGridPosition(), neighbor.GetGridPosition());
                if (neighbor.GetGCost() > tentativeGCost)
                {
                    neighbor.SetGCost(tentativeGCost);
                    neighbor.SetHCost(CaculateDistance(neighbor.GetGridPosition(), endNode.GetGridPosition()));
                    neighbor.CaculateFCost();
                    neighbor.SetMoveFromPathNode(currentNode);

                    if (!openList.Contains(neighbor))
                    {
                        openList.Add(neighbor);
                    }
                }
            }
        }

        pathLength = 0;
        return null;
    }

    private List<PathNode> GetNeighborPathNodeList(PathNode pathNode)
    {
        List<PathNode> neighborList = new List<PathNode>();

        int x = pathNode.GetGridPosition().x;
        int z = pathNode.GetGridPosition().y;

        if (x - 1 >= 0)
        {
            // Left
            neighborList.Add(gridSystem.GetGridObject(new Vector2Int(x - 1, z + 0)));
            if (z + 1 < gridSystem.GetHeight())
            {
                // Left Up
                neighborList.Add(gridSystem.GetGridObject(new Vector2Int(x - 1, z + 1)));
            }

            if (z - 1 >= 0)
            {
                // Left Down
                neighborList.Add(gridSystem.GetGridObject(new Vector2Int(x - 1, z - 1)));
            }
        }

        if (x + 1 < gridSystem.GetWidth())
        {
            // Right
            neighborList.Add(gridSystem.GetGridObject(new Vector2Int(x + 1, z + 0)));
            if (z + 1 < gridSystem.GetHeight())
            {
                // Right Up
                neighborList.Add(gridSystem.GetGridObject(new Vector2Int(x + 1, z + 1)));
            }

            if (z - 1 >= 0)
            {
                // Right Down
                neighborList.Add(gridSystem.GetGridObject(new Vector2Int(x + 1, z - 1)));
            }
        }

        if (z + 1 < gridSystem.GetHeight())
        {
            // Up
            neighborList.Add(gridSystem.GetGridObject(new Vector2Int(x + 0, z + 1)));
        }

        if (z - 1 >= 0)
        {
            // Down
            neighborList.Add(gridSystem.GetGridObject(new Vector2Int(x + 0, z - 1)));
        }

        return neighborList;
    }

    private List<Vector2Int> CalculatePath(PathNode endNode)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        path.Add(endNode.GetGridPosition());

        while (endNode.GetMoveFromPathNode() != null)
        {
            path.Add(endNode.GetMoveFromPathNode().GetGridPosition());
            endNode = endNode.GetMoveFromPathNode();
        }

        path.Reverse();
        return path;
    }

    private int CaculateDistance(Vector2Int a, Vector2Int b)
    {
        int xDistance = math.abs(a.x - b.x);
        int zDistance = math.abs(a.y - b.y);
        int remainder = math.abs(xDistance - zDistance);

        return math.min(xDistance, zDistance) * MOVE_DIAGONAL_COST + remainder * MOVE_STRAIGHT_COST;
    }

    private PathNode GetLowestFCostPathNode(List<PathNode> pathNodeList)
    {
        PathNode lowestFCostPathNode = pathNodeList[0];

        for (int i = 1; i < pathNodeList.Count; i++)
        {
            if (pathNodeList[i].GetFCost() < lowestFCostPathNode.GetFCost())
            {
                lowestFCostPathNode = pathNodeList[i];
            }
        }

        return lowestFCostPathNode;
    }

    public LayerMask GetObstaclesLayerMask()
    {
        return obstaclesLayerMask;
    }

    public bool IsGridPositionWalkable(Vector2Int gridPosition)
    {
        return gridSystem.GetGridObject(gridPosition).IsWalkable();
    }

    public bool HasPath(Vector2Int startGridPosition, Vector2Int endGridPosition)
    {
        return FindPath(startGridPosition, endGridPosition, out int pathLength) != null;
    }

    public int GetPathLength(Vector2Int startGridPosition, Vector2Int endGridPosition)
    {
        FindPath(startGridPosition, endGridPosition, out int pathLength);
        return pathLength;
    }

    public void SetWalkableAtGridPosition(Vector2Int gridPosition, bool isWalkable)
    {

        gridSystem.GetGridObject(gridPosition).SetWalkable(isWalkable);
    }

    public void SetWalkableAtWorldPosition(Vector3 worldPosition, bool isWalkable)
    {
        Vector2Int gridPosition = gridSystem.GetGridPosition(worldPosition);
        SetWalkableAtGridPosition(gridPosition, isWalkable);
    }

    public GridSystem<PathNode> GetGridSystem() => gridSystem;
}
