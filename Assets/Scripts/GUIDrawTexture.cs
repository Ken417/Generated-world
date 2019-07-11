using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIDrawTexture : MonoBehaviour
{
    List<Texture2D> textures = new List<Texture2D>();
    List<RenderTexture> rTextures = new List<RenderTexture>();

    public Vector2 seed = Vector2.zero;
    public float scale = 3;
    [Range(-1f,1f)]
    public float lightness = 0;
    public float f1 = 0;
    public float f2 = 0;
    public float f3 = 0;
    public int i1 = 0;
    public int i2 = 0;
    public int i3 = 0;

    int width = 500;
    int height = 500;


    void Start()
    {
        //textures.Add(new Texture2D(100, 100));

        rTextures.Add(TextureGenerator.CreateTextureForCompute(width, height));
        rTextures.Add(TextureGenerator.CreateTextureForCompute(width, height));
        rTextures.Add(TextureGenerator.CreateTextureForCompute(width, height));



        Texture2D texture;
        texture = new Texture2D(width, height);
        textures.Add(texture);

    }


    void Update()
    {
        f1 = scale;
        TextureGenerator.RenderPerlinNoise(rTextures[0], seed, lightness,scale, 2, 0, 0.7f);
        TextureGenerator.RenderCellularNoise(rTextures[1], seed, lightness, scale, 4, 0, 0.7f);
        TextureGenerator.RenderVoronoiNoiseEdge(rTextures[2], seed, lightness, scale, 4, 0, 0.7f);

        Color[] colorMap = new Color[width * height];
        for (int i = 0, y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++, i++)
            {
                float noise = Mathf.PerlinNoise((float)x/ width * scale + seed.x, (float)y / height * scale + seed.y) + lightness;
                colorMap[i] = new Color(noise, noise, noise, 1);
            }
        }

        textures[0].SetPixels(colorMap);
        textures[0].Apply();
    }

    void OnGUI()
    {
        int w = Screen.width / 2;
        int h = Screen.height / 2;
        int s = 512 / (textures.Count + rTextures.Count);



        for (int i = 0; i < textures.Count; i++)
        {
            GUI.DrawTexture(new Rect((w - s / 2) + s*i - (s/2)*(textures.Count + rTextures.Count - 1), h - s / 2, s, s), textures[i]);
        }

        for (int i = 0; i < rTextures.Count; i++)
        {
            GUI.DrawTexture(new Rect((w - s / 2) + s * (textures.Count + i) - (s / 2) * (textures.Count + rTextures.Count - 1), h - s / 2, s, s), rTextures[i]);
        }
    }
}
