using System.Collections;
using System.Collections.Generic; //added to use a list
using UnityEngine;

//This class will create our layermask that hides part of a layer without erasing or deleting it for the purpose of our obstacles, it will create and populate
//our grid with nodes, it will check the neighboring nodes cost values, and it will draw our physical path between our seeker and target.

public class Grid : MonoBehaviour {

	public LayerMask unwalkableMask;
	//LayerMask hides part of a layer without erasing or deleting
	public Vector2 gridWorldSize;
	//Vector2 used to represent 2D points positions using only X and Y    
	public float nodeRadius; 
	//how much space each individual node covers
	Node[,] grid; 
	//2d array of nodes to represent the grid

	float nodeDiameter;
	int gridSizeX, gridSizeY;

	void Awake() { //how many nodes (based on the radius) can we fit into our grid
		nodeDiameter = nodeRadius*2;
		gridSizeX = Mathf.RoundToInt(gridWorldSize.x/nodeDiameter); //rounds to the neariest integer to ensure we don't have a partial node
		gridSizeY = Mathf.RoundToInt(gridWorldSize.y/nodeDiameter);
		CreateGrid();
	}

	void CreateGrid() { //creates the grid of nodes and double checks for overlaps of outside grid objects
		grid = new Node[gridSizeX,gridSizeY]; //new 2d array of nodes
		Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.forward * gridWorldSize.y/2; //returning the bottom left corner of our world

		for (int x = 0; x < gridSizeX; x ++) { //performing collision checks to see if there is an overlap
			for (int y = 0; y < gridSizeY; y ++) {
				Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
				bool walkable = !(Physics.CheckSphere(worldPoint,nodeRadius,unwalkableMask)); //true if we don't collide with an obstacle
				grid[x,y] = new Node(walkable,worldPoint, x,y); //populating our grid with nodes
			}
		}
	}

	public List<Node> Getneighbors(Node node) { //double checks to see the neighboring nodes f, g and h costs.
		List<Node> neighbors = new List<Node>();

		for (int x = -1; x <= 1; x++) { //search in a 3x3 block to check for surrounding nodes
			for (int y = -1; y <= 1; y++) {
				if (x == 0 && y == 0)
					continue; //skips the iteration

				int checkX = node.gridX + x;
				int checkY = node.gridY + y;

				if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY) { //make sure the node is inside the grid
					neighbors.Add(grid[checkX,checkY]);
				}
			}
		}

		return neighbors;
	}
	

	public Node NodeFromWorldPoint(Vector3 worldPosition) { //converts the world position into a grid coordinate
		float percentX = (worldPosition.x + gridWorldSize.x/2) / gridWorldSize.x;
		float percentY = (worldPosition.z + gridWorldSize.y/2) / gridWorldSize.y;
		percentX = Mathf.Clamp01(percentX); //clamping between 0 and 1 so it does not go off the grid
		percentY = Mathf.Clamp01(percentY);

		int x = Mathf.RoundToInt((gridSizeX-1) * percentX);
		int y = Mathf.RoundToInt((gridSizeY-1) * percentY);
		return grid[x,y]; //returning the indices of the grid array
	}

	public List<Node> path; //draws the path between the two objects and changes the shortest path to black
	void OnDrawGizmos() { //creates the box around the grid
		Gizmos.DrawWireCube(transform.position,new Vector3(gridWorldSize.x,1,gridWorldSize.y)); //use y instead of z axis because the z axis is being represented by the y in the 3d space

		if (grid != null) {
			foreach (Node n in grid) {
				Gizmos.color = (n.walkable)?Color.white:Color.red; //distinguishing an obstacle from open ground
				if (path != null) //used in RetracePath from Pathfinding.CS to trace the path between the starting node and ending node
					if (path.Contains(n)) //used in RetracePath from Pathfinding.CS to trace the path between the starting node and ending node
						Gizmos.color = Color.black; //used in RetracePath from Pathfinding.CS to trace the path between the starting node and ending node
				Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter-.1f));
			}
		}
	}
}