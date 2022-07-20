using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct layer
{
    public List<Plant> p;
    public List<int> pools;
}

public class DensityPipeline : MonoBehaviour
{
    private const int BAYER_RESOLUTION = 256;
    private const int BAYER_RADIUS = 8;
    private const int MAX_TEMPERATURE = 50;
    private const int MAX_PRECIPITATION = 400;

    public ObjectSpawner spawner;
    public RenderTexture testrender;
    public int resolution = 1024;
    public Biom biom;
    public float intensifyer = 1.0f;
    public int[,] layerCollision;

    [Range(0, 1)]
    public float viabilityThreshhold = 0;
    
    private ComputeBuffer debugBuffer;
    private ComputeBuffer normalBuffer;
    private ComputeBuffer spawnPointBuffer;

    Vector2[] spawnPoints;

    [UnityEngine.SerializeField]
    private ComputeShader DensityPipe;
    [UnityEngine.SerializeField]
    private ComputeShader GeneratePipe;
    [UnityEngine.SerializeField]
    private ComputeShader InitShader;
    [UnityEngine.SerializeField]
    private ComputeShader CollisionPipe;
    [UnityEngine.SerializeField]
    private ComputeShader DjitterPipe;

    public TerrainData terrainData;

    private List<RenderTexture> stackedDensity;
    private RenderTexture mask;

    private float[,] heights;
    [UnityEngine.SerializeField]
    public List<layer> layers;
    //private List<RenderTexture> densityStack;
    // Start is called before the first frame update
    void Start()
    {
        // var test = Bayer_Matrix.Instance.BayerMatrix;
        stackedDensity = new List<RenderTexture>();
        //densityStack = new List<RenderTexture>();
        heights = terrainData.GetHeights(0,0, resolution, resolution);
        for(int j = 0; j < layers.Count; j++)
        {
            List<Plant> plants = layers[j].p;
            RenderTexture stackedMap = new RenderTexture(resolution, resolution, 3);
            stackedMap.enableRandomWrite = true;
            stackedDensity.Add(stackedMap);
            for(int i = 0; i < plants.Count; i++){
                RenderTexture _tex = new RenderTexture(resolution, resolution, 3);
                _tex.enableRandomWrite = true;
                //densityStack.Add(_tex);
                plants[i].map = _tex;
                // if(plants[i].vegetationPrefab)
                // {
                //     int id = spawner.InitiatePool(/*this, */plants[i].vegetationPrefab, 100000);
                //     layers[j].pools.Add(id);
                // }
            }
        }

        DispatchInitShader(); 
        ExecutePipeline();
    }

    private void FixedUpdate() 
    {
        ExecutePipeline();
    }

    private void ExecutePipeline()
    {
        // TODO remove all the debug visuals including the white spawning points
        DispatchDensityPipeline();
        DispatchGeneratePipeLine();
        DispatchDjitterPipeLine();
        DispatchCollisionPipeLine(); // TODO i also have to remove the spawnpoints but that will be hadled once the spawnsystem is done i guess
    }

    private void DispatchInitShader()
    {
        Vector3[] normals = new Vector3[resolution * resolution];
        for (int x = 0; x < resolution; x++)
            for(int y = 0; y < resolution; y++)
            {
                Vector3 normal = terrainData.GetInterpolatedNormal((float)x / resolution, (float)y / resolution);
                normals[x + y * resolution] = normal;
            }
        
        normalBuffer = new ComputeBuffer(resolution * resolution, 12);
        normalBuffer.SetData(normals);
        InitShader.SetBuffer(0, "normals", normalBuffer);
        InitShader.SetFloat("th", 0.7f);
        InitShader.SetInt("resolution", resolution);
        mask = new RenderTexture(resolution, resolution, 3);
        mask.enableRandomWrite = true;
        InitShader.SetTexture(0, "Result", mask);
        InitShader.Dispatch(0, resolution/8, resolution / 8, 1);
        normalBuffer.Dispose();
        normalBuffer = null;
    }

    private void DispatchDensityPipeline()
    {
        int kernel = 0;//DensityPipe.FindKernel("CSMain");

        for(int i = 0; i < layers.Count; i++)
        {
            // maybe sume research about how well it could work with just justing a stacked version

            // get the interesting values of that layer
            List<Plant> _plants = layers[i].p;
            DensityPipe.SetFloat("numPlantDivider", 1.0f);
            // Find a better way to do this, but it'll do for now
            DensityPipe.SetTexture(1, "DensityMap", stackedDensity[i]);
            DensityPipe.Dispatch(1, resolution/8, resolution/8, 1);
            float totalViability = 0.0f;
            float maxViability = 0.0f;

            for (int j = 0; j < _plants.Count; j++)
            {
                Plant plant = _plants[j];
                float precipitation = plant.precipitation;
                float temperature = plant.temerature + 10;
                float biomTemperature = biom.averageTemperature + 10;
                float biomPrecipitation = biom.averagePrecipitation;

                temperature = 1 - (Mathf.Abs(biomTemperature - temperature) / MAX_TEMPERATURE);
                precipitation = 1 - (Mathf.Abs(biomPrecipitation - precipitation) / MAX_PRECIPITATION);

                float viability = Mathf.Pow(0.5f * precipitation + 0.5f * temperature, 3);
                if(viability > viabilityThreshhold)
                {
                    totalViability += viability;
                }
                else
                {
                    viability = 0.0f;
                }
                plant.viability = viability;
                maxViability = viability > maxViability ? viability : maxViability;
            }

            for (int j = 0; j < _plants.Count; j++)
            {
                Plant plant = _plants[j];
                // think about weather to communicate forward the precipitation and temperature values
                // or to calculate the viabilities here and just forward those.
                DensityPipe.SetFloat("viability", plant.viability);
                DensityPipe.SetFloat("plantViabilityDivider", maxViability / totalViability);
                DensityPipe.SetTexture(kernel, "DensityMap", stackedDensity[i]);
                DensityPipe.SetTexture(kernel, "PlantDensity", plant.map);
                DensityPipe.SetTexture(kernel, "Mask", mask);


                DensityPipe.Dispatch(kernel, resolution/8, resolution/8, 1);
            }
        }
    }

    private void DispatchGeneratePipeLine()
    {
        int kernel = 0;
        int numRuns = (int)Mathf.Log(resolution, 2);
        GeneratePipe.SetInt("resolution", resolution);

        for(int i = 0; i < stackedDensity.Count; i++)
        {
            for(int j = 0; j < numRuns; j++)
            {
                GeneratePipe.SetInt("offsetmult", j);
                GeneratePipe.SetTexture(kernel, "map", stackedDensity[i]);
                GeneratePipe.Dispatch(kernel, resolution / 8, resolution / 8, 1);
            }
            GeneratePipe.SetTexture(1, "map", stackedDensity[i]);
            GeneratePipe.Dispatch(1, resolution / 8, resolution / 8, 1);
        }
    }


    // TODO test this once object spawning is done and this is fitted to support object spawning
    private void DispatchCollisionPipeLine()
    {
        // TODO predifine collision matrix. handling it intern does not really make sense
        // but this one is the temporary setup of the layer collision matrix
        layerCollision = new int[layers.Count,layers.Count];
        int tmpV = layers.Count - 1;
        for(int i = 0; i < tmpV; i++)
        {
            List<RenderTexture> layersToCollide = new List<RenderTexture>();
            for(int j = i; j < tmpV; j++)
            {
                //if(layerCollision[i, tmpV - 1 - j] == 1)
                    layersToCollide.Add(stackedDensity[j+1]);
            }
            for(int j = 0; j < layersToCollide.Count; j++)
            {
                CollisionPipe.SetTexture(0, "Map", stackedDensity[i]);
                CollisionPipe.SetTexture(0, "Collision", layersToCollide[j]);
                CollisionPipe.SetInt("resolution", resolution);
                CollisionPipe.SetFloat("radius", layers[i].p[0].radius);
                CollisionPipe.Dispatch(0, resolution / 8, resolution / 8, 1);
            }
        }
    }
    private bool debugSpawn = true;
    private void DispatchDjitterPipeLine()
    {   
        int kernel = 0;
        // StructuredBuffer<float3> bayer;
        Vector3[] bayer_matrix = Bayer_Matrix.Instance.BayerMatrix;
        int len = bayer_matrix.GetLength(0);
        ComputeBuffer bayerBuffer = new ComputeBuffer(len, 12);
        bayerBuffer.SetData(bayer_matrix);
        DjitterPipe.SetBuffer(kernel, "bayer", bayerBuffer);
        
        // RWStructuredBuffer<float2> points;
        RenderTexture dt = new RenderTexture(resolution, resolution, 3);
        dt.enableRandomWrite = true;

        for(int i = 0; i < layers.Count; i++)
        {
            DjitterPipe.SetTexture(kernel, "map", stackedDensity[i]);
            List<Plant> _plants = layers[i].p;
            for(int j = 0; j < _plants.Count; j++)
            {
                float rescaleFactorRes = Mathf.Pow(resolution / 256, 2);
                float tmpFood = layers[i].p[j].radius;

                DjitterPipe.SetInt("len", len);
                // float scaler = len * radmult * rescaleFactorRes;
                // len = (int)(Mathf.Pow(resolution / (tmpFood * 2), 2));
                int numBatches = (int)Mathf.Pow((resolution / BAYER_RESOLUTION), 2);
                float radiusNumberModyfier = (float)BAYER_RADIUS / tmpFood;
                float radiusLengthModyfier = tmpFood / (float)BAYER_RADIUS;
                int rows = (int)(4 * radiusNumberModyfier) + 1;
                // rows = rows % (int)rows > 0 ? rows + 1 : rows;
                DjitterPipe.SetInt("rows", rows);
                int scaler = (int)(len * numBatches * Mathf.Pow(radiusNumberModyfier, 2)) * 2;
                // add one line of points for overflow
                scaler += len * rows;
                spawnPointBuffer = new ComputeBuffer(scaler, 8);
                spawnPoints = new Vector2[scaler];
                DjitterPipe.SetBuffer(kernel, "points", spawnPointBuffer);
                // RWTexture2D<float4> map;

                DjitterPipe.SetTexture(kernel, "dt", dt);
                DjitterPipe.SetTexture(kernel, "mult", layers[i].p[j].map);
                DjitterPipe.SetFloat("intensifyer", intensifyer);
                // int resolution;
                DjitterPipe.SetInt("resolution", resolution);
                // float rescaleFactorRad; // calculate on cpu by calculating 8 / footprint
                DjitterPipe.SetFloat("rescaleFactorRad", radiusLengthModyfier);
                // float rescaleFactorRes; // calc on cpu by calculating res / bayerres
                // DjitterPipe.SetFloat("rescaleFactorRes", rescaleFactorRes);

                DjitterPipe.Dispatch(kernel, scaler / 32, 1, 1);
                spawnPointBuffer.GetData(spawnPoints);
                if(debugSpawn)
                // foreach (var point in spawnPoints)
                // {
                //     if(layers[i].pools.Count <= j) continue;
                //     if(point != Vector2.zero) 
                //     {   
                //         Vector3 position = new Vector3(point.x, heights[(int)point.y, (int)point.x] * 146.7887f, point.y);
                //         GameObject test = spawner.SpawnObject(layers[i].pools[j], position, Quaternion.identity);
                //     }
                // }
                spawnPointBuffer.Dispose();
                spawnPointBuffer = null;
            }
            debugSpawn = false;
        }

        Graphics.CopyTexture(dt, testrender);

        //spawnPointBuffer.GetData(spawnPoints);
        bayerBuffer.Dispose();
        bayerBuffer = null;
    }

    private void OnDisable() {
    }
}
