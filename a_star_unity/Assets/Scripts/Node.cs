﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node { //doesn't need to extend monobehavior 
	
	public bool walkable;
	public Vector3 worldPosition; //what point in the world the node represents
	public int gridX;
	public int gridY;

	public int gCost;
	public int hCost;
	public Node parent;
	
	public Node(bool _walkable, Vector3 _worldPos, int _gridX, int _gridY) { //assign worldPosition values when the node is created
		walkable = _walkable;
		worldPosition = _worldPos;
		gridX = _gridX;
		gridY = _gridY;
	}

	public int fCost {
		get {
			return gCost + hCost;
		}
	}
}