using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHolderScript : MonoBehaviour {
    private Vector3 scanPos, screenPoint, offset;
    private int layer_mask;
    public bool isChild = false;

    private void Start() {
        layer_mask = LayerMask.GetMask("ControlsLayer");
    }

    void OnMouseDown() {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit, 10000, layer_mask);
        if (isChild)
            scanPos = hit.transform.parent.position;
        else
            scanPos = hit.transform.position;

        screenPoint = Camera.main.WorldToScreenPoint(scanPos);
        offset = scanPos - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    void OnMouseDrag() {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        if (isChild)
            transform.parent.position = curPosition;
        else
            transform.position = curPosition;
    }

    public GameObject getObject() {
        if (isChild)
            return transform.parent.gameObject.GetComponent<AddObjectEntryScript>().go;
        else return GetComponent<AddObjectEntryScript>().go;
    }

    public void setScale(float f) {
        foreach (Transform child in transform) {
            if (child.name.Contains("(Clone)")) {
                child.transform.localScale = new Vector3(f, f, f);
            }
        }
    }

    public void setRotationX(float f) {
        foreach (Transform child in transform) {
            if (child.name.Contains("(Clone)")) {
                Vector3 orig = child.transform.rotation.eulerAngles;
                child.transform.rotation = Quaternion.Euler(f, orig.y, orig.z);
            }
        }
    }

    public void setRotationY(float f) {
        foreach (Transform child in transform) {
            if (child.name.Contains("(Clone)")) {
                Vector3 orig = child.transform.rotation.eulerAngles;
                child.transform.rotation = Quaternion.Euler(orig.x, f, orig.z);
            }
        }
    }

    public void setRotationZ(float f) {
        foreach (Transform child in transform) {
            if (child.name.Contains("(Clone)")) {
                Vector3 orig = child.transform.rotation.eulerAngles;
                child.transform.rotation = Quaternion.Euler(orig.x, orig.y, f);
            }
        }
    }
}
