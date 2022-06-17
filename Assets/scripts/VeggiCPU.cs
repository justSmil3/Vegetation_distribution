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

    public ComputeShader distribute;
    public ComputeShader visualize;

    private ComputeBuffer normalBuffer;
    private ComputeBuffer mapBuffer;
    private RenderTexture output;
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

        DetermineHabitability();
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
        DetermineHabitability();
    }

    public void DetermineHabitability()
    {
        normalBuffer = new ComputeBuffer(resolution * resolution, sizeof(float) * 3);
        mapBuffer = new ComputeBuffer(resolution * resolution, sizeof(float));

        output = new RenderTexture(resolution, resolution, 3);
        output.enableRandomWrite = true;
        normalBuffer.SetData(normals);
        mapBuffer.SetData(map);

        distribute.SetBuffer(0, "Result", mapBuffer);
        distribute.SetBuffer(0, "normals", normalBuffer);
        distribute.SetInt("res", resolution);
        distribute.SetFloat("th", th);

        distribute.Dispatch(0, resolution * resolution / 32, 1, 1);

        visualize.SetTexture(0, "Result", output);
        visualize.SetBuffer(0, "map", mapBuffer);
        visualize.SetInt("resolution", resolution);
        mapBuffer.GetData(map);
        float currentTmp = 0;
        foreach (float x in map)
        {   
            if(currentTmp != x)
            {
                currentTmp = x;
            }
        }
        visualize.Dispatch(0, resolution / 8, resolution / 8, 1);

        Graphics.CopyTexture(output, texture);
        normalBuffer.Dispose();
        mapBuffer.Dispose();
    }
}