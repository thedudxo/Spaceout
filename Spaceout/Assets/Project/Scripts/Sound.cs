using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound{

    public AudioSource Audio;

    readonly float releaseTime;
    float releasing = 0;
    bool  playing;
    bool randomStartTime = true;

    public Sound(AudioSource Audio, float releaseTime, bool randomStartTime = false)
    {
        this.Audio = Audio;
        this.releaseTime = releaseTime;
        this.randomStartTime = randomStartTime;
        playing = Audio.isPlaying;
    }

    public void Update()
    {
        if (releasing > 0)
        {
            releasing -= Time.deltaTime;
            Audio.volume = releasing / releaseTime; //normalise between 0-1
        }
    }
    public void Play()
    {
        if (randomStartTime) { Audio.time = Random.Range(0, Audio.clip.length); }
        Audio.Play();
        Audio.volume = 1;
        playing = true;
        releasing = 0;
    }

    public void Stop()
    {
        if(!playing) { return; } //Allready stopping sound
        playing = false;
        releasing = releaseTime;
    }

}
