using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DevOptions : MonoBehaviour {

    public GameObject playerShip;

    private Vector3 spawnPoint;

    private void Start()
    {
        spawnPoint = playerShip.transform.position;
    }

    public void ToggleGravity(bool value)
    {
        SpaceObject.enableAllGravity = value;
        Debug.Log("Gravity Enabled: " + value);
    }

    public void StopShip()
    {
        Rigidbody rb = playerShip.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    public void RespawnShip()
    {
        StopShip();
        playerShip.transform.position = spawnPoint;
        playerShip.transform.rotation = Quaternion.identity;
    }
}
