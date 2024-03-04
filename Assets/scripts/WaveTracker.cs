using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveTracker : MonoBehaviour
{
    private int waveCount;
    public bool waveOngoing;
    public bool waveRequest;

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
            EndWave();}//test giving sporesd to the player REMOVE:
        if(Input.GetKeyDown(KeyCode.RightBracket)){
            Debug.Log("start key down");
            StartWave();}//test removing spores from the player REMOVE:
    }

    public void StartWave()
    {
        Debug.Log("starting wave");//REMOVE:
        waveCount++;
        Debug.Log("count++");//REMOVE:
        waveRequest = true;
        Debug.Log("ongoing = true");//REMOVE:
    }

    public void EndWave()
    {
        Debug.Log("ending wave");//REMOVE:
        waveOngoing = false;
        Debug.Log("ongoing = false");//REMOVE:
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
