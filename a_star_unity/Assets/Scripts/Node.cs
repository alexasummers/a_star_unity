using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public bool obstacle;
    public Vector3 worldPosition;
    
    public Node(bool _obstacle, Vector3 _worldPosition) 
    {
        obstacle = _obstacle;
        worldPosition = _worldPosition;
    }
}
