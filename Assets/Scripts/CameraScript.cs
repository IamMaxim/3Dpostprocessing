using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;

public class CameraScript : MonoBehaviour {
    private float sensitivity = 0.2f, movementSensitivity = 1f;
    private Camera cam;
    public GameObject cursor;
    private Vector2 lastPos = new Vector2(10000000, 10000000), nulled = new Vector2(10000000, 10000000);
    private GameObject obj;
    private MeshFilter filter;
    private ObjectScript objScript;
    private float strength = 0.03f;
    private int size = 5;
    private int mode = 0;
    public static bool enableEdit = false;

    public void setEditingEnabled(bool b) {
        enableEdit = b;
    }

    public void SetMode(bool mode) {
        this.mode = mode ? 1 : 0;
    }

    public void SetStrength(float strength) {
        this.strength = strength;
    }

    public void SetSize(float radius) {
        cursor.transform.localScale = cursor.transform.localScale * radius / (float) size;
        this.size = (int) radius;
    }

    void Start() {
        cam = GetComponent<Camera>();
        obj = GameObject.Find("Object");
        filter = obj.GetComponent<MeshFilter>();
        objScript = obj.GetComponent<ObjectScript>();
    }

    void Update() {
        if (Input.GetMouseButton(1)) {
            transform.Rotate(new Vector3(-Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime * Screen.height,
                Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime * Screen.width,
                -transform.rotation.eulerAngles.z));
        }
        transform.position += Input.GetAxis("Vertical") * transform.TransformDirection(Vector3.forward) *
                              movementSensitivity * Time.deltaTime;
        transform.position += Input.GetAxis("Horizontal") * transform.TransformDirection(Vector3.right) *
                              movementSensitivity * Time.deltaTime;

        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out hit)) return;
        if (!Input.GetMouseButton(0) || mode == 1)
            cursor.transform.position = hit.point;
        if (Input.GetMouseButton(0)) {
            if (enableEdit) {
                Vector2 pos = Input.mousePosition;
                if (mode == 0) {
                    if (lastPos == nulled)
                        lastPos = pos;
                    Vector2 dp = pos - lastPos;
                    float len = Mathf.Sqrt(dp.x * dp.x + dp.y * dp.y);
                    if (len == 0)
                        //no changes
                        return;
                    int horMultiplier = (0.5 - Input.mousePosition.x / Display.main.renderingWidth) > 0 ? 1 : -1,
                        vertMultiplier = (0.5 - Input.mousePosition.y / Display.main.renderingHeight) > 0 ? 1 : -1;
                    float delta = (dp.x / len * horMultiplier + dp.y / len * vertMultiplier) * strength;
                    Vector3[] verts = filter.mesh.vertices;

                    for (float oy = -size; oy <= size; oy++)
                        for (float ox = -size; ox <= size; ox++) {
                            if (Mathf.Sqrt(ox * ox + oy * oy) > size)
                                continue;

                            int x = (int)(objScript.w * 0.5 + cursor.transform.position.x * objScript.w / objScript.aspect + ox);
                            int y = (int)(objScript.h * 0.5 + cursor.transform.position.y * objScript.h + oy);

                            if (x < 0 || x >= objScript.w || y < 0 || y >= objScript.h)
                                continue;

                            int arrPos = (int)(objScript.w * y + x);

                            Vector3 vertice = verts[arrPos];
                            verts[arrPos] = new Vector3(vertice.x, vertice.y, vertice.z + delta);
                        }

                    filter.mesh.vertices = verts;
                    filter.mesh.UploadMeshData(false);
                    lastPos = pos;
                } else if (mode == 1) {
                    Vector3[] verts = filter.mesh.vertices;
                    for (float oy = -size; oy <= size; oy++)
                        for (float ox = -size; ox <= size; ox++) {
                            float range = Mathf.Sqrt(ox * ox + oy * oy);
                            if (range > size)
                                continue;

                            int x = (int)(objScript.w * 0.5 + cursor.transform.position.x * objScript.w / objScript.aspect + ox);
                            int y = (int)(objScript.h * 0.5 + cursor.transform.position.y * objScript.h + oy);

                            if (x < 0 || x >= objScript.w || y < 0 || y >= objScript.h)
                                continue;

                            int arrPos = (int)(objScript.w * y + x);
                            Vector3 vertice = verts[arrPos];
                            verts[arrPos] = new Vector3(vertice.x, vertice.y, vertice.z + strength * (1 - Mathf.Sqrt(range / size)));
                        }
                    filter.mesh.vertices = verts;
                    filter.mesh.UploadMeshData(false);
                }
            }
        } else {
            lastPos = nulled;
        }
    }

    public void invert() {
        Vector3[] verts = filter.mesh.vertices;
        for (int x = 0; x < objScript.w; x++)
        for (int y = 0; y < objScript.h; y++) {
            Vector3 vert = verts[(int) (y * objScript.w + x)];
            verts[(int) (y * objScript.w + x)] = new Vector3(vert.x, vert.y, 1 - vert.z);
        }
        filter.mesh.vertices = verts;
        filter.mesh.UploadMeshData(false);
    }

    public void save() {
        int w = objScript.previewTexture.width,
            h = objScript.previewTexture.height;
        Texture2D tex = new Texture2D(w, h, TextureFormat.ARGB32, false);
        RenderTexture.active = objScript.previewTexture;
        tex.ReadPixels(new Rect(0, 0, w, h), 0, 0);
        RenderTexture.active = null;
        byte[] bytes = tex.EncodeToPNG();
        File.WriteAllBytes("out.png", bytes);
        GameObject.Find("Log").GetComponent<Text>().text = "File saved.";
    }
}