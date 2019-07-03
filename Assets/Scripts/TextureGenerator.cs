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
                CreateParlinNoiseTexture(100,100,new Vector2(Random.value, Random.value),30);
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

        _lastUsedParlinNoiseTexture = texture;

        return texture;
    }

    public static Texture2D CreateParlinNoiseTexture(int width,int height,Vector2 seed,float freq)
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

        _lastUsedParlinNoiseTexture = texture;

        return texture;
    }
}
