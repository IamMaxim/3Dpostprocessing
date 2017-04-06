using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

public class EffectsPanelScript : MonoBehaviour {
    private bool opened = false;
    private Animator animator;
    public Text buttonText;
    public GameObject effectHolder;

    public GameObject o_ssao, o_blur, o_bloom;

    void Start() {
        animator = GetComponent<Animator>();

        Bloom bloom = effectHolder.GetComponent<Bloom>();
        ScreenSpaceAmbientOcclusion ssao = effectHolder.GetComponent<ScreenSpaceAmbientOcclusion>();
        Blur blur = effectHolder.GetComponent<Blur>();

        bloom.enabled = false;
        ssao.enabled = false;
        blur.enabled = false;

        o_bloom.GetComponent<EffectEntryScript>().init(effectHolder, typeof(Bloom));
        o_blur.GetComponent<EffectEntryScript>().init(effectHolder, typeof(Blur));
        o_ssao.GetComponent<EffectEntryScript>().init(effectHolder, typeof(ScreenSpaceAmbientOcclusion));
    }

    public void click() {
        if (!opened) {
            animator.Play("FileSelectionOpen");
            buttonText.text = "Close";
        } else {
            animator.Play("FileSelectionClose");
            buttonText.text = "Effects";
        }
        opened = !opened;
    }
}
