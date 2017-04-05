using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddObjectEntryScript : MonoBehaviour {
    public GameObject go;
    public GameObject holderPrefab;

    void Start() {
        holderPrefab = GameObject.Find("AddObjectsPanel").GetComponent<AddObjectsPanelScript>().holderPrefab;
    }

    public void click() {
        GameObject holder = GameObject.Instantiate(holderPrefab);
        GameObject prefabObject = GameObject.Instantiate(go);
        prefabObject.transform.SetParent(holder.transform);
    }
}
