using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : SpaceObject {

    public GameObject playerCamera;
    public GameObject background;
    public PlayerSound playerSound;

    private PlayerParticles playerParticles;
    
    [SerializeField] readonly private ParticleSystem
        engineParticles,
        passiveEngineParticles,
        EngineStutterParticles,
        collsionParticles,
        speedParticles,
        rotationParticles;

    private float engineCutOutTimer;
    private bool engineCutout = false;

    private float 
        EnginePower = 35,
        rotateSpeed = 4;

    private ScreenshakeManager screenshakeManager = new ScreenshakeManager();

    public bool invincible = false;
    public bool dead = false;
    private Vector3 spawnPoint;
    [SerializeField] private GameObject deadUI;




    protected override void Start() {
        base.Start();
        rotateSpeed *= rb.mass;
        playerParticles = new PlayerParticles(rb, playerCamera, engineParticles, passiveEngineParticles, collsionParticles, speedParticles, EngineStutterParticles, rotationParticles);
        spawnPoint = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        screenshakeManager.AddScreenshake(Mathf.Sqrt(rb.velocity.magnitude) / 20, playerCamera, 0.3f);

        playerParticles.Collision();
        playerCamera.GetComponent<PlayerCamera>().SetCollideTarget();
        
    }


    private void Update()
    {
        if (dead) { return; }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            rotateSpeed *= -1;
        }

        screenshakeManager.Update();
        playerParticles.Update();
        
        
    }

    protected override void FixedUpdate() {
        base.FixedUpdate();
        if (dead) { return; }

        rb.AddTorque(0, 0, Input.GetAxis("Horizontal") * rotateSpeed);
        rb.AddTorque(0, 0, Random.Range(-Input.GetAxis("Vertical") * 2, Input.GetAxis("Vertical") * 2));
        playerParticles.EmitRotationParticles(rotateSpeed);

        //slow down the rotation!
        //rb.AddTorque(0, 0, - (rb.angularVelocity.z / 4f));
        rb.AddTorque(0, 0, - (rb.angularVelocity.z * 40f));

        

        background.transform.position = new Vector3(transform.position.x, transform.position.y, background.transform.position.z);
    }

    override public void Destroy()
    {
        if (invincible) { Debug.Log("player is invincible"); return; }
        PlayerCamera.freezeCamera = true;
        Debug.Log("Player was killed");
        deadUI.SetActive(true);
        dead = true;
    }

    public void StopShip()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    public void RespawnShip()
    {
        transform.position = spawnPoint;
        transform.rotation = Quaternion.identity;
        deadUI.SetActive(false);
        dead = false;
        PlayerCamera.freezeCamera = false;
       playerSound.Respawn();
    }
}
