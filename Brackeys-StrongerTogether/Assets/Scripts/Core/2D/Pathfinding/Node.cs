﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Node
{
    public bool walkable;
    public Vector3 worldPosition;
    public int gridX = 0;
    public int gridY = 0;
    public int gCost;
    public int hCost;
    public Node parent;
    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }

    public Node(bool walkable, Vector3 worldPosition, int gridX, int gridY)
    {
        this.walkable = walkable;
        this.worldPosition = worldPosition;
        this.gridX = gridX;
        this.gridY = gridY;
    }

    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return base.ToString();
    }

    public static bool operator ==(Node lhs, Node rhs)
    {
        return lhs.worldPosition == rhs.worldPosition && lhs.walkable == rhs.walkable;
    }
    public static bool operator !=(Node lhs, Node rhs)
    {
        return !(lhs == rhs);
    }
}
