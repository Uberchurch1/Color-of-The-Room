using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    public float musicVolume;
    public float fxVolume;
    public AudioClip eatSound;
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

    public AudioClip[] greetings;
    public AudioClip[] farewells;
    public AudioClip[] notEnoughSpores;
    public AudioClip[] boughtItem;
    public AudioClip[] specialItem;
    public AudioClip[] killMyco;
    
    public AudioClip[] youBoughtItem;
    public AudioClip[] youNotEnoughSpores;
    public AudioClip[] youGreetings;
    public AudioClip youFarewell;//mushroom talk question
    public AudioClip[] youSpecialItem;
    public AudioClip[] youKillMyco;
    public AudioClip laughResponse;
    public AudioClip[] damageSounds;
        [FormerlySerializedAs("eatingSounds")] 
    public AudioClip[] youEatingSounds;
    
    
        [FormerlySerializedAs("songs")]
    public AudioSource[] songSources;

    public float songVolume = 0.3f;
    private PlayerHealth _playerHealth;

    private void Start()
    {
        _playerHealth = GetComponentInParent<PlayerHealth>();
        IgnorePause();
    }

    private void Awake()
    {
        IgnorePause();
    }

    public void IgnorePause()
    {
        foreach (var source in songSources)
        {
            source.ignoreListenerPause = true;
        }
    }

    private void Update()
    {
        if (_playerHealth.seeking)
        {
            oneShots.pitch = Time.timeScale;
            foreach (var source in songSources)
            {
                source.pitch = Time.timeScale;
            }
        }
        else
        {
            oneShots.pitch = 1;
            foreach (var source in songSources)
            {
                source.pitch = 1;
            }
        }
    }

    public IEnumerator ShopGreeting()
    {
        ClearAudio();
        //play greetings
        int randInt = Random.Range(0, greetings.Length);
        oneShots.PlayOneShot(greetings[randInt]);
        if (randInt != 6 && randInt != 7 && randInt != 8)
        {
            yield return new WaitForSeconds(greetings[randInt].length);
            //if 0 or 1 play laughResponse (maybe chance)
            if (randInt == 0 || randInt == 1)
            {
                if (Random.Range(0, 3) == 0)
                {
                    oneShots.PlayOneShot(laughResponse);
                }
            }
            //else if not 6, 7 or 8 play youGreetings
            else
            {
                oneShots.PlayOneShot(youGreetings[Random.Range(0,youGreetings.Length)]);
            }
        }
    }
    public IEnumerator ShopExit()
    {
        ClearAudio();
        //play farewells
        int randInt = Random.Range(0, farewells.Length);
        oneShots.PlayOneShot(farewells[randInt]);
        //play youFarewell (random chance)
        if (Random.Range(0, 3) == 0)
        {
            yield return new WaitForSeconds(farewells[randInt].length);
            oneShots.PlayOneShot(youFarewell);
        }
    }
    public IEnumerator ShopNotEnoughSpores()
    {
        ClearAudio();
        if(Random.Range(0, 3) == 0)
        {
            //play youNotEnoughSpores
            int randInt = Random.Range(0, youNotEnoughSpores.Length);
            oneShots.PlayOneShot(youNotEnoughSpores[randInt]);
            //wait clip length
            yield return new WaitForSeconds(youNotEnoughSpores[randInt].length);
        }
        //play notEnoughSpores
        oneShots.PlayOneShot(notEnoughSpores[Random.Range(0,notEnoughSpores.Length)]);
    }
    public IEnumerator ShopBoughtItem()
    {
        ClearAudio();
        if (Random.Range(0,3) == 0)
        {
            //play youBoughtItem
            int randInt = Random.Range(0, youBoughtItem.Length);
            oneShots.PlayOneShot(youBoughtItem[randInt]);
            yield return new WaitForSeconds(youBoughtItem[randInt].length);
        }
        //play boughtItem
        int randInt1 = Random.Range(0, boughtItem.Length);
        oneShots.PlayOneShot(boughtItem[randInt1]);
        //if 0 or 1 play laughResponse (random chance)
        if (randInt1 == 0 || randInt1 == 1)
        {
            if (Random.Range(0, 3) == 0)
            {
                yield return new WaitForSeconds(boughtItem[randInt1].length);
                oneShots.PlayOneShot(laughResponse);
            }
        }
    }
    public IEnumerator ShopSpecialItem()
    {
        ClearAudio();
        //play specialItem
        int randInt = Random.Range(0, specialItem.Length);
        oneShots.PlayOneShot(specialItem[randInt]);
        yield return new WaitForSeconds(specialItem[randInt].length);
        //if 2 or 3 play youSpecialItem
        if (randInt == 2 || randInt == 3)
        {
            oneShots.PlayOneShot(youSpecialItem[Random.Range(0,youSpecialItem.Length)]);
        }
    }

    public IEnumerator ShopBuySpecialItem()
    {
        ClearAudio();
        //play youKillMyco 0 or 1
        int randInt = Random.Range(0,2);
        oneShots.PlayOneShot(youKillMyco[randInt]);
        PlaySong(0);
        yield return new WaitForSeconds(youKillMyco[randInt].length);
        //play killMyco 0 or 1
        int randInt1 = Random.Range(0, killMyco.Length);
        oneShots.PlayOneShot(killMyco[randInt1]);
        yield return new WaitForSeconds(killMyco[randInt1].length);
        //play youKillMyco 2
        oneShots.PlayOneShot(youKillMyco[2]);
        HudManager.Instance.StartCoroutine(HudManager.Instance.CreditsFade());
    }

    public IEnumerator EatSound()
    {
        ClearAudio();
        //play eatSound
        oneShots.PlayOneShot(eatSound);
        yield return new WaitForSeconds(eatSound.length);
        //play youEatingSounds
        int randInt = Random.Range(0, youEatingSounds.Length);
        oneShots.PlayOneShot(youEatingSounds[randInt]);
    }

    public void DamageSound()
    {
        ClearAudio();
        //play damageSounds
        oneShots.PlayOneShot(damageSounds[Random.Range(0,damageSounds.Length)],0.5f);
    }
    
    public IEnumerator StartHeal()
    {
        isHealing = true;
        oneShots.PlayOneShot(capOpen,fxVolume);
        yield return new WaitForSeconds(0.9f);
        StartInhale();
    }
    public IEnumerator StopHeal()
    {
        isHealing = false;
        StopInhale();
        oneShots.PlayOneShot(capClose);
        yield return new WaitForSeconds(0.85f);
        Cough();
    }
    public void Cough()
    {
        //REMOVE:Debug.Log("cough");
        oneShots.PlayOneShot(coughs[Random.Range(0,coughs.Length)]);
    }

    public void StartInhale()
    {
        inhaleLoop.Play();
    }
    
    public void StopInhale()
    {
        inhaleLoop.Stop();
    }

    public bool CheckHealing()
    {
        return isHealing;
    }

    public void ClearAudio()
    {
        //REMOVE:Debug.Log("clear audio");
        StopCoroutine(ShopBoughtItem());
        StopCoroutine(ShopExit());
        StopCoroutine(ShopNotEnoughSpores());
        StopCoroutine(ShopBoughtItem());
        StopCoroutine(ShopSpecialItem());
        StopCoroutine(ShopBuySpecialItem());
        StopCoroutine(EatSound());
        StopCoroutine(StartHeal());
        StopCoroutine(StopHeal());
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
        //REMOVE:Debug.Log("playing song #"+song);
        foreach (var source in songSources)
        {
            if (songSources[song] != source)
            {
                StartCoroutine(FadeSong(source));
            }
        }
        songSources[song].Play();
    }

    public IEnumerator TransitionSong(int song)
    {
        //REMOVE:Debug.Log("transitioning to song #"+song);
        //transition from any song to another
        AudioSource currentSong = null;
        foreach (var source in songSources)
        {
            if (source.isPlaying)
            {
                currentSong = source;
                break;
            }
        }
        //REMOVE:Debug.Log("transitioning from: "+currentSong);
        if (currentSong != null)
        {
            while (currentSong.volume > 0)
            {
                currentSong.volume -= 0.05f;
                songSources[song].volume += 0.05f;
                yield return new WaitForSecondsRealtime(0.2f);
            }
        }
        else
        {
            while (songSources[song].volume < 0.25f)
            {
                songSources[song].volume += 0.05f;
                yield return new WaitForSecondsRealtime(0.2f);
            }
        }
    }

    public IEnumerator FadeSong(AudioSource source)
    {
        while (source.volume > 0)
        {
            source.volume -= 0.15f;
            yield return new WaitForSeconds(0.25f);
        }
        source.Stop();
        source.volume = 0.3f;
    }
}
