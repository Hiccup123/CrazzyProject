using UnityEngine;
using System.Collections;
using System.Collections.Generic; 

[RequireComponent(typeof(Camera))]
public class TestMRT : MonoBehaviour
{
    public Material testMRTMaterial = null;
    public RenderTexture[] mrtTex = new RenderTexture[2];
    public RenderTexture[] TextureA = new RenderTexture[2];
    RenderBuffer[] mrtRB = new RenderBuffer[2];

    public Camera effectCame;
    
    public RenderTexture sourTexture;
    public RenderTexture canYanTexture;
    public RenderTexture curTexture;
    public Material CreateCanYinMaterial;

    void Start()
    {
        TextureA[0] = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
        TextureA[1] = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);

        Shader testMRT = Shader.Find("Custom/TestMRT");
        if (testMRT.isSupported)
            testMRTMaterial = new Material(testMRT);
        else
            return;
        Shader CanyanCreat = Shader.Find("Custom/CanYingCreate");
        if (CanyanCreat.isSupported)
            CreateCanYinMaterial = new Material(CanyanCreat);
        else
            return;
        StartCoroutine(ShowEffect());
    }
    
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        sourTexture = source;
        //Graphics.Blit(sourTexture, source, testMRTMaterial, 0);  
        if (!testMRTMaterial.shader.isSupported)
        {
            Graphics.Blit(source, destination);
            return;
        }

        //Show the result  
        testMRTMaterial.SetTexture("_Tex0", sourTexture);
        testMRTMaterial.SetTexture("_Tex1", curTexture);
        //Graphics.Blit(curTexture, destination, testMRTMaterial, 2);  
        Graphics.Blit(null, destination, testMRTMaterial, 2);

    }

    Dictionary<Renderer, Material[]> rendToMat = new Dictionary<Renderer, Material[]>();

    void Clear(RenderTexture destTexture)
    {
        Graphics.SetRenderTarget(destTexture);
        GL.PushMatrix();
        GL.Clear(true, true, Color.white);
        GL.PopMatrix();
    }

    void InitCanYanTexture()
    {
        playRoot = GameObject.Find("Players").transform;
        Transform[] chs = playRoot.GetComponentsInChildren<Transform>();
        int effectLayer = LayerMask.NameToLayer("ImageEffect");
        for (int i = 0, iMax = chs.Length; i < iMax; i++)
        {
            chs[i].gameObject.layer = effectLayer;
            Renderer red = chs[i].GetComponent<Renderer>();
            if (red != null)
            {
                if (red.sharedMaterial.shader.name == "Toon/Basic" ||
                red.gameObject.name == "m_battle_box01")
                {
                    Material newMt = GameObject.Instantiate(CreateCanYinMaterial) as Material;
                    newMt.mainTexture = red.sharedMaterial.mainTexture;
                    rendToMat.Add(red, new Material[] { red.sharedMaterial, newMt });
                }
                else
                    Debug.Log(red.sharedMaterial.shader.name);
            }

        }
        effectCame.cullingMask = 1 << effectLayer;
        canYanTexture = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
        effectCame.targetTexture = canYanTexture;
    }
    void CreateCanYanTexture()
    {
        int colorId = Shader.PropertyToID("_Color");
        Shader sd = CreateCanYinMaterial.shader;
        float i = 0;
        foreach (KeyValuePair<Renderer, Material[]> pair in rendToMat)
        {
            i += 1f;
            Renderer rd = pair.Key;
            rd.sharedMaterial = rendToMat[rd][1];
            rd.sharedMaterial.SetColor(colorId, new Color(0, 0, 1, 0.7f + 0.1f * i));
        }
        //GL.Clear(true, true, Color.clear);  
        effectCame.Render();
        foreach (KeyValuePair<Renderer, Material[]> pair in rendToMat)
        {
            Renderer rd = pair.Key;
            rd.sharedMaterial = rendToMat[rd][0];
        }


    }
    private Transform playRoot;
    IEnumerator ShowEffect()
    {
        InitCanYanTexture();
        int ii = 0;
        while (true)
        {
            CreateCanYanTexture();
            yield return new WaitForSeconds(0.01f);
            int a = ii % 2;//0;//  
            int b = ++ii % 2;//1;//  
            curTexture = TextureA[a];
            Graphics.SetRenderTarget(curTexture);
            GL.Clear(false, false, Color.clear);
            testMRTMaterial.SetTexture("_Tex0", sourTexture);
            testMRTMaterial.SetTexture("_Tex1", TextureA[b]);
            testMRTMaterial.SetTexture("_MainTex", canYanTexture);

            GL.PushMatrix();

            GL.LoadOrtho();

            testMRTMaterial.SetPass(1);     //Pass 0 outputs 2 render textures.  

            GL.Begin(GL.QUADS);

            GL.TexCoord2(0.0f, 0); GL.Vertex3(0.0f, 0.0f, 0.1f);

            GL.TexCoord2(1.0f, 0); GL.Vertex3(1.0f, 0.0f, 0.1f);

            GL.TexCoord2(1.0f, 1); GL.Vertex3(1.0f, 1.0f, 0.1f);

            GL.TexCoord2(0.0f, 1); GL.Vertex3(0.0f, 1.0f, 0.1f);

            GL.End();

            GL.PopMatrix();

        }
    }
}

//[RequireComponent(typeof(Camera))]
//public class TestMRT : MonoBehaviour
//{
//    public Material testMRTMaterial = null;
//    public RenderTexture[] mrtTex = new RenderTexture[2];
//    public RenderTexture[] TextureA = new RenderTexture[2];
//    RenderBuffer[] mrtRB = new RenderBuffer[2];


//    public Camera effectCame;

//    public Texture texture0;
//    public Texture texture1;
//    public RenderTexture sourTexture;
//    public RenderTexture canYanTexture;
//    public RenderTexture curTexture;
//    public Material CreateCanYinMaterial;
//    void Start()
//    {
//        TextureA[0] = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
//        TextureA[1] = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);

//        Shader testMRT = Shader.Find("Custom/TestMRT");
//        if (testMRT.isSupported)
//            testMRTMaterial = new Material(testMRT);
//        else
//            return;
//        Shader CanyanCreat = Shader.Find("Custom/CanyanCreat");
//        if (CanyanCreat.isSupported)
//            CreateCanYinMaterial = new Material(CanyanCreat);
//        else
//            return;
//        StartCoroutine(ShowEffect());
//    }


//    void OnRenderImage(RenderTexture source, RenderTexture destination)
//    {
//        sourTexture = source;
//        //Graphics.Blit(sourTexture, source, testMRTMaterial, 0);  
//        if (!testMRTMaterial.shader.isSupported)
//        {
//            Graphics.Blit(source, destination);
//            return;

//        }

//        //Show the result  
//        testMRTMaterial.SetTexture("_Tex0", sourTexture);
//        testMRTMaterial.SetTexture("_Tex1", curTexture);
//        //Graphics.Blit(curTexture, destination, testMRTMaterial, 2);  
//        Graphics.Blit(null, destination, testMRTMaterial, 2);

//    }

//    Dictionary<Renderer, Material[]> rendToMat = new Dictionary<Renderer, Material[]>();

//    void Clear(RenderTexture destTexture)
//    {
//        Graphics.SetRenderTarget(destTexture);
//        GL.PushMatrix();
//        GL.Clear(true, true, Color.white);
//        GL.PopMatrix();
//    }

//    void InitCanYanTexture()
//    {
//        //effectCame;  
//        GameObject cameObj = new GameObject("effectCame");
//        cameObj.transform.parent = Camera.main.gameObject.transform;
//        cameObj.transform.localPosition = Vector3.zero;
//        effectCame = cameObj.AddComponent<Camera>();
//        effectCame.CopyFrom(Camera.main);
//        effectCame.clearFlags = CameraClearFlags.SolidColor;
//        effectCame.backgroundColor = Color.clear;
//        effectCame.enabled = false;

//        if (playRoot == null)
//            if (GameObject.Find("Players"))
//                playRoot = GameObject.Find("Players").transform;
//        if (playRoot == null)
//            playRoot = GameObject.Find("GameOnly").transform.FindChild("SceneParent/PlayerRoot");

//        Transform[] chs = playRoot.GetComponentsInChildren<Transform>();
//        int effectLayer = LayerMask.NameToLayer("ImageEffect");
//        for (int i = 0, iMax = chs.Length; i < iMax; i++)
//        {
//            chs[i].gameObject.layer = effectLayer;
//            Renderer red = chs[i].GetComponent<Renderer>();
//            if (red != null)
//            {
//                if (red.sharedMaterial.shader.name == "Toon/Basic" ||
//                red.gameObject.name == "m_battle_box01" ||
//                red is SkinnedMeshRenderer)
//                {
//                    Material newMt = GameObject.Instantiate(CreateCanYinMaterial) as Material;
//                    newMt.mainTexture = red.sharedMaterial.mainTexture;
//                    rendToMat.Add(red, new Material[] { red.sharedMaterial, newMt });
//                }
//                else
//                    Debuger.Log(red.sharedMaterial.shader.name);
//            }

//        }
//        effectCame.cullingMask = 1 << effectLayer;
//        canYanTexture = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
//        effectCame.targetTexture = canYanTexture;
//        camePos = Camera.main.transform.position;
//        tarPos = tarObj.position;
//        ccpos = Camera.main.transform.position - tarObj.position;
//    }

//    void CreateCanYanTexture()
//    {
//        int colorId = Shader.PropertyToID("_Color");
//        Shader sd = CreateCanYinMaterial.shader;
//        float i = 0;
//        foreach (KeyValuePair<Renderer, Material[]> pair in rendToMat)
//        {
//            i += 1f;
//            Renderer rd = pair.Key;
//            rd.sharedMaterial = rendToMat[rd][1];
//            rd.sharedMaterial.SetColor(colorId, new Color(0, 0, 1, 0.7f + 0.1f * i));
//        }
//        //GL.Clear(true, true, Color.clear);  
//        effectCame.Render();
//        foreach (KeyValuePair<Renderer, Material[]> pair in rendToMat)
//        {
//            Renderer rd = pair.Key;
//            rd.sharedMaterial = rendToMat[rd][0];
//        }

//    }
//    public Transform tarObj;
//    Vector3 camePos = Vector3.zero;
//    Vector3 tarPos = Vector3.zero;

//    Vector3 ccpos = Vector3.zero;
//    void Update()
//    {
//        //Camera.main.transform.position = tarObj.position + ccpos;  
//    }
//    private Transform playRoot;
//    IEnumerator ShowEffect()
//    {
//        InitCanYanTexture();
//        int ii = 0;
//        Vector4 cameMove = Vector4.zero;

//        while (true)
//        {
//            CreateCanYanTexture();

//            if (camePos != Camera.main.transform.position)
//            {
//                Vector3 newTarPos = tarObj.position;
//                Vector2 pm1 = effectCame.WorldToScreenPoint(tarPos);
//                Vector2 pm2 = effectCame.WorldToScreenPoint(newTarPos);
//                tarPos = newTarPos;
//                camePos = effectCame.transform.position;
//                cameMove = pm2 - pm1;
//            }
//            cameMove.z = Screen.width;
//            cameMove.w = Screen.height;

//            yield return new WaitForSeconds(0.01f);
//            int a = ii % 2;//0;//  
//            int b = ++ii % 2;//1;//  
//            curTexture = TextureA[a];
//            Graphics.SetRenderTarget(curTexture);
//            GL.Clear(false, false, Color.clear);
//            GL.PushMatrix();

//            GL.LoadOrtho();
//            testMRTMaterial.SetTexture("_Tex0", sourTexture);
//            testMRTMaterial.SetTexture("_Tex1", TextureA[b]);
//            testMRTMaterial.SetTexture("_MainTex", canYanTexture);
//            testMRTMaterial.SetVector("_cameMove", cameMove);

//            testMRTMaterial.SetPass(1);     //Pass 0 outputs 2 render textures.  

//            GL.Begin(GL.QUADS);

//            GL.TexCoord2(0.0f, 0); GL.Vertex3(0.0f, 0.0f, 0.1f);

//            GL.TexCoord2(1.0f, 0); GL.Vertex3(1.0f, 0.0f, 0.1f);

//            GL.TexCoord2(1.0f, 1); GL.Vertex3(1.0f, 1.0f, 0.1f);

//            GL.TexCoord2(0.0f, 1); GL.Vertex3(0.0f, 1.0f, 0.1f);

//            GL.End();

//            GL.PopMatrix();

//        }
//    }
//}