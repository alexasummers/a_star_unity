using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Node class basically defines our variables that helps create our grid of nodes, determine our world position, and dictate what is and is not an obstacle. 
//This will also help calculate our Fcost, which again is the gcost plus hcost.

public class Node { //doesn't need to extend monobehavior because the functionality provided is not used here
	
	public bool walkable; //walkable true/false
	public Vector3 worldPosition; //what point in the world the node represents
	public int gridX;
	public int gridY;

	public int gCost; //these are already declared for time element-- these are used in the pathfinding.cs to calculate the node evaluations
	public int hCost;
	public Node parent;
	
	public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY) { //assign worldPosition values when the node is created
		walkable = _walkable;
		worldPosition = _worldPos;
		gridX = _gridX;
		gridY = _gridY;
	}

	public int fCost { //never will have to assign to fcost-- will always be able to caluclate it from the gcost and hcost (therefore, no SET; only GET)
		get {
			return gCost + hCost;
		}
	}
}