using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSideThrusters {
    //controlls the rotation thrust

    Player p;

    float rotateSpeed = 4;

    public PlayerSideThrusters(Player player)
    {
        p = player;
        rotateSpeed *= p.rb.mass;
    }
	

	public void FixedUpdate () {

        p.rb.AddTorque(0, 0, Input.GetAxis("Horizontal") * rotateSpeed);
        p.rb.AddTorque(0, 0, Random.Range(-Input.GetAxis("Vertical") * 2, Input.GetAxis("Vertical") * 2));
        p.playerParticles.EmitRotationParticles(rotateSpeed);

        //slow down the rotation!
        p.rb.AddTorque(0, 0, -(p.rb.angularVelocity.z * 40f));
    }
}
