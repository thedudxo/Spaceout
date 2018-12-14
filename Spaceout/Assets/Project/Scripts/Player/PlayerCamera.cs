using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;

public class PlayerCamera : MonoBehaviour {

    public GameObject follow;
    public AnimationCurve collisionCurve;


    private readonly float MinimumZoom = 10;
    private readonly float zoomSpeed = 0.3f;
    private float zoom;
    private float zoomAdded; //for debug camera overide

    private readonly float cameraLead = 0.15f;
    public float yShakeAdjust = 0;
    public float xShakeAdjust = 0;

    [SerializeField]
    private GameObject CollisionTween;
    private readonly float CollisionCameraModifier = 10f;
    private readonly float reactionSpeed = 10f;

    public static bool freezeCamera = false;

    public void Shake(float x, float y)
    {

        xShakeAdjust = x;
        yShakeAdjust = y;

        //adjust for the camera zoom
        xShakeAdjust *= transform.position.z / 100;
    }
	
	// Update is called once per frame
	void LateUpdate () {
        if (freezeCamera) {
            Debug.Log("camera froze");
            return; }
        //Zoom the camera
        Vector3 Velocity = follow.GetComponent<Player>().GetRigidBody().velocity;
        float totalVelocity = Mathf.Sqrt(Mathf.Pow(Velocity.x,2)  + Mathf.Pow(Velocity.y,2));

        float targetZoom = MinimumZoom + (totalVelocity * zoomSpeed);
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
        transform.position = new Vector3(follow.transform.position.x + xAdjust + xShakeAdjust + collideAdjust.x,
            follow.transform.position.y + yAdjust + yShakeAdjust + collideAdjust.y,
            -zoom -zoomAdded);

    }



    public void SetCollideTarget()
    {   //delayed raction on a collision
        if (freezeCamera) { return; }
        Vector3 direction = follow.GetComponent<Rigidbody>().velocity.normalized;
        float distance = follow.GetComponent<Rigidbody>().velocity.magnitude;
        Vector3 collideTarget = direction * distance * CollisionCameraModifier;
        collideTarget.z = 0;
        Debug.Log(collideTarget);
        collideTarget += transform.position;
        //Debug.Log(collideTarget);
        CollisionTween.GetComponent<CameraCollision>().Collide(collideTarget, reactionSpeed, collisionCurve);
    }

    public void AddZoom(float ammount)
    {
        zoomAdded = ammount;
    }
}
