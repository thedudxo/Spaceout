using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;

public class PlayerCamera : MonoBehaviour {

    [SerializeField] GameObject follow;
    [SerializeField] AnimationCurve collisionCurve;

    private float zoom;
    private readonly float MinimumZoom = 10;
    private readonly float targetZoomScale = 0.3f;
    
    private readonly float cameraLead = 0.15f;

    private float yShakeAdjust = 0;
    private float xShakeAdjust = 0;

    private float starBoundary = 3; //star radius is added in Start()

    //colisions (doesnt work atm)
    [SerializeField] private GameObject CollisionTween;
    private readonly float CollisionCameraModifier = 10f;
    private readonly float reactionSpeed = 10f;

    public static bool freezeCamera = false;
    private float zoomAdded; //for debug camera overide


    private void Start()
    {
        starBoundary += Manager.instance.starRadius;
    }

    // Update is called once per frame
    void LateUpdate () {
        if (freezeCamera) {
            return;
        }

        Vector3 Velocity = follow.GetComponent<Player>().GetRigidBody().velocity;


        //Set target zoom based on speed
        float targetZoom = MinimumZoom + (Velocity.magnitude * targetZoomScale);
        if (targetZoom < MinimumZoom) { targetZoom = MinimumZoom; }

        //Fade to target zoom
        if (zoom > targetZoom)
        {
            zoom--;
            if(zoom < targetZoom) { zoom = targetZoom;}
        }
        else if (zoom < targetZoom)
        {
            zoom++;
            if(zoom > targetZoom) { zoom = targetZoom;}
        }

        //Adjust camera lead
        float yAdjust = 0;
        float xAdjust = 0;

        xAdjust = Velocity.x * cameraLead;
        yAdjust = Velocity.y * cameraLead;


        Vector3 collideAdjust = new Vector3(0, 0, 0);

        //Apply changes
        Vector3 newPosition = new Vector3(
            follow.transform.position.x + xAdjust + xShakeAdjust + collideAdjust.x,
            follow.transform.position.y + yAdjust + yShakeAdjust + collideAdjust.y,
            -zoom -zoomAdded
            );


        //Stop camera getting close to the star
        if( newPosition.magnitude < starBoundary)
        {
            newPosition = newPosition.normalized * starBoundary;
        }


        //update position
        transform.position = newPosition;
    }



    public void SetCollideTarget() //currently doesn't work
    {   //delayed raction on a collision
        if (freezeCamera) { return; }
        Vector3 direction = follow.GetComponent<Rigidbody>().velocity.normalized;
        float distance = follow.GetComponent<Rigidbody>().velocity.magnitude;
        Vector3 collideTarget = direction * distance * CollisionCameraModifier;
        collideTarget.z = 0;
        //Debug.Log(collideTarget);
        collideTarget += transform.position;
        //Debug.Log(collideTarget);
        CollisionTween.GetComponent<CameraCollision>().Collide(collideTarget, reactionSpeed, collisionCurve);
    }

    public void AddZoom(float ammount)
    {
        zoomAdded = ammount;
    }

    public void Shake(float x, float y)
    {

        xShakeAdjust = x;
        yShakeAdjust = y;

        //adjust for the camera zoom
        xShakeAdjust *= transform.position.z / 100;
    }
}
