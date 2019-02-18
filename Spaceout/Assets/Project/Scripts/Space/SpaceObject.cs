using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceObject : MonoBehaviour {

    public static bool enableAllGravity = true;
    public bool reverseGravity = false; //will repell away objects if true. might only work for centered objects.

    static readonly float gConstant = 10f;
    static readonly float distanceMultiplyer = 210f;

    public static float RandomPosMaxRadius = -200; // += ring radius (done in manager)
    public static float RandomPosMinRadius = +500; // += star radius

    public Rigidbody rb;


    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        
    }

    protected virtual void FixedUpdate()
    {
        if (!enableAllGravity) { return; } //Debug Option

        SpaceObject[] gravatators = FindObjectsOfType<SpaceObject>(); // could be optimised, in manager script
        foreach (SpaceObject spaceObject in gravatators)
        {
            if (spaceObject != this)
            {
                Gravatate(spaceObject);
            }
        }
    }

    void Gravatate(SpaceObject objToGravatate)
    {
        Rigidbody rbToGravatate = objToGravatate.rb;

        //create a vector between spaceobjects
        Vector3 direction = transform.position - rbToGravatate.transform.position;
        float distance = direction.magnitude;

        //Distance needs to be changed to towards the ring, instead of star.
        if (reverseGravity)
        {
            distance = (Manager.instance.ringRadius + 50) - distance;
        }

        //calculate the gravitational force
        float newGConstant = gConstant * (distance / distanceMultiplyer); // Adjust for distance nonrealisticly 
        float forceMagnitude = // f = g*((m1*m2) / d^2) //newton gravity formula thing
            (newGConstant * ((rb.mass * rbToGravatate.mass) / Mathf.Pow(distance, 2)));


        Vector3 force = direction.normalized * forceMagnitude;
        if (reverseGravity) { force *= -1; }

        rbToGravatate.AddForce(force);
    }

    public void SetStableOrbit(SpaceObject orbitAround, int direction = 0)
    {
        // Doesn't make sence to orbit around yourself
        if (orbitAround == this) { Debug.Log(this + " tried to orbit itself"); return; };

        Vector3 distance = orbitAround.transform.position - transform.position;
        float radius = distance.magnitude;


        // v = sqrt(G*M/r)   Formula for velicity in an orbit
        float newGConstant = gConstant * (radius / distanceMultiplyer); // Adjust for distance nonrealisticly 
        float orbitalSpeed = Mathf.Sqrt(newGConstant * orbitAround.GetRigidBody().mass / radius);


        //adjust velocity based on position
        Vector3 tangent = new Vector3(distance.y, -distance.x, 0); //swap the components and negate one to get a tangent
        tangent.Normalize();
        Vector3 orbitalVelocity = tangent * orbitalSpeed;

        //Which way to orbit, randomized if none specified
        while (direction == 0) { direction = Random.Range(-1, 2); }
        if (direction > 0)
        { orbitalVelocity *= -1; }

        //apply chnages
        rb.velocity = orbitalVelocity;

    }

    public void RandomPosition()
    {   //set to a random position within the ring but not too close to the star
        //Does not handle objects spawing within eachother

        Vector2 direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        float magnitude = Random.Range(RandomPosMinRadius, RandomPosMaxRadius);
        Vector3 position = direction * magnitude;

        transform.position = position;

    }

    virtual public void Destroy()
    {
        Debug.Log("problem");
    }

    public Rigidbody GetRigidBody()
    {
        return GetComponent<Rigidbody>();
    }
}
