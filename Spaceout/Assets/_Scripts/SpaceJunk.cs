using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceJunk : SpaceObject {

    // Use this for initialization
    void Start()
    {
        base.Start();
        Debug.Log(Manager.instance);
        SetStableOrbit(Manager.instance.star.GetComponent<SpaceObject>(), 1000f);
    }

    private void Awake()
    {
        
    }


    private void OnCollisionEnter(Collision collision)
    {
        //Destroy(gameObject);
    }

    // Update is called once per frame
    void FixedUpdate () {
        base.FixedUpdate();
	}
}
