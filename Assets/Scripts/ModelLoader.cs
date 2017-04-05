using UnityEngine;
using System.Collections;
using System.IO;

public class ModelLoader : MonoBehaviour {
    public class mModel {
        public Mesh mesh;
        public Texture2D tex;

        public mModel(Mesh mesh, Texture2D tex) {
            this.mesh = mesh;
            this.tex = tex;
        }
    }

    public static mModel loadModel(string fileName, string depthFileName) {
        Mesh mesh = new Mesh();
        Texture2D tex = loadTextureFromFile(fileName);
        Texture2D depthTex = null;
        if (depthFileName != null)
            depthTex = loadTextureFromFile(depthFileName);
        tex.filterMode = FilterMode.Point;
        if (tex) {
            Color[] colors = new Color[tex.width * tex.height];
            float[] depths = new float[tex.width * tex.height];
            int[] indices = new int[(tex.width - 1) * (tex.height - 1) * 6];
            Vector3[] vertices = new Vector3[tex.width * tex.height];
            Vector2[] uvs = new Vector2[tex.width * tex.height];
            colors = tex.GetPixels();
            float aspect = (float) tex.width / tex.height;

            if (depthTex) {
                Color[] depthColors = depthTex.GetPixels();
                for (int i = 0; i < depths.Length; i++) depths[i] = depthColors[i].r;
            }
            else for (int i = 0; i < depths.Length; i++) depths[i] = colors[i].r;

            for (int x = 0; x < tex.width; x++)
            for (int y = 0; y < tex.height; y++) {
                vertices[y * tex.width + x] = new Vector3(((float) x / (tex.width - 1) - 0.5f) * aspect,
                    (float) y / (tex.height - 1) - 0.5f, 1 - depths[y * tex.width + x]);
                uvs[y * tex.width + x] = new Vector2((float) x / (tex.width), (float) y / (tex.height));
            }
            for (int y = 1; y < tex.height; y++)
            for (int x = 1; x < tex.width; x++) {
                int b = ((y - 1) * (tex.width - 1) + (x - 1)) * 6;
                indices[b] = getIndex(x - 1, y - 1, tex.width);
                indices[b + 1] = getIndex(x - 1, y, tex.width);
                indices[b + 2] = getIndex(x, y - 1, tex.width);
                indices[b + 3] = getIndex(x, y - 1, tex.width);
                indices[b + 4] = getIndex(x - 1, y, tex.width);
                indices[b + 5] = getIndex(x, y, tex.width);
            }
            mesh.vertices = vertices;
            mesh.triangles = indices;
            mesh.uv = uvs;
        }
        return new mModel(mesh, tex);
    }

    public static Texture2D loadTextureFromFile(string fileName) {
        Texture2D tex = null;
        byte[] fileData;
        if (File.Exists(fileName)) {
            fileData = File.ReadAllBytes(fileName);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData); //this will auto-resize the texture dimensions.
        }
        return tex;
    }

    private static int getIndex(int x, int y, int w) {
        return y * w + x;
    }
}