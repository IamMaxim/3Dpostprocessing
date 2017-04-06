using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectEntryScript : MonoBehaviour {
    private Type type;
    private GameObject holder;

    public void init(GameObject holder, Type clazz) {
        this.type = clazz;
        this.holder = holder;
    }

	public void click(bool enable) {
        Debug.Log("type: " + type);
        ((MonoBehaviour) holder.GetComponent(type)).enabled = enable;
    }
}
