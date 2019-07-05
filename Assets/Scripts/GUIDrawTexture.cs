using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIDrawTexture : MonoBehaviour
{
    Texture2D texture = null;

    Texture2D[] textures = null;

    public float freq = 3;


    public int index = 0;

    void Start()
    {
        if(!texture)
        {
            textures = new Texture2D[2];

            textures[0] = new Texture2D(100,100);
            Color[] colors = new Color[100*100];
            float[,] noise = NoiseArray.GetParlinNoiseArray(100,100,new Vector2(0,0),3);
            for (int v = 0, x = 0; x < noise.GetLength(1); x++)
            {
                for (int y = 0; y < noise.GetLength(0); y++,v++)
                {
                    colors[v] = new Color(noise[x, y], noise[x, y], noise[x, y], 1);
                }
            }
            textures[0].SetPixels(colors);
            textures[0].Apply();

            textures[1] = new Texture2D(100, 100);
            for (int v = 0, x = 0; x < 100; x++)
            {
                for (int y = 0; y < 100; y++, v++)
                {
                    float f = Mathf.PerlinNoise((float)x/100*freq, (float)y/100 * freq);
                    colors[v] = new Color(f, f, f, 1);
                }
            }
            textures[1].SetPixels(colors);
            textures[1].Apply();

            texture = textures[index];
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            index++;
            index %= textures.Length;
            texture = textures[index];
        }
        Color[] colors = new Color[100 * 100];
        for (int v = 0, x = 0; x < 100; x++)
        {
            for (int y = 0; y < 100; y++, v++)
            {
                float f = Mathf.PerlinNoise((float)x / 100 * freq, (float)y / 100 * freq);
                colors[v] = new Color(f, f, f, 1);
            }
        }
        textures[1].SetPixels(colors);
        textures[1].Apply();
    }

    void OnGUI()
    {
        int w = Screen.width / 2;
        int h = Screen.height / 2;
        int s = 512;

        GUI.DrawTexture(new Rect(w - s / 2, h - s / 2, s, s), texture);
    }
}
