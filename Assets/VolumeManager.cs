using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeManager : MonoBehaviour
{
    public float volume;
    private GameObject[] volumemanagers;
    public AudioSource combat;


    void Awake()
    {
        volumemanagers = GameObject.FindGameObjectsWithTag("VolumeManager");
        if (volumemanagers.Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<AudioSource>().volume = volume;
        combat.volume = volume/2;
    }

    public void playCombat()
    {
        if(!combat.isPlaying)
            combat.Play(0);
    }

    public void stopCombat()
    {
        combat.Stop();
    }
}
