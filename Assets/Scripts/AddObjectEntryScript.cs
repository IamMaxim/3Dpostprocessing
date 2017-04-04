using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddObjectEntryScript : MonoBehaviour {
    public GameObject go;

    public void click() {
        GameObject.Instantiate(go);
    }
}
