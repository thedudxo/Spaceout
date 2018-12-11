using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : SpaceObject {

    public GameObject playerCamera;
    public GameObject background;

    private PlayerParticles playerParticles;
    [SerializeField] private ParticleSystem engineParticles;
    [SerializeField] private ParticleSystem passiveEngineParticles;
    [SerializeField] private ParticleSystem EngineStutterParticles;
    [SerializeField] private ParticleSystem collsionParticles;
    [SerializeField] private ParticleSystem speedParticles;
    [SerializeField] private ParticleSystem rotationParticles;


    private readonly float engineStutterChance = 1.15f;
    private readonly float engineCutOutChance = 0.02f;
    private readonly float engineMaxCutOutTime = 0.4f;
    private readonly float engineMinCutOutTime = 0.1f;
    private float engineCutOutTimer;
    private bool engineCutout = false;
    private float EnginePower = 35;
    private  float rotateSpeed = 3;
    private ScreenshakeManager screenshakeManager = new ScreenshakeManager();

    public bool dead = false;
    private Vector3 spawnPoint;
    [SerializeField] private GameObject deadUI;




    protected override void Start() {
        base.Start();
        EnginePower *= engineStutterChance;
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
        //Cut engine?
        if (Random.Range(0f, 2f)  < engineCutOutChance)
        {
            engineCutout = true;
            EngineStutterParticles.Emit(100);
        }

        if (engineCutout)
        {
            engineCutOutTimer += Time.deltaTime;
            if (engineCutOutTimer > Random.Range(engineMinCutOutTime, engineMaxCutOutTime))
            {
                engineCutout = false;
                engineCutOutTimer = 0;
            }
        } else { 
            // boost the engines!
            if ((int)Random.Range(1, engineStutterChance + 1) == 1)
            {

                rb.AddForce(transform.up * EnginePower * Mathf.Clamp(Input.GetAxis("Vertical"), 0, 1));
                playerParticles.EmitEngineParticles();

                if (Input.GetKey(KeyCode.W)) //add screenshake lasting for one frame
                { screenshakeManager.AddScreenshake(0.15f, playerCamera, 1.0f / (1.0f / Time.deltaTime)); }
            }
        }
        
    }

    protected override void FixedUpdate() {
        base.FixedUpdate();
        if (dead) { return; }

        rb.AddTorque(0, 0, Input.GetAxis("Horizontal") * rotateSpeed);
        rb.AddTorque(0, 0, Random.Range(-Input.GetAxis("Vertical") * 2, Input.GetAxis("Vertical") * 2));
        playerParticles.EmitRotationParticles(rotateSpeed);

        //slow down the rotation!
        rb.AddTorque(0, 0, - (rb.angularVelocity.z / 4));
        

        background.transform.position = new Vector3(transform.position.x, transform.position.y, background.transform.position.z);
    }

    public void KillPlayer()
    {
        dead = true;
        Debug.Log("Player was killed");
        //StopShip();
        deadUI.SetActive(true);
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
    }
}
