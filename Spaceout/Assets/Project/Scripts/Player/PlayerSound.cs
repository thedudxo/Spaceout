using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour{

    public Player player;

    public AudioSource
        _engineActive,
        _enginePassive,
        _sideThrusters,
        _engineStutter;

    private Sound 
        engineActive, 
        enginePassive, 
        sideThrusters,
        engineStutter;

    List<Sound> Sounds = new List<Sound>();

    Rigidbody rb;
    readonly float activeVelPitchClamp = 350; //pitch will increase up to this speed
    readonly float activeVelPitchMin = 0.7f;
    readonly float activeVelPitchMax = 2.2f;

    // Start is called before the first frame update
    void Start()
    {
        Sounds.Add(engineActive  = new Sound(_engineActive , 0.3f, true));
        Sounds.Add(enginePassive = new Sound(_enginePassive, 0.1f, true));
        Sounds.Add(sideThrusters = new Sound(_sideThrusters, 0.1f, true));
        Sounds.Add(engineStutter = new Sound(_engineStutter, 1f));
        rb = gameObject.GetComponentInParent<Rigidbody>();
    }


    // Update is called once per frame
    void Update()
    {
        if (player.dead)
        { foreach (Sound sound in Sounds) { sound.Stop(); } }

        foreach (Sound sound in Sounds)
        { sound.Update(); }

        //Change the engine pitch based on speed
        _engineActive.pitch = Mathf.Clamp(
            (Mathf.Clamp(rb.velocity.magnitude, 0, activeVelPitchClamp)
            / activeVelPitchClamp  //between 0 and 1
            * activeVelPitchMax)   //between 0 and pitchmax
            + activeVelPitchMin,   //between pitchmin and pitchmax
            activeVelPitchMin, activeVelPitchMax);  //clamp between min and max

        if (player.dead) { return; }
        if (Input.GetKeyDown(KeyCode.W))
        {
            engineActive.Play();
            enginePassive.Stop();
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            engineActive.Stop();
            enginePassive.Play();
        }

        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            sideThrusters.Play();
        }
        else
        {
            sideThrusters.Stop();
        }
    }

    public void Respawn()
    {
        enginePassive.Play();
    }

    public void Stutter()
    {
        engineStutter.Play();
    }
}
