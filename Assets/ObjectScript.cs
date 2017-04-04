using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ObjectScript : MonoBehaviour {
    private static float maxw = 300, maxh = 300, margin = 10;
    public float w = 0, h = 0, aspect = 1;
    public MeshCollider collider;
    public RenderTexture previewTexture;

    public void click() {
        FileSelectionScript fss = GameObject.Find("FileSelectionPanel").GetComponent<FileSelectionScript>();
        if (fss.selectedPhoto) {
            string depthPath = null;
            if (fss.selectedDepth != null)
                depthPath = fss.selectedDepth.GetComponent<FileEntry>().path;
            load(fss.selectedPhoto.GetComponent<FileEntry>().path, depthPath);
        }
        fss.close();
    }

    public void load(string filepath, string depthFilepath) {
        //depthFilepath is optional
        GetComponent<Animator>().Play("ObjectAppearAnimation", 0);
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        ModelLoader.mModel model = ModelLoader.loadModel(filepath, depthFilepath);
        meshFilter.mesh = model.mesh;
        collider = GetComponent<MeshCollider>(); //.sharedMesh = model.mesh;
        GetComponent<MeshRenderer>().material.mainTexture = model.tex;

        w = model.tex.width;
        h = model.tex.height;
        aspect = w / h;

        Mesh mesh = new Mesh();
        mesh.vertices = new Vector3[] {
            new Vector3(-0.5f * aspect, 0.5f, 0),
            new Vector3(-0.5f * aspect, -0.5f, 0),
            new Vector3(0.5f * aspect, 0.5f, 0),
            new Vector3(0.5f * aspect, 0.5f, 0),
            new Vector3(-0.5f * aspect, -0.5f, 0),
            new Vector3(0.5f * aspect, -0.5f, 0)
        };
        mesh.triangles = new int[] {
            2, 1, 0, 5, 4, 3
        };
        collider.sharedMesh = mesh;

        previewTexture = GameObject.Find("PreviewCamera").GetComponent<Camera>().targetTexture =
            new RenderTexture((int) w, (int) h, 24, RenderTextureFormat.ARGB32);
        GameObject.Find("PreviewTexture").GetComponent<RawImage>().texture = previewTexture;
        //rt.width = (int)w;
        //rt.height = (int)h;
        RectTransform previewPanel = GameObject.Find("PreviewPanel").GetComponent<RectTransform>();
        if (w > h) {
            previewPanel.sizeDelta = new Vector2(maxw, maxw / aspect);
            previewPanel.anchoredPosition = new Vector2(-maxw / 2 - margin, -maxw / aspect / 2 - margin);
        }
        else {
            previewPanel.sizeDelta = new Vector2(maxh * aspect, maxh);
            previewPanel.anchoredPosition = new Vector2(-maxh * aspect / 2 - margin, -maxh / 2 - margin);
        }
    }
}