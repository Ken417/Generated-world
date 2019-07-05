using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseArray : MonoBehaviour
{
    ComputeShader shader;

    // Start is called before the first frame update
    void Start()
    {
        shader = Resources.Load<ComputeShader>("test");

    }

    //public static float[,] GetParlinNoiseArray(int width, int height, Vector2 seed, float freq)
    //{
    //    float[,] result = new float[width,height];
    //    ComputeShader shader = Resources.Load<ComputeShader>("PerlinNoise");
    //    shader.SetFloat("width", width);
    //    shader.SetFloat("height", height);
    //    shader.SetFloat("frequency", freq);
    //    shader.SetFloat("octaves", 0);
    //    shader.SetFloat("lacunarity", 2);
    //    shader.SetFloat("persistence", 0.6f);

    //    int num = width * height;
    //    ComputeBuffer buffer = new ComputeBuffer(num, sizeof(float));

    //    int kernelID = shader.FindKernel("PerlinNoise2DArray");
    //    shader.SetBuffer(kernelID, "resultBuf", buffer);
    //    shader.Dispatch(kernelID, width,height, 1);
    //    buffer.GetData(result);

    //    buffer.Release();
    //    return result;
    //}
}
