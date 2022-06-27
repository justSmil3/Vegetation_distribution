using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VeggiCPU : MonoBehaviour
{
    public int resolution = 1024;
    public RenderTexture texture;
    public List<Plant> Plants;
    public TerrainData terrainData;
    private Vector3[] normals;
    private float[] map;
    [Range(0, 1)]
    public float th;
    [Range(0, 100)]
    public float intensifyer;

    public ComputeShader distribute;
    public ComputeShader visualize;
    public ComputeShader distance;
    public ComputeShader density;
    public texture bayer_field;

    private ComputeBuffer normalBuffer;
    private ComputeBuffer mapBuffer;
    private RenderTexture output;
    private RenderTexture SDF;
    // Start is called before the first frame update
    void Start()
    {
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
    }

    public void  SimulateVeggi()
    {
        // loop through avaliable bioms / areas 
            // search for most habitable plant
            // sre radius as kernel to loop through points on area
            // place plant 
    }

    private void FixedUpdate()
    {
        DetermineDensity();
        //DetermineHabitability();
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

        int distributionKernel = density.FindKernel("Distribution");
        int jfaKernel = density.FindKernel("JFA");
        int visulizeKernrl = density.FindKernel("Visulize");
        int djitterKernel = density.FindKernel("Djitter");

        density.SetFloat("th", th);
        density.SetInt("resolution", resolution); 
        density.SetFloat("intensifyer", intensifyer);

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

        density.SetTexture(djitterKernel, "map", output);
        density.SetTexture(djitterKernel, "bayer", bayer_field);

        Graphics.CopyTexture(output, texture);
        normalBuffer.Dispose();
    }
}