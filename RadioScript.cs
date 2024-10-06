using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioScript : MonoBehaviour
{
    private GameObject playerObj;
    private playerScript playerControl;

    private GameObject vanObj;
    private vanScript vanControl;

    public AudioSource Radio;
    public AudioClip[] RadioChannels;
    public int CurrentChannel;
    public int MaxChannels;
    public bool playing = false;

    private GameObject soundManager;
    private AudioSource SMAudio;

    // Start is called before the first frame update
    void Start()
    {
        MaxChannels = RadioChannels.Length;

        playerObj = GameObject.FindWithTag("Player");
        playerControl = playerObj.GetComponent<playerScript>();

        vanObj = GameObject.FindWithTag("van");
        vanControl = vanObj.GetComponent<vanScript>();

        CurrentChannel = 0;

        Radio = this.GetComponent<AudioSource>();
        Radio.Play();

        soundManager = GameObject.FindWithTag("sound manager");
        SMAudio = soundManager.GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentChannel < MaxChannels)
        {
            Radio.clip = RadioChannels[CurrentChannel];
        } else
        {
            Radio.Stop();
            playing = false;
        }

        //Debug.Log(Radio.isPlaying);

        if(Radio.isPlaying == false && CurrentChannel != MaxChannels)
        {
            Radio.Play();
            playing = true;
        }


        if (playerControl.driving == false && vanControl.vanMode)
        {
            if (Mathf.Abs(playerControl.transform.position.x - this.transform.position.x) <= 0.5f && Input.GetKeyUp(KeyCode.Space) && playerControl.carrying == false)
            {
                SMAudio.Play();

                if (CurrentChannel <= MaxChannels)
                {
                    CurrentChannel += 1;
                }

                if (CurrentChannel > MaxChannels)
                {
                    CurrentChannel = 0;
                }

            }

        }

        

    }
}
