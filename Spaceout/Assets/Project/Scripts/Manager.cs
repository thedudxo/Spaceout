using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

    public GameObject star;
    public Player player;
    public float ringRadius;
    public float starRadius;
    public static Manager instance = null;


    // Use this for initialization
    void Awake () {
        if (instance == null) { instance = this; }
        else if (instance != this) { Destroy(gameObject); }

        //Min-Max radius for random spaceobject spawning. 
        SpaceObject.RandomPosMaxRadius += ringRadius;
        SpaceObject.RandomPosMinRadius += starRadius;
    }


    // Update is called once per frame
    void Update () {
		
	}

}
