using UnityEngine;
using UnityEngine.UI;

public class SettingsPanelScript : MonoBehaviour {
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
			buttonText.text = "Brush settings";
		}
		opened = !opened;
	}
}