using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEngine : MonoBehaviour {

    Player player;

    private readonly float 
        engineStutterChance = 1.15f,
        engineCutOutChance = 0.02f,
        engineMaxCutOutTime = 0.4f,
        engineMinCutOutTime = 0.1f;
	// Use this for initialization

	void Start (Player player) {
        this.player = player;
		player.EnginePower *= engineStutterChance * player.rb.mass;
	}
	
	// Update is called once per frame
	void Update () {
		//Cut engine?
        if (Random.Range(0f, 2f)  < engineCutOutChance)
        {
            engineCutout = true;
            EngineStutterParticles.Emit(100);
            playerSound.Stutter();
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

                player.rb.AddForce(transform.up * EnginePower * Mathf.Clamp(Input.GetAxis("Vertical"), 0, 1));
                playerParticles.EmitEngineParticles();

                if (Input.GetKey(KeyCode.W)) //add screenshake lasting for one frame
                { screenshakeManager.AddScreenshake(0.15f, playerCamera, 1.0f / (1.0f / Time.deltaTime)); }
            }
        }
	}
}
