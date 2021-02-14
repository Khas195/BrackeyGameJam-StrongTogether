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
    Transform TestPlayer = null;
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
                bool walkable = !(Physics2D.OverlapBox(worldPoint, new Vector2(nodeRadius, nodeRadius), 0, unwalkableMask));
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
                bool walkable = !(Physics2D.OverlapCircle(worldPoint, nodeRadius, unwalkableMask));
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
            Node playerNode = GetNodeFromWorldPoint(this.TestPlayer.position);
            foreach (var n in grid)
            {
                if (n == playerNode)
                {
                    Gizmos.color = Color.blue;
                }
                else
                {
                    Gizmos.color = (n.walkable) ? Color.white : Color.red;
                }
                Gizmos.DrawWireCube(n.worldPosition, Vector3.one * (nodeDiameter - 0.05f));
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
    private void Update()
    {
        if (grid != null)
        {
            UpdateGridState();
        }
    }
}
