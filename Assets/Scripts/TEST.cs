using UnityEngine;
using System.Collections;

public class TEST : MonoBehaviour
{
    ComputeShader shader;

    RenderTexture tex;


    public float freq = 100;
    public int res = 100;
    void Start()
    {
        shader = Resources.Load<ComputeShader>("test");
        tex = new RenderTexture(64, 64, 0);
        tex.enableRandomWrite = true;
        tex.Create();


        shader.SetFloat("w", tex.width);
        shader.SetFloat("h", tex.height);
        shader.SetTexture(0, "tex", tex);
    }

    void Update()
    {
        shader.SetFloat("fr", freq);
        shader.SetInt("re", res);
        shader.Dispatch(0, tex.width / 8, tex.height / 8, 1);
    }

    void OnGUI()
    {
        int w = Screen.width / 2;
        int h = Screen.height / 2;
        int s = 512;

        GUI.DrawTexture(new Rect(w - s / 2, h - s / 2, s, s), tex);
    }

    void OnDestroy()
    {
        tex.Release();
    }
}