using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceJunk : SpaceObject {

    public int orbitDirection = 0;

    // Use this for initialization
    new void Start()
    {
        base.Start();
        SetStableOrbit(Manager.instance.star.GetComponent<SpaceObject>() , orbitDirection);
    }

    private void Awake()
    {
        
    }


    private void OnCollisionEnter(Collision collision)
    {
        //Destroy(gameObject);
    }
    /*
    // Update is called once per frame
    void FixedUpdate () {
        base.FixedUpdate();
	}
    */
}
