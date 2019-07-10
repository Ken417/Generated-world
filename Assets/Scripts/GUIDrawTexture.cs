using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIDrawTexture : MonoBehaviour
{
    List<Texture2D> textures = new List<Texture2D>();
    List<RenderTexture> rTextures = new List<RenderTexture>();

    public float freq = 3;

    public float f1 = 0;
    public float f2 = 0;
    public float f3 = 0;
    public int i1 = 0;
    public int i2 = 0;
    public int i3 = 0;



    void Start()
    {
        //textures.Add(new Texture2D(100, 100));

        rTextures.Add(TextureGenerator.CreateTextureForCompute(100,100));
        rTextures.Add(TextureGenerator.CreateTextureForCompute(100,100));
    }

    void Update()
    {
        TextureGenerator.RenderParlinNoise(rTextures[0], Vector2.zero, freq, 4, 0, 0.7f);
        TextureGenerator.TestComputeTex(rTextures[1],"TEST2", f1,f2,f3,i1,i2,i3);
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
