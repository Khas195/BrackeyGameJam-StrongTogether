using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding2D : SingletonMonobehavior<PathFinding2D>
{

    public List<Node> FindPath(Vector3 startPos, Vector3 targetPos)
    {
        List<Node> path = new List<Node>();
        Node startNode = Grid.GetInstance().GetNodeFromWorldPoint(startPos);
        Node targetNode = Grid.GetInstance().GetNodeFromWorldPoint(targetPos);
        List<Node> openSet = new List<Node>();
        HashSet<Node> closeSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost)
                {
                    if (openSet[i].hCost < currentNode.hCost)
                    {

                        currentNode = openSet[i];
                    }
                }
            }
            openSet.Remove(currentNode);
            closeSet.Add(currentNode);
            if (currentNode == targetNode)
            {
                return RetracePath(startNode, targetNode);
            }
            List<Node> neighbourNodes = Grid.GetInstance().GetNeighbourNodes(currentNode);
            foreach (var neighbour in neighbourNodes)
            {
                if (neighbour.walkable == false || closeSet.Contains(neighbour))
                {
                    continue;
                }

                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);

                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                    {
                        openSet.Add(neighbour);
                    }
                }
            }

        }
        return path;
    }
    List<Node> RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;
        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        return path;
    }
    int GetDistance(Node nodeX, Node nodeB)
    {
        int distance = 0;
        int dstX = Mathf.Abs(nodeX.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeX.gridY - nodeB.gridY);

        if (dstX > dstY)
        {
            distance = 14 * dstY + 10 * (dstX - dstY);
        }
        else
        {
            distance = 14 * dstX + 10 * (dstY - dstX);
        }
        return distance;
    }

}
