using Dummiesman;
using SSXMultiTool.JsonFiles.Tricky;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SkyboxManager : MonoBehaviour
{
    public bool active = true;
    string LoadPath;
    public static SkyboxManager Instance;
    public Camera SkyboxCamera;

    public Material SkyboxMaterial;
    public GameObject Skybox;
    
    public PrefabJsonHandler prefabJson;
    public MaterialJsonHandler materialJson;
    public MaterialBlockJsonHandler materialBlock;
    public List<Texture2D> textures;
    public List<ModelObject> modelObjects = new List<ModelObject>();
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(active)
        {
            //Located in Mouse Look Script
            //Skybox.transform.eulerAngles = new Vector3(-90, 0, 0);
        }
    }

    public void LoadSkyboxData(string StringPath)
    {
        StringPath = Path.GetDirectoryName(StringPath);
        LoadPath = StringPath;
        if (File.Exists(StringPath + "\\MaterialBlocks.json"))
        {
            materialBlock = MaterialBlockJsonHandler.Load(StringPath + "\\MaterialBlocks.json");
            materialJson = MaterialJsonHandler.Load(StringPath + "\\Material.json");
            LoadModels(StringPath + "\\ModelHeaders.json");
            LoadTextures(StringPath + "\\Textures");

            if (modelObjects.Count != 0)
            {
                for (int i = 0; i < modelObjects[0].meshes.Count; i++)
                {
                    var NewObject = new GameObject();
                    NewObject.layer = 8;
                    NewObject.transform.parent = Skybox.transform;
                    NewObject.transform.localPosition = new Vector3(0, 0, 0);
                    NewObject.transform.localRotation = new Quaternion(0, 0, 0, 0);
                    var Renderer = NewObject.AddComponent<MeshRenderer>();
                    var Filter = NewObject.AddComponent<MeshFilter>();
                    Material newSkyboxMat = new Material(SkyboxMaterial);
                    newSkyboxMat.SetTexture("_MainTexture", textures[i]);
                    Filter.mesh = modelObjects[0].meshes[i];
                    Renderer.material = newSkyboxMat;
                }
            }
            SkyboxCamera.backgroundColor = textures[0].GetPixel(textures[0].width - 1, textures[0].height - 1);
        }
    }

    void LoadModels(string Path)
    {
        modelObjects = new List<ModelObject>();
        prefabJson = PrefabJsonHandler.Load(Path);
        for (int i = 0; i < prefabJson.PrefabJsons.Count; i++)
        {
            var TempModelJson = prefabJson.PrefabJsons[i];
            ModelObject mObject = new ModelObject();
            GameObject gameObject = new OBJLoader().Load(LoadPath + "/Models/" + i.ToString() + ".obj", null);
            var Meshes = gameObject.GetComponentsInChildren<MeshFilter>();
            for (int a = 0; a < Meshes.Length; a++)
            {
                mObject.meshes.Add(Meshes[a].mesh);
            }
            Destroy(gameObject);

            mObject.ModelName = TempModelJson.PrefabName;
            modelObjects.Add(mObject);
        }
    }

    void LoadTextures(string Folder)
    {
        string[] Files = Directory.GetFiles(Folder);
        textures = new List<Texture2D>();
        for (int i = 0; i < Files.Length; i++)
        {
            Texture2D NewImage = new Texture2D(1, 1);
            if (Files[i].ToLower().Contains(".png"))
            {
                using (Stream stream = File.Open(Files[i], FileMode.Open))
                {
                    byte[] bytes = new byte[stream.Length];
                    stream.Read(bytes, 0, (int)stream.Length);
                    NewImage.LoadImage(bytes);
                    //NewImage.filterMode = FilterMode.Point;
                    //NewImage.wrapMode = TextureWrapMode.MirrorOnce;
                }
                textures.Add(NewImage);
            }
        }
    }
}
