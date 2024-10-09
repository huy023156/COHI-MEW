using System;
using Unity.Mathematics;
using UnityEngine;

public class GridSystem<TGridObject>
{
    private int width;
    private int height;
    private Vector2 originPosition;
    private float cellSize;
    private TGridObject[,] gridObjectArray;

    public GridSystem(int width, int height, float cellSize, Vector2 originPosition,
        Func<GridSystem<TGridObject>, Vector2Int, TGridObject> createGridObject)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;   
        gridObjectArray = new TGridObject[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2Int gridPosition = new Vector2Int(x, y);
                gridObjectArray[x, y] = createGridObject(this, gridPosition);
            }
        }
    }

    public Vector2 GetWorldPosition(Vector2Int gridPosition)
    {
        return new Vector2(gridPosition.x, gridPosition.y) * cellSize + originPosition;
    }

    public Vector2Int GetGridPosition(Vector3 worldPosition)
    {
        return new Vector2Int(
            Mathf.FloorToInt((worldPosition.x - originPosition.x + cellSize * .5f)  / cellSize),
            Mathf.FloorToInt((worldPosition.y - originPosition.y + cellSize * .5f)  / cellSize)
        );
    }

    public void CreateDebugObjects(Transform debugPrefabs)
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2Int gridPosition = new Vector2Int(x, y);
                Transform gridDebugObjectTransform = GameObject.Instantiate(debugPrefabs, GetWorldPosition(gridPosition), quaternion.identity);
                GridDebugObject gridDebugObject = gridDebugObjectTransform.GetComponent<GridDebugObject>();

                gridDebugObject.SetGridObject((GetGridObject(new Vector2Int(x, y))));
            }
        }
    }

    public TGridObject GetGridObject(Vector2Int gridPosition)
    {
        return gridObjectArray[gridPosition.x, gridPosition.y];
    }

    public TGridObject GetGridObject(Vector3 position)
    {
        Vector2Int gridPosition = GetGridPosition(position);

        return gridObjectArray[gridPosition.x, gridPosition.y];
    }

    public bool IsValidGridPosition(Vector2Int gridPosition)
    {
        return gridPosition.x >= 0 &&
               gridPosition.y >= 0 &&
               gridPosition.x < width &&
               gridPosition.y < height;
    }

    public int GetWidth() => width;
    public int GetHeight() => height;
}
