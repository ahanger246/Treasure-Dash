using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {
    
    private Vector3 position;
    private bool walkable;
    private int x, y, gCost, hCost, heapIndex;
    private Node parent;

    public Node(Vector3 pos, bool walk, int x, int y)
    {
        this.position = pos;
        this.position.z = 0;
        this.walkable = walk;
        this.x = x;
        this.y = y;
        // Initialize the g and h costs
        gCost = 0;
        hCost = 0;
    }
    // Retrieves the position of the node
    public Vector3 getPos()
    {
        return position;
    }
    // Returns whether the node is walkable
    public bool isWalkable()
    {
        return walkable;
    }
    // Returns the x position
    public int getX()
    {
        return x;
    }
    // Returns the y position
    public int getY()
    {
        return y;
    }
    // Allows the gCost to be changed
    public void setGCost(int gCost)
    {
        this.gCost = gCost;
    }
    // Retrieves gCost's value
    public int getGCost()
    {
        return gCost;
    }
    // Allows the hCost to be changed
    public void setHCost(int hCost)
    {
        this.hCost = hCost;
    }
    // Retrieve's hCost's value
    public int getHCost()
    {
        return hCost;
    }
    // Retrieves the fCost for pathfinding needs
    public int getFCost()
    {
        return gCost + hCost;
    }
    // Set a parent node
    public void setParent(Node parent)
    {
        this.parent = parent;
    }
    // Get the parent node
    public Node getParent()
    {
        return parent;
    }
}