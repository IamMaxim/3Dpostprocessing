using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AddObjectsPanelScript : MonoBehaviour {
	private bool opened = false;
	private Animator animator;
	public Text buttonText;

	void Start () {
		animator = GetComponent<Animator> ();
	}

	public void click() {
		if (!opened) {
			animator.Play ("FileSelectionOpen");
			buttonText.text = "Close";
		} else {
			animator.Play ("FileSelectionClose");
			buttonText.text = "Add objects";
		}
		opened = !opened;
	}
}