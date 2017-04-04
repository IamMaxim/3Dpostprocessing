using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class FileSelectionScript : MonoBehaviour {
    public GameObject photoPrefab, depthPrefab;
    private static string path = Directory.GetCurrentDirectory();
    private ArrayList fileGOs = new ArrayList(),
					depthFileGOs = new ArrayList();
    public GameObject imageListGO, pathGO;
	public GameObject depthListGO, depthPathGO;
    public FileEntry selectedPhoto, selectedDepth;

    public void open() {
		//setup photo selection fist
        pathGO.GetComponent<Text>().text = path;
        foreach (GameObject o in fileGOs) {
            Destroy(o);
        }
        fileGOs.Clear();
        fileGOs.AddRange(getFileObjectsInDir(photoPrefab, imageListGO, path));

		//setup depth selection list
		depthPathGO.GetComponent<Text>().text = path;
		foreach (GameObject o in depthFileGOs) {
			Destroy(o);
		}
		depthFileGOs.Clear();
		depthFileGOs.AddRange(getFileObjectsInDir(depthPrefab, depthListGO, path));
		GetComponent<Animator>().Play("FileSelectionOpen");
    }

    public void close() {
        GetComponent<Animator>().Play("FileSelectionClose");
    }

    private string[] getFilesInDir(string path) {
        return Directory.GetFiles(path);
    }

	private GameObject[] getFileObjectsInDir(GameObject prefab, GameObject parent, string path) {
        string[] files = getFilesInDir(path);
        GameObject[] objects = new GameObject[files.Length];
        int counter = 0;
        foreach (string s in files) {
            GameObject o = GameObject.Instantiate(prefab);
            o.GetComponent<Text>().text = s.Substring(path.Length + 1);
            RectTransform rect = o.GetComponent<RectTransform>();
            rect.position = new Vector3(rect.position.x + 10, -20f - 30f * counter, rect.position.z);
			o.transform.SetParent(parent.transform, false);
            o.GetComponent<FileEntry>().path = s;
            objects[counter] = o;
            counter++;
        }
	    RectTransform rect1 = parent.GetComponent<RectTransform>();
	    rect1.sizeDelta = new Vector2(rect1.sizeDelta.x, counter * 30 + 10);
        return objects;
    }
}
