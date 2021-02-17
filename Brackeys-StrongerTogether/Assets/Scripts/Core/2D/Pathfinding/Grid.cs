﻿using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Tilemaps;
[ExecuteInEditMode]
public class Grid : SingletonMonobehavior<Grid>
{
    [SerializeField]
    bool showGizmos = false;
    [SerializeField]
    LayerMask walkableMask;

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

    private void Awake()
    {
        CreateGrid();
    }

    private void CalculateGridSize()
    {
        nodeDiameter = nodeRadius * 2;
        gridWorldSize.x = gridSizeX * nodeDiameter;
        gridWorldSize.y = gridSizeY * nodeDiameter;
    }

    [Button]
    private void CreateGrid()
    {

        CalculateGridSize();
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);
                bool walkable = (Physics2D.OverlapBox(worldPoint, new Vector2(nodeRadius, nodeRadius), 0, walkableMask));
                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    public float GetNodeDiameter()
    {
        return nodeDiameter;
    }

    public float GetTileOffset()
    {
        return gridTileOffset;
    }

    public List<Node> GetNeighbourNodes(Node node)
    {
        List<Node> neighbours = new List<Node>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;
                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }
        return neighbours;
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
                bool walkable = (Physics2D.OverlapCircle(worldPoint, nodeRadius, walkableMask));
                grid[x, y].walkable = walkable;
            }
        }

    }

    [SerializeField]
    float gridTileOffset;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y, 0));


        if (grid != null && showGizmos == true)
        {
            Gizmos.color = Color.white;
            foreach (var n in grid)
            {
                if (n.walkable)
                {
                    Gizmos.DrawWireCube(n.worldPosition, Vector3.one * (nodeDiameter - gridTileOffset));
                }
            }
        }
    }
    public Node GetNodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x, y];

    }
    public int MaxSize
    {
        get
        {
            return gridSizeX * gridSizeY;
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
