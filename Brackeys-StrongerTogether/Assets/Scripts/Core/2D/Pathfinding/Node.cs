using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool walkable;
    public Vector3 worldPosition;

    public Node(bool walkable, Vector3 worldPosition)
    {
        this.walkable = walkable;
        this.worldPosition = worldPosition;
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
