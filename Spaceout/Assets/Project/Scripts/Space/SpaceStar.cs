using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceStar : SpaceObject {

    public bool dangerousStar = true;
    public float rotationSpeed;

    new private void FixedUpdate()
    {
        base.FixedUpdate();
        transform.Rotate(new Vector3( 0, 0, rotationSpeed));
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Manager.instance.player.StopShip();
        }
        

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") { 
            Manager.instance.player.KillPlayer();
            //player dissolves slightly into the star before stopping, so it doesnt seem solid
        }
    }
}
