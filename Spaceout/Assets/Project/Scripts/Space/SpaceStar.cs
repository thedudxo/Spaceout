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
        //Debug.Log(Manager.instance.player.gameObject.GetComponent<SpaceObject>());
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Manager.instance.player.StopShip();
        }
        

    }

    private void OnTriggerEnter(Collider collider)
    {
        //object dissolves slightly into the star before stopping, so it doesnt seem solid
        SpaceObject collision = collider.gameObject.GetComponent<SpaceObject>();

        if (collision == null) { return; }
        Debug.Log(collision);
        collision.Destroy();
        

    }
}
