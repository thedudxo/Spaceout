using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DevOptions : MonoBehaviour {


    [SerializeField] GameObject playerShip;
    [SerializeField] PlayerCamera cameraScript;
    [SerializeField] Slider zoomOveride;

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

    public void CameraZoom()
    {
        cameraScript.AddZoom(zoomOveride.value);
    }

    private void Update()
    {
        zoomOveride.value += Input.mouseScrollDelta.y * 10;
    }
}
