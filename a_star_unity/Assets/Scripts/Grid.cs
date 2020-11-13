using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public Transform player;
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    Node[,] grid;

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    void Start () { //how many nodes can we fit into our grid
        nodeDiameter = nodeRadius*2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x/nodeDiameter); //how many nodes we can fit into worldSize.X
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y/nodeDiameter); //how many nodes we can fit into worldSize.Y
        CreateGrid();
    }
    
    void CreateGrid() {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x/2 - Vector3.forward * gridWorldSize.y/2;
       
        for (int x = 0; x < gridSizeX; x++)
         {
             for (int y = 0; y < gridSizeY; y++)
             {
                    Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                    bool walkable = !(Physics.CheckSphere(worldPoint,nodeRadius,unwalkableMask));
                    grid[x,y] = new Node(walkable,worldPoint);
             }
         }
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition) { //so the capsule knows which node it's standing on
        float percentX = (worldPosition.x + gridWorldSize.x/2) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y/2) / gridWorldSize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY); //if the world position is outside of the grid, it won't give us a weird percent.
    
        int x = Mathf.RoundToInt((gridSizeX -1) * percentX); //if we're on the far right of the grid, we will subtract one from that so we are not outside of the array
        int y = Mathf.RoundToInt((gridSizeY -1) * percentY);
        return grid[x,y];
    }



    void OnDrawGizmos() {
        Gizmos.DrawWireCube(transform.position,new Vector3(gridWorldSize.x,1,gridWorldSize.y));
       
       if (grid != null) {
           Node playerNode = NodeFromWorldPoint(player.position);
            foreach (Node n in grid) {
                Gizmos.color = (n.walkable)?Color.white:Color.red;
                if (playerNode == n) {
                    Gizmos.color = Color.cyan;
                }
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter-.1f));
             }
        }
    }
}
