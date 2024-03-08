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
    public float waveDelay = 4f;

    // Start is called before the first frame update
    void Start()
    {
        waveCount = 0;
        waveOngoing = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftBracket)){
            Debug.Log("end key down");
            StartCoroutine(EndWave());}//test giving sporesd to the player REMOVE:
        if(Input.GetKeyDown(KeyCode.RightBracket)){
            Debug.Log("start key down");
            StartWave();}//test removing spores from the player REMOVE:
    }

    public void StartWave()
    {
        Debug.Log("starting wave");//REMOVE:
        waveCount++;
        Debug.Log("count++");//REMOVE:
        foreach (var spawner in spawners)
        {
            spawner.StartWave(waveCount);
        }
        Debug.Log("ongoing = true");//REMOVE:
    }

    public IEnumerator EndWave()
    {
        Debug.Log("ending wave");//REMOVE:
        if (waveRepeat && !waveOngoing)
        {
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
