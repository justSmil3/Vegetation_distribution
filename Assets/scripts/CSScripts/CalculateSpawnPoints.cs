using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateSpawnPoints : MonoBehaviour
{
    public ComputeShader shader;
    public ComputeBuffer TerrainBuffer;
    public RenderTexture renderTexture;
    public RenderTexture rdt;
    public float[,] terrainData;
    public GameObject Terrain;
    public List<Plant> plantModels;
    // Start is called before the first frame update
    void Start()
    {
        terrainData = Terrain.GetComponent<Terrain>().terrainData.GetHeights(0,0,1024,1024);
    }

    // Update is called once per frame
    void Update()
    {
        RunShader();
    }

    void RunShader()
    {
        rdt = new RenderTexture(renderTexture);
        rdt.enableRandomWrite = true;
        rdt.Create();
        int kernelIndex = shader.FindKernel("CSMain");
        shader.SetTexture(kernelIndex, "Terrain", rdt);
        shader.Dispatch(kernelIndex, 1024 / 16, 1024 / 16,1);
        Graphics.CopyTexture(rdt, renderTexture);
    }
}
