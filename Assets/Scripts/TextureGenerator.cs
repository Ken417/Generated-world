using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class TextureGenerator : MonoBehaviour
{
    static ComputeShader perlinNoise = null;
    static ComputeShader cellularNoise = null;
    static ComputeShader voronoiNoise = null;
    static ComputeShader testComp = null;
    
    //コンピュートシェーダーでテクスチャを作る場合
    public static RenderTexture CreateTextureForCompute(int width, int height, RenderTextureFormat format = RenderTextureFormat.Default)
    {
        RenderTexture tex = new RenderTexture(width, height, 0, format);
        tex.enableRandomWrite = true;
        tex.Create();
        return tex;
    }

    static Dictionary<RenderTexture, AsyncGPUReadbackRequest> waitingConversionTex = new Dictionary<RenderTexture, AsyncGPUReadbackRequest>();
    //RenderTextureをTexture2Dに変えたいときにここに登録
    static void RegistAsyncGPUReadback(RenderTexture rt)
    {
        if (waitingConversionTex.Equals(rt)) {return; }
        AsyncGPUReadbackRequest reqest = AsyncGPUReadback.Request(rt);
        waitingConversionTex.Add(rt, reqest);
    }

    //RenderTextureをTexture2Dに変えたいときに登録したものができていればTexture2Dを返す。
    static Texture2D GetTexture2D_ByAsyncGPUReadback(RenderTexture rt)
    {
        if(waitingConversionTex[rt].done)
        {
            Texture2D tex = new Texture2D(rt.width,rt.height);
            Unity.Collections.NativeArray<Color32> buffer = waitingConversionTex[rt].GetData<Color32>();
            tex.LoadRawTextureData(buffer);
            tex.Apply();
            waitingConversionTex.Remove(rt);
            return tex;
        }
        return null;
    }

    struct Voronoi
    {
        public Vector2 position;
        public Color color;
    }

    public static Texture2D CreateVoronoiDiagramTexture(int width, int height)
    {
        List<Voronoi> list = new List<Voronoi>();
        Voronoi v = new Voronoi();
        Color[] colorMap = new Color[width * height];
        int r = (width + height) / 2;
        for (int i = 0, y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++, i++)
            {
                int point = Random.Range(0, r);
                if(point >= r-1)
                {
                    v.position.x = x;
                    v.position.y = y;
                    v.color = Random.ColorHSV();
                    v.color[Random.Range(0, 3)] = Random.Range(1f, 2f);
                    list.Add(v);
                    colorMap[i] = Color.black;
                }
                else
                {
                    colorMap[i] = Color.white;
                }
            }
        }
        Vector2 vec = Vector2.zero;
        for (int i = 0, y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++, i++)
            {
                if(colorMap[i] == Color.white)
                {
                    Voronoi min = new Voronoi();
                    min.position = new Vector2(width + 1, height + 1);
                    foreach(Voronoi p in list)
                    {
                        vec.x = x;
                        vec.y = y;
                        if ((p.position - vec).magnitude < (min.position- vec).magnitude)
                        {
                            min = p;
                        }
                    }
                    colorMap[i] = min.color;
                }
            }
        }
        Texture2D texture = new Texture2D(width, height);

        texture.SetPixels(colorMap);
        texture.Apply();


        return texture;
    }

    public static void RenderCellularNoise(
        RenderTexture rt, 
        Vector2 seed,
        float lightness,
        float scale,
        float octaves,
        float lacunarity,
        float persistence
        )
    {
        if(cellularNoise == null)
        {
            cellularNoise = Resources.Load<ComputeShader>("CellularNoise");
        }
        cellularNoise.SetVector("seed", seed);
        cellularNoise.SetFloat("lightness", lightness);
        cellularNoise.SetFloat("scale", scale);
        cellularNoise.SetFloat("octaves", octaves);
        cellularNoise.SetFloat("lacunarity", lacunarity);
        cellularNoise.SetFloat("persistence", persistence);

        int kernelID = cellularNoise.FindKernel("CellularNoise");
        cellularNoise.SetTexture(kernelID,"resultTex",rt);
        cellularNoise.Dispatch(kernelID, rt.width, rt.height, 1);

    }

    public static void RenderVoronoiNoiseColor(
    RenderTexture rt,
    Vector2 seed,
    float lightness,
    float scale,
    float octaves,
    float lacunarity,
    float persistence
    )
    {
        if (voronoiNoise == null)
        {
            voronoiNoise = Resources.Load<ComputeShader>("VoronoiNoise");
        }
        voronoiNoise.SetVector("seed", seed);
        voronoiNoise.SetFloat("lightness", lightness);
        voronoiNoise.SetFloat("scale", scale);
        voronoiNoise.SetFloat("octaves", octaves);
        voronoiNoise.SetFloat("lacunarity", lacunarity);
        voronoiNoise.SetFloat("persistence", persistence);

        int kernelID = voronoiNoise.FindKernel("VoronoiNoiseColor");
        voronoiNoise.SetTexture(kernelID, "resultTex", rt);
        voronoiNoise.Dispatch(kernelID, rt.width, rt.height, 1);

    }

    public static void RenderVoronoiNoise(
    RenderTexture rt,
    Vector2 seed,
    float lightness,
    float scale,
    float octaves,
    float lacunarity,
    float persistence
    )
    {
        if (voronoiNoise == null)
        {
            voronoiNoise = Resources.Load<ComputeShader>("VoronoiNoise");
        }
        voronoiNoise.SetVector("seed", seed);
        voronoiNoise.SetFloat("lightness", lightness);
        voronoiNoise.SetFloat("scale", scale);
        voronoiNoise.SetFloat("octaves", octaves);
        voronoiNoise.SetFloat("lacunarity", lacunarity);
        voronoiNoise.SetFloat("persistence", persistence);

        int kernelID = voronoiNoise.FindKernel("VoronoiNoise");
        voronoiNoise.SetTexture(kernelID, "resultTex", rt);
        voronoiNoise.Dispatch(kernelID, rt.width, rt.height, 1);

    }

    public static void RenderVoronoiNoiseEdge(
    RenderTexture rt,
    Vector2 seed,
    float lightness,
    float scale,
    float octaves,
    float lacunarity,
    float persistence
    )
    {
        if (voronoiNoise == null)
        {
            voronoiNoise = Resources.Load<ComputeShader>("VoronoiNoise");
        }
        voronoiNoise.SetVector("seed", seed);
        voronoiNoise.SetFloat("lightness", lightness);
        voronoiNoise.SetFloat("scale", scale);
        voronoiNoise.SetFloat("octaves", octaves);
        voronoiNoise.SetFloat("lacunarity", lacunarity);
        voronoiNoise.SetFloat("persistence", persistence);

        int kernelID = voronoiNoise.FindKernel("VoronoiNoiseEdge");
        voronoiNoise.SetTexture(kernelID, "resultTex", rt);
        voronoiNoise.Dispatch(kernelID, rt.width, rt.height, 1);

    }

    public static void RenderPerlinNoise(
    RenderTexture rt,
    Vector2 seed,
    float lightness,
    float freq,
    float octaves,
    float lacunarity,
    float persistence
    )
    {
        if (perlinNoise == null)
        {
            perlinNoise = Resources.Load<ComputeShader>("PerlinNoise");
        }
        perlinNoise.SetVector("seed", seed);
        perlinNoise.SetFloat("lightness", lightness);
        perlinNoise.SetFloat("frequency", freq);
        perlinNoise.SetFloat("octaves", octaves);
        perlinNoise.SetFloat("lacunarity", lacunarity);
        perlinNoise.SetFloat("persistence", persistence);

        int kernelID = perlinNoise.FindKernel("PerlinNoise2DTex");
        perlinNoise.SetTexture(kernelID, "resultTex", rt);
        perlinNoise.Dispatch(kernelID, rt.width, rt.height, 1);

    }

    public static void TestComputeTex(
        RenderTexture rt,
        string kernaelName,
        float f1,
        float f2,
        float f3,
        int i1,
        int i2,
        int i3
        )
    {
        if (testComp == null)
        {
            testComp = Resources.Load<ComputeShader>("TEST");
        }
        testComp.SetFloat("f1", f1);
        testComp.SetFloat("f2", f2);
        testComp.SetFloat("f3", f3);
        testComp.SetInt("i1", i1);
        testComp.SetInt("i2", i2);
        testComp.SetInt("i3", i3);

        int kernelID = testComp.FindKernel(kernaelName);
        testComp.SetTexture(kernelID, "resultTex", rt);
        testComp.Dispatch(kernelID, rt.width, rt.height, 1);
    }

    public static Texture2D CreateRandomNoiseTexture(int width, int height)
    {
        Color[] colorMap = new Color[width * height];
        for (int i = 0, y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++, i++)
            {
                float noise = Random.Range(0f,1f); ;
                colorMap[i] = new Color(noise, noise, noise, 1);
            }
        }
        Texture2D texture = new Texture2D(width, height);

        texture.SetPixels(colorMap);
        texture.Apply();

        return texture;
    }

    public static Texture2D CreateUnityPerlinNoise2DTexture()
    {
        return null;
    }

}
