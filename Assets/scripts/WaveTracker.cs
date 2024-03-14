using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveTracker : MonoBehaviour
{
    private int waveCount;
    public EnemySpawn[] spawners;
    public bool waveOngoing;
    public bool waveRequest;
    public bool waveRepeat = false;
    public float waveDelay = 5f;

    // Start is called before the first frame update
    void Start()
    {
        waveCount = 0;
        waveOngoing = false;
    }
    public void StartWave()
    {
        waveCount++;
        //REMOVE:Debug.Log("starting wave #"+waveCount);
        foreach (var spawner in spawners)
        {
            spawner.StartWave(waveCount);
        }
        //REMOVE:Debug.Log("ongoing = true");
    }

    public IEnumerator EndWave()
    {
        //REMOVE:Debug.Log("ending wave");
        if (waveRepeat && !waveOngoing)
        {
            yield return new WaitForSeconds(2f);
            HudManager.Instance.StartCoroutine(HudManager.Instance.CountdownCoroutine());
            yield return new WaitForSeconds(waveDelay);
            StartWave();
        }
    }
    
    public bool CheckWave()
    {
        return waveOngoing;
    }

    public bool CheckRequest()
    {
        return waveRequest;
    }

    public void ResetWaveCount(int count = 0)
    {
        Debug.Log("reset wave count to "+count);
        waveCount = count;
    }

    public int GetWaveCount()
    {
        return waveCount;
    }

    public void SetOngoing()
    {
        waveOngoing = true;
        waveRequest = false;
    }
}
