using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceObject : MonoBehaviour {

    public static bool enableAllGravity = true;
    static readonly float gConstant = 10f; // this used to be 6.6
    static readonly float distanceMultiplyer = 210f; //1.4 * 150
    //static readonly float minDistance = 150;

    protected Rigidbody rb;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    protected virtual void FixedUpdate()
    {
        if (!enableAllGravity) { return; } //Debug Option

        SpaceObject[] gravatators = FindObjectsOfType<SpaceObject>(); // could be optimised, in manager script
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

        float newGConstant = gConstant * (distance / distanceMultiplyer); // Adjust for distance nonrealisticly 
        float forceMagnitude = // f = g*((m1*m2) / d^2)
            (newGConstant * ((rb.mass * rbToGravatate.mass) / Mathf.Pow(distance, 2))); //newton gravity formula thing
            

        Vector3 force = direction.normalized * forceMagnitude;

        rbToGravatate.AddForce(force);
    }

    public void SetStableOrbit(SpaceObject orbitAround)
    {
        Debug.Log("setting orbit");
        Vector3 distance = orbitAround.transform.position - transform.position;
        float radius = distance.magnitude;

        // Doesn't make sence to orbit around yourself
        if (orbitAround == this) { Debug.Log(this + " tried to orbit itself"); return; };

        // v = sqrt(G*M/r)
        float newGConstant = gConstant * (radius / distanceMultiplyer); // Adjust for distance nonrealisticly 
        float orbitalVelocity = Mathf.Sqrt(newGConstant * orbitAround.GetRigidBody().mass / radius);
            

        //transform.position = new Vector3(radius, 0, transform.position.z);
        rb.velocity = new Vector3(0, orbitalVelocity, 0);

    }

    public Rigidbody GetRigidBody()
    {
        return GetComponent<Rigidbody>();
    }
}
