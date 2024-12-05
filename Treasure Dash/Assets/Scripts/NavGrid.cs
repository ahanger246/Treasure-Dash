using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavGrid : MonoBehaviour
{
    [SerializeField] private int gridSizeX, gridSizeY;
    [SerializeField] private Transform originPoint, player;
    [SerializeField] private LayerMask blockedMask;
    [SerializeField] private bool displayGrid;

    private float nodeRadius;// nodeDiameter;
    private Node[,] grid;
    
    void Awake() {
        // Determine the size of the nodes
        nodeRadius = 0.5f;
        //nodeDiameter = 1.0f;

        // Create the grid
        grid = new Node[gridSizeX, gridSizeY];
        // Use the location provided as the origin for the grid
        Vector3 gridOrigin = originPoint.position;
        // Fill the array with nodes and check 
        for(int x = 0; x < gridSizeX; x++) {
            for (int y = 0; y < gridSizeY; y++) { 
                // Calculate the position of the node using the origin
                Vector3 position = gridOrigin + Vector3.right * (x + nodeRadius) + Vector3.up * (y + nodeRadius);
                // Create a Vector2 of the node's position
                Vector2 boxPos = new Vector2(position.x, position.y);
                // Determine if the node's space is occupied by a wall, using the boxPos
                bool traversable = !(Physics2D.OverlapCircle(boxPos, nodeRadius, blockedMask));
                // Create a new node object with the position and traversable value
                grid[x,y] = new Node(position, traversable, x, y);
            }
        }
    }

    public int MaxSize() { 
        return gridSizeX * gridSizeY;
    }

    // This function returns the node corresponding to a specific position
    public Node FindNode(Vector3 gridPos) {
        float percentX = Mathf.Clamp01(gridPos.x / gridSizeX + 0.5f);
        float percentY = Mathf.Clamp01(gridPos.y / gridSizeY + 0.5f);

        int x = Mathf.RoundToInt((gridSizeX) * percentX);
        int y = Mathf.RoundToInt((gridSizeY) * percentY);

        if(x < 0) x = 0;
        if (x >= gridSizeX) x = gridSizeX - 1;
        if(y < 0) y = 0;
        if(y >= gridSizeY) y = gridSizeY - 1;

        return grid[x,y];
    }

    // This function returns a list of nodes that are direct neighbors to the given grid node
    public List<Node> FindNeighbors(Node node) {
        List<Node> neighbors = new List<Node>();

        // Find the nodes whose x and y values are 1 more or less than node's x and y values
        for (int xAdd = -1; xAdd <= 1; xAdd++) {
            for (int yAdd = -1; yAdd <= 1; yAdd++) {
                // If the x and y values to be added are 0, then this x,y value is node's own x,y value
                if (xAdd == 0 && yAdd == 0)
                    continue;
                //
                    // Create the x and y values for this neighbor node by adding xAdd and yAdd to node's x and y values
                    int x = node.getX() + xAdd;
                    int y = node.getY() + yAdd;

                // Now, check that the x and y values are within the grid
                if (x >= 0 && x < gridSizeX && y >= 0 && y < gridSizeY) {
                    if (x == node.getX() || y == node.getY()) {
                        neighbors.Add(grid[x, y]);
                    }
                }
            }
        }
        return neighbors;
    }
}
