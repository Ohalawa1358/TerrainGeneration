using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class Grid : MonoBehaviour
{
    public int xSize;
    public int ySize;

    private Vector3[] vertices;

    private void Awake()
    {
        Generate();
    }

    private void Generate()
    {
        vertices = new Vector3[(xSize + 1) * (ySize + 1)];
        for (int y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                vertices[((xSize + 1) * y) + x] = new Vector3(x, y);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (vertices == null) return;
        
        Gizmos.color = Color.black;
        foreach (var vert in vertices)
        {
            Gizmos.DrawSphere(vert, .1f);
        }
    }
}
