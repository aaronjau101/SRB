using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musicManager : MonoBehaviour
{
    AudioSource[] songs;
    int index = 0;
    bool mute = false;

    // Start with first song
    void Start()
    {
        songs = GetComponents<AudioSource>();
        //songs[index].Play();
    }

    // When song ends, Play next song
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            AudioListener.volume = mute ? 1 : 0;
            mute = !mute;
        }
        if (songs[index].time >= songs[index].clip.length)
        {
            songs[index].Stop();
            index = index < songs.Length - 1 ? index + 1 : 0;
            songs[index].Play();
        }
    }
}
