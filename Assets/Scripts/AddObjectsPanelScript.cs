using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class AddObjectsPanelScript : MonoBehaviour {
    private bool opened = false;
    private Animator animator;
    public Text buttonText;
    public List<GameObject> objects;
    public GameObject listGO;
    public GameObject listPrefab;
    public GameObject holderPrefab;

    void Start() {
        animator = GetComponent<Animator>();
        RectTransform parentRect = listGO.GetComponent<RectTransform>();

        int x = 10, y = -30, step = -30;
        foreach (GameObject prefab in objects) {
            GameObject o = GameObject.Instantiate(listPrefab);
            o.GetComponent<Text>().text = prefab.name;
            RectTransform rect = o.GetComponent<RectTransform>();
            rect.position = new Vector3(rect.position.x + x, y, rect.position.z);
            y += step;
            o.transform.SetParent(listGO.transform, false);
            o.GetComponent<AddObjectEntryScript>().go = prefab;
        }
        //set list height according to elements
        parentRect.sizeDelta = new Vector2(parentRect.sizeDelta.x, -y - 10);
    }

    //fires when open/close button clicked
    public void click() {
        if (!opened) {
            animator.Play("FileSelectionOpen");
            buttonText.text = "Close";
        } else {
            animator.Play("FileSelectionClose");
            buttonText.text = "Add objects";
        }
        opened = !opened;
    }
}