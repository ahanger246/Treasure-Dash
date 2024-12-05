using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyMove : MonoBehaviour {

    private NavGrid grid;
    PathRequestManager requestManager;

    void Awake() {
        grid = GetComponent<NavGrid>();
        requestManager = GetComponent<PathRequestManager>();
    }

    public void StartFindingPath(Vector3 startPos, Vector3 targetPos) { 
        StartCoroutine(FindPath(startPos, targetPos));
    }

    IEnumerator FindPath(Vector3 startPos, Vector3 targetPos) {

        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        Node startNode = grid.FindNode(startPos);
        Node targetNode = grid.FindNode(targetPos);

        // Create the open and closed sets
        List<Node>openSet = new List<Node>();
        openSet.Add(startNode);
        HashSet<Node> closedSet = new HashSet<Node>();

        while (openSet.Count > 0) {
            // Create currentNode and set it to be the first node in the openSet
           Node currentNode = openSet[0];
            // Loop through the rest of the openSet and try to find a node with a lower F-cost
            for (int i = 1; i < openSet.Count; i++) {
                if (openSet[i].getFCost()<currentNode.getFCost() || openSet[i].getFCost() == currentNode.getFCost() && openSet[i].getHCost() < currentNode.getHCost()) { 
                    currentNode = openSet[i];
                }
            }

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode) {
                pathSuccess = true;
                break;
            }

            foreach (Node neighbor in grid.FindNeighbors(currentNode)) {
                if (!neighbor.isWalkable() || closedSet.Contains(neighbor)) {
                    continue;
                }

                int newMoveCost = currentNode.getGCost() + FindDistance(currentNode, neighbor);
                if (newMoveCost < neighbor.getGCost() || !openSet.Contains(neighbor)) { 
                    neighbor.setGCost(newMoveCost);
                    neighbor.setHCost(FindDistance(neighbor, targetNode));
                    neighbor.setParent(currentNode);
                    // Now, check if the neighbor is in the openSet
                    if (!openSet.Contains(neighbor)) { 
                        openSet.Add(neighbor);
                    }
                }
            }
        }
        yield return null;
        if (pathSuccess) {
            waypoints = RetracePath(startNode, targetNode);
        }
        requestManager.FinishedPath(waypoints, pathSuccess);
    }

    Vector3[] RetracePath(Node startNode, Node endNode) {
        List<Vector3> path = new List<Vector3>();
        Node currentNode = endNode;

        while (currentNode != startNode) {
            path.Add(currentNode.getPos());
            currentNode = currentNode.getParent();
        }
        
        Vector3[] waypoints = path.ToArray();
        Array.Reverse(waypoints);
        return waypoints;
    }

    int FindDistance(Node nodeA, Node nodeB) {
        int xDistance = Mathf.Abs(nodeA.getX() - nodeB.getX());
        int yDistance = Mathf.Abs(nodeA.getY() - nodeB.getY());

        if (xDistance > yDistance) { 
            return 14 * yDistance + 10 * (xDistance - yDistance);
        } else {
            return 14 * xDistance + 10 * (yDistance - xDistance);
        }
    }
}
