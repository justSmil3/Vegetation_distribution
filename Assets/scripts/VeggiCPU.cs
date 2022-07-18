using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VeggiCPU : MonoBehaviour
{
    private float _radius;
    public int resolution = 1024;
    [Min(1)]
    public float radius = 1;
    public RenderTexture texture;
    public List<Plant> Plants;
    public TerrainData terrainData;
    private Vector3[] normals;
    private float[] map;
    [Range(0, 1)]
    public float th;
    [Range(0, 100)]
    public float intensifyer;

    public bool spawnTrees = false;

    public ComputeShader distribute;
    public ComputeShader visualize;
    public ComputeShader distance;
    public ComputeShader density;
    public Texture2D bayer_field;

    private ComputeBuffer debugBuffer;

    private ComputeBuffer normalBuffer;
    private ComputeBuffer mapBuffer;
    private ComputeBuffer spawnPosition;
    private RenderTexture output;
    private RenderTexture helper;
    private RenderTexture SDF;
    private ComputeBuffer Spawn;
    private float[,] heights;
    private List<GameObject> trees;
    // Start is called before the first frame update
    void Awake()
    {
        trees = new List<GameObject>();
        SimulateVeggi();

        normals = new Vector3[resolution * resolution];
        map = new float[resolution * resolution];

        for (int x = 0; x < resolution; x++)
            for(int y = 0; y < resolution; y++)
            {
                Vector3 normal = terrainData.GetInterpolatedNormal((float)x / resolution, (float)y / resolution);
                normals[x + y * resolution] = normal;
            }

        DetermineDensity();
        //DetermineHabitability();
        _radius = radius;
        if(spawnTrees)
        for(int i = 0; i < 100000; i++)
        {
            GameObject obj = Instantiate(Plants[0].vegetationPrefab);
            obj.transform.SetParent(this.gameObject.transform);
            trees.Add(obj);
            obj.SetActive(false);
        }

        heights = terrainData.GetHeights(0,0, resolution, resolution);
    }

    private GameObject GetTree()
    {
        if(trees.Count <= 0) return null;
        GameObject tree = trees[0];
        trees.RemoveAt(0);
        trees.Add(tree);
        return tree;
    }


    public void  SimulateVeggi()
    {
        // loop through avaliable bioms / areas 
            // search for most habitable plant
            // sre radius as kernel to loop through points on area
            // place plant 
    }
    bool one = true;
    private void FixedUpdate()
    {
        // foreach(Plant p in Plants)
        // {
        //     radius = p.radius;
        //     DetermineDensity();
        //     if(one){
        //         one = false;
        //         for (int x = 0; x < 100; x++)
        //         {
        //         for (int y = 0; y < 100; y++){
        //             float[] data = new float[resolution * resolution];
        //             Spawn.GetData(data);

        //             UnityEngine.Debug.Log(data);
        //             if(data[x + y * resolution] > 0){
        //                 GameObject tmp = Instantiate(p.vegetationPrefab, new Vector3(x, y, terrainData.GetHeights(x, y, 1, 1)[0,0]), Quaternion.identity);  
        //             }
        //         }
        //         }
        //     }
        // }
        if(radius != _radius)
        {

            foreach(GameObject tree in trees)
            {
                tree.SetActive(false);
            }
            DetermineDensity();
            _radius = radius;

            if(spawnTrees)
            {
                float[] spawn = new float[resolution * resolution];
                Spawn.GetData(spawn);
                for(int i = 0; i < resolution * resolution; i++)
                {   
                    int x_tmp = (int)(i % resolution);
                    int y_tmp = (int)(i / resolution);
                    float height = heights[y_tmp, x_tmp] * 146.7887f;
                    Vector3 pos = new Vector3(x_tmp, height,y_tmp);
                    if (spawn[i] > 0)
                    {
                        GameObject tmp = GetTree();
                        if(tmp == null) continue;
                        tmp.SetActive(true);
                        tmp.transform.localScale *= radius;
                        tmp.transform.position = pos;
                    }
                }
            }
        }
    }



    public void DetermineHabitability()
    {
        normalBuffer = new ComputeBuffer(resolution * resolution, sizeof(float) * 3);
        mapBuffer = new ComputeBuffer(resolution * resolution, sizeof(float));
        float pixelsize = 1.0f / resolution;

        output = new RenderTexture(resolution, resolution, 3);
        output.enableRandomWrite = true;
        normalBuffer.SetData(normals);
        mapBuffer.SetData(map);

        distribute.SetBuffer(0, "Result", mapBuffer);
        distribute.SetBuffer(0, "normals", normalBuffer);
        distribute.SetInt("res", resolution);
        distribute.SetFloat("th", th);

        distribute.Dispatch(0, resolution * resolution / 32, 1, 1);

        SDF = new RenderTexture(resolution, resolution, 3);
        SDF.enableRandomWrite = true;
        for (int i = (int)Mathf.Log(resolution, 2); i >= 0; i--)
        {
            distance.SetFloat("PixelSize", pixelsize);
            distance.SetTexture(0, "distanceField", SDF);
            distance.SetBuffer(0, "map", mapBuffer);
            distance.SetInt("resolution", resolution);
            distance.SetInt("offsetmult", i);
            distance.Dispatch(0, resolution / 8, resolution / 8, 1);
        }

        visualize.SetTexture(0, "Result", output);
        visualize.SetBuffer(0, "map", mapBuffer);
        visualize.SetInt("resolution", resolution);
        mapBuffer.GetData(map);
        visualize.Dispatch(0, resolution / 8, resolution / 8, 1);
        mapBuffer.GetData(map); 
        Graphics.CopyTexture(output, texture);
        normalBuffer.Dispose();
        mapBuffer.Dispose();
    }

    public void DetermineDensity()
    {
        normalBuffer = new ComputeBuffer(resolution * resolution, sizeof(float) * 3);
        normalBuffer.SetData(normals);
        output = new RenderTexture(resolution, resolution, 3);
        output.enableRandomWrite = true;
        helper = new RenderTexture(resolution, resolution, 3);
        Spawn = new ComputeBuffer(resolution * resolution, sizeof(float));
        Spawn.SetData(new float[resolution * resolution]);

        int distributionKernel = density.FindKernel("Distribution");
        int jfaKernel = density.FindKernel("JFA");
        int visulizeKernrl = density.FindKernel("Visulize");
        int djitterKernel = density.FindKernel("Djitter");
        int spawnKernel = density.FindKernel("SpawnPoints");

        density.SetFloat("th", th);
        density.SetInt("resolution", resolution); 
        density.SetFloat("intensifyer", intensifyer);
        density.SetFloat("radius", radius);

        density.SetBuffer(distributionKernel, "normals", normalBuffer);
        density.SetTexture(distributionKernel, "map", output);
        density.Dispatch(distributionKernel, resolution / 8, resolution / 8, 1);

        int numRuns = (int)Mathf.Log(resolution, 2);
        for(int i = 0; i < numRuns; i++)
        {
            density.SetInt("offsetmult", i);
            density.SetTexture(jfaKernel, "map", output);
            density.Dispatch(jfaKernel, resolution / 8, resolution / 8, 1);
        }
        density.SetTexture(visulizeKernrl, "map", output);
        density.Dispatch(visulizeKernrl, resolution / 8, resolution / 8, 1);
        Graphics.CopyTexture(output, helper);

        float[,] debuggingArray = new float[resolution * resolution,4];
        debugBuffer = new ComputeBuffer(resolution*resolution, sizeof(float) * 4);
        debugBuffer.SetData(debuggingArray);

        density.SetTexture(djitterKernel, "map", output);
        density.SetTexture(djitterKernel, "helper", helper);
        density.SetTexture(djitterKernel, "bayer", bayer_field);
        density.SetBuffer(djitterKernel, "spawn", Spawn);
        density.SetBuffer(djitterKernel, "debug", debugBuffer);
        density.Dispatch(djitterKernel, resolution / 8, resolution / 8, 1);


        debugBuffer.GetData(debuggingArray);
        // for(int i = 0; i < debuggingArray.Length / 4; i++){
        //     if( debuggingArray[i,0] != 0)
        //     Debug.Log(debuggingArray[i,0] + " | " + debuggingArray[i,1] + " | " + debuggingArray[i,2] + " | " + debuggingArray[i,3] + " | ");
        // }

        density.SetTexture(spawnKernel, "map", output);
        density.SetBuffer(spawnKernel, "spawn", Spawn);
        density.Dispatch(spawnKernel, resolution / 8, resolution / 8, 1);

        Graphics.CopyTexture(output, texture);
        normalBuffer.Dispose();
    }
}