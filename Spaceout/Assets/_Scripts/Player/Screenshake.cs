using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screenshake{

    float ammount;
    float maxTime;
    float time = 0;
    public bool done = false;
    GameObject camera;
    string tag;

	public Screenshake(float ammount, GameObject camera, float time = Mathf.Infinity, string tag = "null")
    {
        this.ammount = ammount;
        this.maxTime = time;
        this.camera = camera;
        this.tag = tag;
    }

    public void Update()
    {
        time += Time.deltaTime;
        if( time > maxTime) { done = true; }

        if (!done)
        {
            float shakeX = Random.Range(0, ammount);
            float shakeY = Random.Range(0, ammount);

            camera.GetComponent<PlayerCamera>().Shake(shakeX, shakeY);
        }
    }

    public void StopShaking()
    {
        time = maxTime;
        done = true;
    }


}
