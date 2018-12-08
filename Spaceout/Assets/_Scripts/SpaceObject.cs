using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceObject : MonoBehaviour {


    static float gConstant = 6.6f;
    static float distanceMultiplyer = 1.4f;
    static float minDistance = 150;

    protected Rigidbody rb;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    protected virtual void FixedUpdate()
    {
        SpaceObject[] gravatators = FindObjectsOfType<SpaceObject>();
        foreach(SpaceObject spaceObject in gravatators)
        {
            if(spaceObject != this)
            {
                Gravatate(spaceObject);
            }
        }
    }

    void Gravatate(SpaceObject objToGravatate)
    {
        Rigidbody rbToGravatate = objToGravatate.rb;

        Vector3 direction = transform.position - rbToGravatate.transform.position;
        float distance = direction.magnitude;

        
        float forceMagnitude = 
            gConstant * (rb.mass * rbToGravatate.mass) / Mathf.Pow(distance, 2) //newton gravity formula thing
            * (distance / minDistance * distanceMultiplyer); // Adjust for distance nonrealisticly

        Vector3 force = direction.normalized * forceMagnitude;

        rbToGravatate.AddForce(force);
    }

    public void SetStableOrbit(SpaceObject orbitAround, float radius)
    {
        Debug.Log("setting orbit");

        // Doesn't make sence to orbit around yourself
        if(orbitAround == this) { Debug.Log(this + " tried to orbit itself"); return; };

        // v = sqrt(G*M/r)
        float orbitalVelocity = Mathf.Sqrt(gConstant * orbitAround.GetRigidBody().mass / radius);


        transform.position = new Vector3(radius, 0, transform.position.z);
        rb.velocity = new Vector3(0, orbitalVelocity, 0);

    }

    public Rigidbody GetRigidBody()
    {
        return GetComponent<Rigidbody>();
    }
}
