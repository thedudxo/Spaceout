using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

    public GameObject star;
    public static Manager instance = null;

	// Use this for initialization
	void Awake () {
        if (instance == null) { instance = this; }
        else if (instance != this) { Destroy(gameObject); }
    }

	// Update is called once per frame
	void Update () {
		
	}

}
