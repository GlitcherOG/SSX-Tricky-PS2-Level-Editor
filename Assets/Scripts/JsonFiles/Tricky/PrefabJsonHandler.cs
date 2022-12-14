using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using UnityEngine;

namespace SSXMultiTool.JsonFiles.Tricky
{
    public class PrefabJsonHandler
    {
        public List<PrefabJson> PrefabJsons = new List<PrefabJson>();

        public void CreateJson(string path)
        {
            var serializer = JsonUtility.ToJson(this);
            File.WriteAllText(path, serializer);
        }

        public static PrefabJsonHandler Load(string path)
        {
            string paths = path;
            if (File.Exists(paths))
            {
                var stream = File.ReadAllText(paths);
                var container = JsonUtility.FromJson<PrefabJsonHandler>(stream);
                return container;
            }
            else
            {
                return new PrefabJsonHandler();
            }
        }

        [Serializable]
        public struct PrefabJson
        {
            public string PrefabName;
            public int MaterialBlockID;
            public int Unknown3;
            public float AnimTime;
            public List<ObjectHeader> PrefabObjects;
        }
        [Serializable]
        public struct ObjectHeader
        {
            public int ParentID;
            public int Flags;
            public int AnimOffset; //Replace With animation ID or something later

            public List<MeshHeader> MeshData;

            public float[] Position;
            public float[] Rotation;
            public float[] Scale;
        }
        [Serializable]
        public struct MeshHeader
        {
            public string MeshPath;
            public int MeshID;
            public int MaterialID;

        }
    }
}
