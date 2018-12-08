using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;

public class PlayerCameraBackup : MonoBehaviour {

    public GameObject follow;
    public AnimationCurve collisionCurve;

    private readonly float MinimumZoom = 10;
    private readonly float zoomSpeed = 0.3f;
    private float zoom;

    private readonly float cameraLead = 0.15f;
    public float yShakeAdjust = 0;
    public float xShakeAdjust = 0;

    private readonly float collisionReaction = 10f;
    private readonly float reactionSpeed = 10f;
    private Vector3 collideTarget = new Vector3(0,0,0);

    public void Shake(float x, float y)
    {

        xShakeAdjust = x;
        yShakeAdjust = y;

        //adjust for the camera zoom
        xShakeAdjust *= transform.position.z / 100;
    }
	
	// Update is called once per frame
	void LateUpdate () {

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
        transform.localPosition = new Vector3(follow.transform.position.x + xAdjust + xShakeAdjust + collideAdjust.x,
            follow.transform.position.y + yAdjust + yShakeAdjust + collideAdjust.y,
            -zoom);

        
        //delayed raction on a collision
        if (collideTarget != new Vector3(0, 0, 0))
        {
            

        }
    }

    public void SetCollideTarget()
    {   //delayed raction on a collision
        Vector3 direction = follow.GetComponent<Rigidbody>().velocity.normalized;
        float distance = follow.GetComponent<Rigidbody>().velocity.magnitude;
        collideTarget = direction * distance * collisionReaction;
        Debug.Log(collideTarget);
        collideTarget += transform.position;
        collideTarget.z = 0;
        collideTarget *= 10000;
        //Debug.Log(collideTarget);
        Tween.Position(transform, collideTarget, reactionSpeed, 0f, collisionCurve);
    }
}
