using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : SpaceObject {

    public GameObject playerCamera;
    public GameObject background;
    public PlayerSound playerSound;
    
    public ParticleSystem
        engineParticles,
        passiveEngineParticles,
        EngineStutterParticles,
        collsionParticles,
        speedParticles,
        rotationParticles;

    [HideInInspector] public PlayerParticles playerParticles;

    public bool invincible = false;
    public bool dead = false;
    private Vector3 spawnPoint;
    [SerializeField] private GameObject deadUI;

    [HideInInspector] public ScreenshakeManager screenshakeManager = new ScreenshakeManager();
    PlayerEngine playerEngine;
    PlayerSideThrusters playerSideThrusters;




    protected override void Start() {
        base.Start();
        
        playerParticles     = new PlayerParticles(rb, playerCamera, engineParticles, passiveEngineParticles, collsionParticles, speedParticles, EngineStutterParticles, rotationParticles);
        playerEngine        = new PlayerEngine(this);
        playerSideThrusters = new PlayerSideThrusters(this);

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

        screenshakeManager.Update();
        playerEngine.Update();
        playerParticles.Update();
        
        
    }

    protected override void FixedUpdate() {
        base.FixedUpdate();
        if (dead) { return; }

        playerSideThrusters.FixedUpdate();

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
