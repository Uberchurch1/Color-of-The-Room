using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    public AudioClip[] coughs;

    public AudioClip capOpen;

    public AudioClip capClose;
    public AudioSource oneShots;
    public AudioSource inhaleLoop;
    private bool isHealing = false;

    public IEnumerator StartHeal()
    {
        ClearAudio();
        isHealing = true;
        oneShots.PlayOneShot(capOpen);
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
}
