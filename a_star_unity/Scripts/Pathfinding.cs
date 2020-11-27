using System.Collections;
using System.Collections.Generic; //using a list
using UnityEngine;

public class Pathfinding : MonoBehaviour { //Awake(), Update()

		public Transform seeker, target; //point A and point B -- movable
		Grid grid; //creating a reference to grid

	void Awake() { //getting the grid
		grid = GetComponent<Grid> ();
	}

	void Update() { //updates the path
		FindPath (seeker.position, target.position);
	}

	void FindPath(Vector3 startPos, Vector3 targetPos) { //generates the path between point A and point B
	Node startNode = grid.NodeFromWorldPoint(startPos);
	Node targetNode = grid.NodeFromWorldPoint(targetPos);

	List<Node> openSet = new List<Node>();
	HashSet<Node> closedSet = new HashSet<Node>();

	openSet.Add(startNode);

	while (openSet.Count > 0) {
		Node node = openSet[0];

		for (int i = 1; i < openSet.Count; i++) {
			if (openSet[i].fCost < node.fCost || openSet[i].fCost == node.fCost) {
				if (openSet[i].hCost < node.hCost)
				node = openSet[i];
			}
		}
		openSet.Remove(node);
		closedSet.Add(node);

		if (node == targetNode) {

			RetracePath(startNode, targetNode);
			return;
		}

		foreach (Node neighbor in grid.Getneighbors(node)) {
			if (!neighbor.walkable || closedSet.Contains(neighbor)) {
				continue;
			}

			int newCostToNeighbor = node.gCost + GetDistance(node, neighbor);
			if (newcostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor)) {
				neighbor.gCost = newCostToNeighbor;
				neighbor.hCost = GetDistance(neighbor, targetNode);
				neighbor.parent = node;

				if (!openSet.Contains(neighbor))
				openSet.Add(neighbor);
			}
		}
	}
	
	}

	void RetracePath(Node startNode, Node endNode) { //retraces the path between point A and point B
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
		int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
		int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

		if (dstX > dstY) 
			return 14*dstY + 10* (dstX-dstY); //10 for up/down, 14 for diagonal
		return 14*dstX + 10 * (dstY-dstX);
    }

}