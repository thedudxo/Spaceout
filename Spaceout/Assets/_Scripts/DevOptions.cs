using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DevOptions : MonoBehaviour {


    [SerializeField] GameObject playerShip;
    [SerializeField] PlayerCamera cameraScript;
    [SerializeField] Slider zoomOveride;

    Player player;

    private void Start()
    {
        player = playerShip.GetComponent<Player>();
    }

    public void ToggleGravity(bool value)
    {
        SpaceObject.enableAllGravity = value;
        Debug.Log("Gravity Enabled: " + value);
    }

    public void setPlayerInvincible(bool value)
    {
        player.invincible = value;
    }

    public void StopShip()
    {
        player.StopShip();
    }

    public void RespawnShip()
    {
        player.StopShip();
        player.RespawnShip();
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
