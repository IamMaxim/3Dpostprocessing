using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FileEntry : MonoBehaviour {
    public string path;
	public bool isDepth = false;

	public void click() {
        FileSelectionScript scr = GameObject.Find("FileSelectionPanel").GetComponent<FileSelectionScript>();
		if (!isDepth) { //photo file
			if (scr.selectedPhoto)
				scr.selectedPhoto.GetComponent<Text> ().color = Color.white;
			scr.selectedPhoto = this;
		} else { //depth buffer file
			if (scr.selectedDepth)
				scr.selectedDepth.GetComponent<Text> ().color = Color.white;
			scr.selectedDepth = this;
		}
		GetComponent<Text> ().color = Color.yellow;
    }
}
