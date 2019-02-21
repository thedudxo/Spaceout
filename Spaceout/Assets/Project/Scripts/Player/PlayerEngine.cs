using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEngine {
    //Controlls the forward thrust

    Player p;

    private readonly float
        engineStutterChance = 1.15f,
        engineCutOutChance = 0.02f,
        engineMaxCutOutTime = 0.4f,
        engineMinCutOutTime = 0.1f;

    private float
        EnginePower = 35,
        engineCutOutTimer;

    private bool engineCutout = false;


    public PlayerEngine (Player player) {
        this.p = player;
		EnginePower *= engineStutterChance * player.rb.mass;
	}
	
	public void Update () {

		//Cut engine?
        if (Random.Range(0f, 2f)  < engineCutOutChance)
        {
            engineCutout = true;
            p.EngineStutterParticles.Emit(100);
            p.playerSound.Stutter();
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

                p.rb.AddForce(p.transform.up * EnginePower * Mathf.Clamp(Input.GetAxis("Vertical"), 0, 1));
                p.playerParticles.EmitEngineParticles();

                if (Input.GetKey(KeyCode.W)) //add screenshake lasting for one frame
                { p.screenshakeManager.AddScreenshake(0.15f, p.playerCamera, 1.0f / (1.0f / Time.deltaTime)); }
            }
        }
	}
}
