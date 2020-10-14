using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour
{
    public Renderer textureRender;
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;
    public Transform mesh;
    public GameObject gm;

    public void DrawTexture(Texture2D texture)
    {
        textureRender.sharedMaterial.mainTexture = texture;
        textureRender.transform.localScale = new Vector3(texture.width, 1, texture.height);

    }

    public void DrawMesh(MeshData meshData, Texture2D texture)
    {
        meshFilter.sharedMesh = meshData.CreateMesh();
        meshRenderer.sharedMaterial.mainTexture = texture;
        
    }
    
    public void DrawEnviroment(MeshData meshData, Texture2D texture, bool[] spawnObject, int meshDimension, Vector3 offset)
    {
        meshFilter.sharedMesh = meshData.CreateMesh();

        for(int y=0; y < meshDimension - 1; y++)
        {
            for (int x = 0; x < meshDimension - 1; x++)
            {
                if (spawnObject[y * meshDimension + x])
                {
                    GameObject cube = Instantiate(gm, Vector3.Scale(meshData.vertices[y * meshDimension + x] + offset, mesh.localScale), Quaternion.identity);
                    cube.transform.SetParent(mesh.transform);
                }
               
            }
        }
        meshRenderer.sharedMaterial.mainTexture = texture;
        
    }
    
}
