using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticles{

    private ParticleSystem engineParticles;
    private ParticleSystem passiveEngineParticles;
    private ParticleSystem collsionParticles;
    private ParticleSystem speedParticles;
    private ParticleSystem EngineStutterParticles;
    private ParticleSystem rotationParticles;

    private Rigidbody rb;
    private GameObject playerCamera;

    private float speedParticleMultiplyer = 0.04f;
    private float minParticleAlpha = 0.1f;
    private float maxParticleAlpha = 0.9f;
    private float maxVelEffectAlpha = 150f;



   public  PlayerParticles(Rigidbody rb, GameObject playerCamera,
       ParticleSystem engineParticles, ParticleSystem passiveEngineParticles, ParticleSystem collsionParticles, ParticleSystem speedParticles,
       ParticleSystem EngineStutterParticles, ParticleSystem rotationParticles)
    {
        this.rb = rb;
        this.playerCamera = playerCamera;

        this.engineParticles = engineParticles;
        this.passiveEngineParticles = passiveEngineParticles;
        this.collsionParticles = collsionParticles;
        this.speedParticles = speedParticles;
        this.rotationParticles = rotationParticles;

        rb.AddForce(new Vector3(1f, 0, 0)); //this fixes random bug where speed particles arn't rotated right
    }
	
    public void EmitEngineParticles()
    {
        engineParticles.Emit((int)(Input.GetAxis("Vertical") * 10));
    }

    public void EmitRotationParticles(float rotateSpeed)
    {
        int invert = 1;
        if (rotateSpeed > 0) { invert = -1; }
        var shape = rotationParticles.shape;

        if(Input.GetAxis("Horizontal") * invert > 0)
        {
            shape.rotation = new Vector3(180,0,0);
            rotationParticles.Emit((int)(Input.GetAxis("Horizontal") + 2 * 10 / 2));
        } else if (Input.GetAxis("Horizontal") * invert < 0)
        {
            shape.rotation = new Vector3(0, 0, 0);
            rotationParticles.Emit((int)(Input.GetAxis("Horizontal") * 10));
        }
    }

	public void Update () {
        //Emit Particles

        
        passiveEngineParticles.Emit(((int)Input.GetAxis("Vertical") * 10) + 10);
        speedParticles.Emit((int)(rb.velocity.magnitude * speedParticleMultiplyer));

        ////rotate speed particles
        var shape = speedParticles.shape;
        shape.rotation = (Quaternion.LookRotation(rb.velocity).eulerAngles);

        //Colour speed Particles
        var colour = speedParticles.colorOverLifetime;
        Gradient grad = new Gradient(); grad.SetKeys(
           new GradientColorKey[] { new GradientColorKey(new Color(180f / 255f, 0f, 180f / 255f), 0f) },
           new GradientAlphaKey[] {
                new GradientAlphaKey(Mathf.Clamp (rb.velocity.magnitude / maxVelEffectAlpha , minParticleAlpha, maxParticleAlpha) , 0.9f),
                new GradientAlphaKey(Mathf.Clamp (rb.velocity.magnitude / maxVelEffectAlpha , minParticleAlpha, maxParticleAlpha) , 0.01f),
                new GradientAlphaKey(0f,0f) , new GradientAlphaKey(0f, 1f) }
           );

        colour.color = grad;

        //Set Speed Particle speed
        var speed = speedParticles.main;
        speed.startSpeed = rb.velocity.magnitude;

        //Set Speed Particle Position
        speedParticles.transform.position = new Vector3(
           playerCamera.transform.position.x,
           playerCamera.transform.position.y,
           10
           );

        //Scale speed particle based on camera distance
        float modifier = -playerCamera.transform.position.z * 1.5f;
        shape.scale = new Vector3(
            7,
            50 + modifier,
            50 + modifier
           );

    }

    public void Collision()
    {
        var particles = collsionParticles.main;
        particles.startLifetime = (int)(rb.velocity.magnitude);
        collsionParticles.Emit((int)(rb.velocity.magnitude * 5f));
    }
}
