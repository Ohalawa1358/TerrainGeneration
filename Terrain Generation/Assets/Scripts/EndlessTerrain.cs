using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessTerrain : MonoBehaviour
{
    public const float maxViewDst = 450;
    public Transform viewer;
    public Material mapMaterial;
    private static MapGenerator mapGenerator;
    public static Vector2 viewerPosition;

    private int chunkSize;
    private int chunksVisibleInViewDst;
    
    Dictionary<Vector2, TerrainChunk> terrainChunkDictionary = new Dictionary<Vector2, TerrainChunk>();
    private List<TerrainChunk> chunksVisibleLastUpdate = new List<TerrainChunk>();

    private void Start()
    {
        mapGenerator = FindObjectOfType<MapGenerator>();
        chunkSize = MapGenerator.mapChunkSize - 1;
        chunksVisibleInViewDst = Mathf.RoundToInt( maxViewDst / chunkSize );
        
    }

    private void Update()
    {
        viewerPosition = new Vector2(viewer.position.x, viewer.position.z);
        UpdateVisibleChunks();
    }

    public void UpdateVisibleChunks()
    {

        foreach (TerrainChunk t in chunksVisibleLastUpdate)
        {
            t.SetVisible(false);
        }
        chunksVisibleLastUpdate.Clear();
        
        int currentChunkCoordX = Mathf.RoundToInt(viewerPosition.x / chunkSize);
        int currentChunkCoordY = Mathf.RoundToInt(viewerPosition.y / chunkSize);

        for (int yOffset = -chunksVisibleInViewDst; yOffset <= chunksVisibleInViewDst; yOffset++)
        {
            for (int xOffset = -chunksVisibleInViewDst; xOffset <= chunksVisibleInViewDst; xOffset++)
            {
                Vector2 visableTerrainChunkCoord = new Vector2(currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);

                if (terrainChunkDictionary.ContainsKey(visableTerrainChunkCoord))
                {
                    terrainChunkDictionary[visableTerrainChunkCoord].UpdateTerrainChunk();
                    if (terrainChunkDictionary[visableTerrainChunkCoord].IsVisible())
                    {
                        chunksVisibleLastUpdate.Add(terrainChunkDictionary[visableTerrainChunkCoord]);
                    }
                }
                else
                {
                    terrainChunkDictionary.Add(visableTerrainChunkCoord, new TerrainChunk(visableTerrainChunkCoord, chunkSize, transform, mapMaterial));
                }
            }
            
        }
    }

    public class TerrainChunk
    {
        private GameObject meshObject;
        private Vector2 position;

        private MapData mapData;
        private Bounds bounds;

        private MeshFilter meshFilter;
        private MeshRenderer meshRenderer;
        
        public TerrainChunk(Vector2 coord, int size, Transform parent, Material material)
        {
            position = coord * size;
            bounds = new Bounds(position, Vector2.one * size);
            Vector3 positionV3 = new Vector3(position.x, 0, position.y);
            
            meshObject = new GameObject("TerrainChunk");
            meshRenderer = meshObject.AddComponent<MeshRenderer>();
            meshFilter = meshObject.AddComponent<MeshFilter>();
            meshRenderer.material = material;
            
            meshObject.transform.SetParent(parent);
            meshObject.transform.position = positionV3;
            SetVisible(false);
            
            mapGenerator.RequestMapData(OnMapDataReceived);
        }

        public void UpdateTerrainChunk()
        {
            float viewerDistanceFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(viewerPosition));

            bool visible = viewerDistanceFromNearestEdge <= maxViewDst;
            
            SetVisible(visible);
        }

        public void SetVisible(bool visible)
        {
            meshObject.SetActive(visible);
        }

        public bool IsVisible()
        {
            return meshObject.activeSelf;
        }

        void OnMapDataReceived(MapData mapData)
        {
            mapGenerator.RequestMeshData(mapData, OnMeshDataReceived);
        } 
        
        void OnMeshDataReceived(MeshData meshData)
        {
            meshFilter.mesh = meshData.CreateMesh();
        }
    }
}
