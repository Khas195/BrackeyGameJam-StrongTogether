using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Tilemaps;
[ExecuteInEditMode]
public class Grid : MonoBehaviour
{
    [SerializeField]
    LayerMask unwalkableMask;
    [SerializeField]
    [ReadOnly]
    Vector2 gridWorldSize;
    [SerializeField]
    float nodeRadius;

    [SerializeField]
    [ReadOnly]
    float nodeDiameter;

    Node[,] grid = null;
    [SerializeField]
    int gridSizeX, gridSizeY;

    private void Start()
    {
        CalculateGridSize();
        CreateGrid();
    }

    [Button]
    private void CalculateGridSize()
    {
        nodeDiameter = nodeRadius * 2;
        gridWorldSize.x = gridSizeX * nodeDiameter;
        gridWorldSize.y = gridSizeY * nodeDiameter;
    }

    [Button]
    private void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics2D.OverlapBox(worldPoint, new Vector2(nodeRadius, nodeRadius), 0));
                grid[x, y] = new Node(walkable, worldPoint);
            }
        }
    }
    [SerializeField]
    void UpdateGridState()
    {
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics2D.OverlapCircle(worldPoint, nodeRadius));
                grid[x, y].walkable = walkable;
            }
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 0));
        if (grid != null)
        {
            foreach (var n in grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                Gizmos.DrawWireCube(n.worldPosition, Vector3.one * (nodeDiameter - 0.01f));
            }
        }


    }
    private void Update()
    {
        if (grid != null)
        {
            UpdateGridState();
        }
    }
}
