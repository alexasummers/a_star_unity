using System.Collections;
using System.Collections.Generic; //using a list
using UnityEngine;

public class Pathfinding : MonoBehaviour { //must extend monobehavior because we are using Awake() and Update()

		public Transform seeker, target; //point A and point B -- movable
		Grid grid; //creating a reference to grid

	void Awake() { //getting the grid
		grid = GetComponent<Grid> ();
	}

	void Update() { //updates the path drawn between point A and point B as we move the points
		FindPath (seeker.position, target.position);
	}

	void FindPath(Vector3 startPos, Vector3 targetPos) { //generates the path between point A and point B
		
	}

	void RetracePath(Node startNode, Node endNode) { //traces the path between point A and point B
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
			return 14*dstY + 10* (dstX-dstY);
		return 14*dstX + 10 * (dstY-dstX);
    }

}