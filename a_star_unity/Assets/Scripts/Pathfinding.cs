using System.Collections;
using System.Collections.Generic; //using a list
using UnityEngine;

public class Pathfinding : MonoBehaviour { //must extend monobehavior because we are using some of the funcitonality

	public Transform seeker, target; //point A and point B
	Grid grid; 

	void Awake() {
		grid = GetComponent<Grid> ();
	}

	void Update() {
		FindPath (seeker.position, target.position);
	}

	void FindPath(Vector3 startPos, Vector3 targetPos) {
		Node startNode = grid.NodeFromWorldPoint(startPos); //convert world positions into nodes (from Grid.cs)
		Node targetNode = grid.NodeFromWorldPoint(targetPos);

		List<Node> openSet = new List<Node>(); //creating a list of nodes for the open set-- the set that has yet to be evaluated
		HashSet<Node> closedSet = new HashSet<Node>(); 
		openSet.Add(startNode); //add the starting node to the open set

		while (openSet.Count > 0) {
			Node node = openSet[0]; 
			for (int i = 1; i < openSet.Count; i ++) { //loop through all the open nodes to find the node with the lowest f-cost
				if (openSet[i].fCost < node.fCost || openSet[i].fCost == node.fCost) {
					if (openSet[i].hCost < node.hCost) //evaluate the hCosts of the newly found nodes to make sure they are in fact closer to reach the end point than the original
						node = openSet[i];
				}
			}

			openSet.Remove(node); //remove the current node from the open set
			closedSet.Add(node); //add the cuirrent node to the closed set

			if (node == targetNode) {
				RetracePath(startNode,targetNode);
				return;
			}

			foreach (Node neighbour in grid.GetNeighbours(node)) {
				if (!neighbour.walkable || closedSet.Contains(neighbour)) {
					continue;
				}

				int newCostToNeighbour = node.gCost + GetDistance(node, neighbour); //check to see if the new node has a shorter path than the old node
				if (newCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour)) {
					neighbour.gCost = newCostToNeighbour;
					neighbour.hCost = GetDistance(neighbour, targetNode);
					neighbour.parent = node;

					if (!openSet.Contains(neighbour)) //add the neighbor to the open set if it's not already in there
						openSet.Add(neighbour);
				}
			}
		}
	}

	void RetracePath(Node startNode, Node endNode) {
		List<Node> path = new List<Node>();
		Node currentNode = endNode;

		while (currentNode != startNode) {
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}
		path.Reverse();

		grid.path = path;

	}

	int GetDistance(Node nodeA, Node nodeB) { //get the distance between two given nodes
		int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX); //subtract highest number by lowest number to reach the  most optimal path, so we use the absolute value
		int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

		if (dstX > dstY)
			return 14*dstY + 10* (dstX-dstY);
		return 14*dstX + 10 * (dstY-dstX);
	}
}