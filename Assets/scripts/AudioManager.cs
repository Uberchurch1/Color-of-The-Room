using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    public float musicVolume;
    public float fxVolume;
    public AudioClip[] coughs;
    public AudioClip capOpen;
    public AudioClip capClose;
    public AudioSource oneShots;
    public AudioSource inhaleLoop;
    private bool isHealing = false;
    public AudioClip[] meleeHits;//deprecated didnt sound good
    public AudioClip meleeChomp;
    public AudioClip[] meleeAirs;
    
    public AudioClip locked;
    public AudioClip open;

    [FormerlySerializedAs("songs")] public AudioSource[] songSources;

    public IEnumerator StartHeal()
    {
        ClearAudio();
        isHealing = true;
        oneShots.PlayOneShot(capOpen,fxVolume);
        yield return new WaitForSeconds(0.9f);
        StartInhale();
    }
    public IEnumerator StopHeal()
    {
        isHealing = false;
        ClearAudio();
        oneShots.PlayOneShot(capClose);
        yield return new WaitForSeconds(0.85f);
        Cough();
    }
    public void Cough()
    {
        oneShots.PlayOneShot(coughs[Random.Range(0,coughs.Length-1)]);
    }

    public void StartInhale()
    {
        inhaleLoop.Play();
    }

    public bool CheckHealing()
    {
        return isHealing;
    }

    public void ClearAudio()
    {
        oneShots.Stop();
        inhaleLoop.Stop();
    }
    
    public void MeleeHit()
    {
        oneShots.PlayOneShot(meleeChomp);
    }

    public void MeleeAir()
    {
        oneShots.PlayOneShot(meleeAirs[Random.Range(0,coughs.Length-1)]);
    }

    public void DoorLocked()
    {
        oneShots.PlayOneShot(locked);
    }
    
    public void DoorOpen()
    {
        oneShots.PlayOneShot(open);
    }

    public void PlaySong(int song)
    {
        foreach (var source in songSources)
        {
            source.Stop();
        }
        songSources[song].Play();
    }
}
