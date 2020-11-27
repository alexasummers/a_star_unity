using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node { //no monobehaviour
	
	public bool walkable;
	public Vector3 worldPosition; //in vector 3 format for x,y and z positions
	public int gridX;
	public int gridY;

	public int gCost; //general cost of movement
	public int hCost; //heuristic cost
	public Node parent;
	
	public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY) { //assigns worldPosition
		walkable = _walkable;
		worldPosition = _worldPos;
		gridX = _gridX;
		gridY = _gridY;
	}

	public int fCost { //gCost + hCost
		get {
			return gCost + hCost;
		}
	}
}