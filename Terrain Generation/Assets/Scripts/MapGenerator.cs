using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;
using UnityEngine;
using Random = System.Random;


public class MapGenerator : MonoBehaviour
{
    public enum DrawMode
    {
        DrawNoise,
        DrawColor,
        DrawMesh,
        DrawEnviroment

    };


    public DrawMode mode = DrawMode.DrawNoise;

    public const int mapChunkSize = 241;
    [Range(0, 6)] public int levelOfDetail;

    public float noiseScale;
    
    public int octaves;
    [Range(0, 1)] public float persistence;
    public float lacunarity;
    public Vector2 offset;
    public int seed;
    public float heightScale;

    public AnimationCurve meshHeightCurve;
    
    public MapDisplay mapDisplay;

    public bool autoUpdate;
    public TerrainType[] regions;

    public Vector3 objectOffset;
    
    private Queue<MapThreadInfo<MapData>> mapDataThreadInfoQueue = new Queue<MapThreadInfo<MapData>>();
    private Queue<MapThreadInfo<MeshData>> meshDataThreadInfoQueue = new Queue<MapThreadInfo<MeshData>>();

    public void DrawMapInEditor()
    {
        MapData mapData = GenerateMapData();
        if(mode == DrawMode.DrawNoise) mapDisplay.DrawTexture(TextureGenerator.TextureFromHeightMap(mapData.heightMap));
        
        else if(mode == DrawMode.DrawColor) mapDisplay.DrawTexture(TextureGenerator.TextureFromColorMap(mapData.colors, mapChunkSize, mapChunkSize));
        
        else if(mode == DrawMode.DrawMesh) mapDisplay.DrawMesh(MeshGenerator.GenerateTerrainMesh(mapData.heightMap, heightScale, meshHeightCurve, levelOfDetail), TextureGenerator.TextureFromColorMap(mapData.colors, mapChunkSize, mapChunkSize));
        else if (mode == DrawMode.DrawEnviroment) mapDisplay.DrawEnviroment(MeshGenerator.GenerateTerrainMesh(mapData.heightMap, heightScale, 
            meshHeightCurve, levelOfDetail), TextureGenerator.TextureFromColorMap(mapData.colors, mapChunkSize, mapChunkSize), mapData.spawnObjectOnPoint, mapChunkSize, objectOffset);
    }

    public void RequestMapData(Action<MapData> callback)
    {
        ThreadStart threadStart = delegate
        {
            MapDataThread(callback);
        };
        
        new Thread(threadStart).Start();
    }

    void MapDataThread(Action<MapData> callback)
    {
        MapData mapData = GenerateMapData ();
        lock (mapDataThreadInfoQueue)
        {
            mapDataThreadInfoQueue.Enqueue(new MapThreadInfo<MapData>(callback, mapData));
        }
    }

    public void RequestMeshData(MapData mapData, Action<MeshData> callBack)
    {
        ThreadStart threadStart = delegate
        {
            MeshDataThread(mapData, callBack);
        };
        
        new Thread(threadStart).Start();
    }

    void MeshDataThread(MapData mapData, Action<MeshData> callBack)
    {
        MeshData meshData =
            MeshGenerator.GenerateTerrainMesh(mapData.heightMap, heightScale, meshHeightCurve, levelOfDetail);
        
        lock(meshDataThreadInfoQueue)
        {
            meshDataThreadInfoQueue.Enqueue(new MapThreadInfo<MeshData>(callBack, meshData));
        }
    }

    private void Update()
    {
        if (mapDataThreadInfoQueue.Count > 0)
        {
            for (int i = 0; i < mapDataThreadInfoQueue.Count; i++)
            {
                MapThreadInfo<MapData> threadInfo = mapDataThreadInfoQueue.Dequeue();
                threadInfo.callBack(threadInfo.parameter);
            }
        }        
        if (meshDataThreadInfoQueue.Count > 0)
        {
            for (int i = 0; i < meshDataThreadInfoQueue.Count; i++)
            {
                MapThreadInfo<MeshData> threadInfo = meshDataThreadInfoQueue.Dequeue();
                threadInfo.callBack(threadInfo.parameter);
            }
        }
    }

    public MapData GenerateMapData()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, seed, noiseScale, octaves, persistence, lacunarity, offset);
        
        Color[] colors = new Color[mapChunkSize * mapChunkSize];

        bool[] spawnObj = new bool[mapChunkSize * mapChunkSize];

        for (int y = 0; y < mapChunkSize; y++)
        {
            for (int x = 0; x < mapChunkSize; x++)
            {
                foreach (TerrainType t in regions)
                {
                    if (noiseMap[x, y] <= t.height)
                    {
                        colors[y * mapChunkSize + x] = t.color;
//                        if (t.placeable != null)
//                        {
//                            float randomVal = new System.Random().;
//                            if (System.Random. > 0.99f)
//                            {
//                                spawnObj[y * mapChunkSize + x] = true;
//                            }
//                        }
                        break;
                    }
                }
            }
        }

        
        return new MapData(noiseMap, colors, spawnObj);


    }

    private void OnValidate()
    {

        if (lacunarity < 1)
        {
            lacunarity = 1;
        }
        if (octaves < 1)
        {
            octaves = 1;
        }
        
    }
    public static UnityEngine.Object SafeDelete(UnityEngine.Object obj)
    {
        if (obj != null)
        {
            if (Application.isEditor)
            {
                UnityEngine.Object.DestroyImmediate(obj);
            }
            else
            {
                UnityEngine.Object.Destroy(obj);
            }
        }

        return null;
    }

    struct MapThreadInfo<T>
    {
        public readonly Action<T> callBack;
        public readonly T parameter;

        public MapThreadInfo(Action<T> callBack, T parameter)
        {
            this.callBack = callBack;
            this.parameter = parameter;
        }
    }
    
}

[System.Serializable]
public struct TerrainType
{
    public string Name;
    public float height;
    public Color color;
    public GameObject placeable;
}

public struct MapData
{
    public readonly float[,] heightMap;
    public readonly Color[] colors;
    public readonly bool[] spawnObjectOnPoint;

    public MapData(float[,] heightMap, Color[] colors, bool[] spawnObjectOnPoint)
    {
        this.heightMap = heightMap;
        this.colors = colors;
        this.spawnObjectOnPoint = spawnObjectOnPoint;
    }
}

