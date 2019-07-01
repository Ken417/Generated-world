using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureGenerator : MonoBehaviour
{
    static Texture2D _lastUsedParlinNoiseTexture = null;
    public static Texture2D lastUsedParlinNoiseTexture
    {
        get
        {
            if(!_lastUsedParlinNoiseTexture)
            {
                CreateParlinNoseTexture(100,100,new Vector2(Random.value, Random.value),30);
            }
            return _lastUsedParlinNoiseTexture;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static Texture2D CreateParlinNoseTexture(int width,int height,Vector2 seed,float freq)
    {
        Color[] colorMap = new Color[width * height];
        for (int i = 0, y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++, i++)
            {
                float xSample = ((float)x/ width + seed.x) * freq;
                float zSample = ((float)y / height + seed.y) * freq;
                float noise = Mathf.PerlinNoise(xSample, zSample);
                colorMap[i] = new Color(noise, noise, noise, 1);
            }
        }
        Texture2D texture = new Texture2D(width,height);

        texture.SetPixels(colorMap);
        texture.Apply();

        _lastUsedParlinNoiseTexture = texture;

        return texture;
    }
}
