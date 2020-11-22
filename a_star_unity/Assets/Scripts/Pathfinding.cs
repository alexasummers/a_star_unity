using System.Collections;
using System.Collections.Generic; //using a list
using UnityEngine;

public class Pathfinding : MonoBehaviour { //must extend monobehavior because we are using some of the funcitonality

	public Transform seeker, target; //point A and point B -- movable
	Grid grid; //creating a reference to our grid


	void Awake() { //getting the grid
		grid = GetComponent<Grid> (); //must be on the same game object, which is our Grid
	}


//-------------------------------------------------------------
	//DO THE UPDATE() LAST
	void Update() { //updates the path drawn between point A and point B as we move the points
		FindPath (seeker.position, target.position);
	}
//-------------------------------------------------------------



	void FindPath(Vector3 startPos, Vector3 targetPos) { //generates the path between point A and point B
		Node startNode = grid.NodeFromWorldPoint(startPos); //convert world positions into nodes (from Grid.cs, NodeFromWorldPoint) Point A
		Node targetNode = grid.NodeFromWorldPoint(targetPos); //Point B

		List<Node> openSet = new List<Node>(); //creating a list of nodes for the open set-- the set that has yet to be evaluated. List is used because 
		//we can search for the node with the lowest fCost and easy to add and take away from
		//This is the most expensive part of this, so it can be optimized but since this is a basic implementation, we're not getting into that.
		HashSet<Node> closedSet = new HashSet<Node>();  //creating a hashset of nodes for the closed set -- the set that has already been evaluated and/or visited. Create a hashset
		//rather than a list for this one because it is easy to add to and take away from
		openSet.Add(startNode); //add the starting node to the open set

		while (openSet.Count > 0) {//iterate through the open set
			Node node = openSet[0]; //equal to the first element in the open set
			for (int i = 1; i < openSet.Count; i ++) { //loop through all thenodes in the open set to find the node with the lowest fcost.
			//i=1 because we're started at index zero in the open set.
				if (openSet[i].fCost < node.fCost || openSet[i].fCost == node.fCost) { //if the node in the open set has a less than the current node's Fcost, 
					if (openSet[i].hCost < node.hCost) //evaluate the hCosts of the newly found nodes to make sure they are in fact closer to reach the end point than the original
						node = openSet[i]; //then the current node is updated with the index of the node in the open set
				}
			}

			openSet.Remove(node); //remove the current node from the open set
			closedSet.Add(node); //add the current node to the closed set

			if (node == targetNode) {//when we have found our path, we want to go ahead and trace it from point a to point b

//GO DOWN AND DEFINE RETRACE PATH
				RetracePath(startNode,targetNode);
				return;
			}

			foreach (Node neighbor in grid.Getneighbors(node)) { //checking the neighboring node's f, g and hcosts-- defined in Grid.CS
				if (!neighbor.walkable || closedSet.Contains(neighbor)) { //if the neighboring node has an obstacle, it can get added to the closed set.
					continue;
				}
//GO DOWN AND DEFINE GETDISTANCE AT THE BOTTOM
				int newCostToNeighbor = node.gCost + GetDistance(node, neighbor); //check to see if the new node has a shorter path than the old node. Cost of the current node's gcost plus the distance of the current node to the neighbor
				if (newCostToNeighbor < neighbor.gCost || !openSet.Contains(neighbor)) {//or if the neighbor is not currently in the open list
					neighbor.gCost = newCostToNeighbor; //newCostToNeighbor is now the gCost
					neighbor.hCost = GetDistance(neighbor, targetNode); //the hcost is equal to the distance from the node we're looking at to the end node
					neighbor.parent = node; //parent is defined in Node.CS--want to set the current node to be the neighbor's parent.

					if (!openSet.Contains(neighbor)) //check to see if the neigbor is in the open set. If not, add it to the open set.
						openSet.Add(neighbor);
				}
			}
		}
	}

	void RetracePath(Node startNode, Node endNode) {
		List<Node> path = new List<Node>();
		Node currentNode = endNode; //which will trace the path backwards because that's how the parents work

		while (currentNode != startNode) {
			path.Add(currentNode); //add the current node to our path
			currentNode = currentNode.parent;//retraces our steps from the ending node back to the starting node
		}
		path.Reverse(); //since the path is in reverse, we'll go ahead and reverse it again to make it go the correct direction

		grid.path = path; //this will draw our path out with the Gizmos function, which was defined in our Grid.CS (line 78-80)

	}

	int GetDistance(Node nodeA, Node nodeB) { //get the distance between two given nodes
		int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX); //subtract highest number by lowest number to reach the  most optimal path, so we use the absolute value
		int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

		if (dstX > dstY) //basically will calcualte how many moves we need.
			return 14*dstY + 10* (dstX-dstY); //14 is because that's how much each diagonal move costs, and 10 is because that's how much each horizontal/vertical move costs.
		//but if dstx is less than dstY, then we'll flip so the smaller number is being subtracted from the bigger one
		return 14*dstX + 10 * (dstY-dstX);
	}
}