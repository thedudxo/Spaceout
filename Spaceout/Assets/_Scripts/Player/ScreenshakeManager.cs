using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenshakeManager{

    private List<Screenshake> Screenshakes;

    public ScreenshakeManager()
    {
        Screenshakes = new List<Screenshake>();
    }

    public void Update()
    {
        for ( int i = 0; i < Screenshakes.Count; i++)
        {
            Screenshakes[i].Update();
            if (Screenshakes[i].done)
            {
                Screenshakes.Remove(Screenshakes[i]);
            }
        }
    }

    public void AddScreenshake(float ammount, GameObject camera, float time = Mathf.Infinity, string tag = null)
    {
        Screenshakes.Add(new Screenshake(ammount, camera, time, tag));
    }
}
