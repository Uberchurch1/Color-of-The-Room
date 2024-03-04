using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject[] enemyTypes;
    public GameObject enemyBlank;
    public float spawnRange;
    public float spawnRate = 1f;

    private WaveTracker waveTracker;
    private EnemyManager enemyManager;

    // Start is called before the first frame update
    void Start()
    {
        enemyManager = FindObjectOfType<EnemyManager>();
        waveTracker = enemyManager.GetComponent<WaveTracker>();
    }

    // Update is called once per frame
    void Update()
    {
        if(waveTracker.CheckRequest()){
            Debug.Log("wave checked true");//REMOVE:
            StartWave(waveTracker.GetWaveCount());
        }
    }

    //spawn an enemy with type indexed from array enemyTypes
    private void SpawnEnemy(int enType)
    {
        Instantiate(enemyTypes[enType], transform.position, Quaternion.identity);
    }

    //spawn an enemy with type indexed from array enemyTypes
    private void SpawnEnemyBlank()
    {
        Instantiate(enemyBlank, transform.position, Quaternion.identity);
    }

    private void StartWave(int waveCount)
    {
        waveTracker.SetOngoing();
        Debug.Log("starting spawn wave #"+waveCount);//REMOVE:
        enemyManager.AddSpawner(this);//makes spawner active in enemyManager
        //spawns an enemy every spawnRate a waveCount amount of times
        StartCoroutine(waveCoroutine(waveCount));
        enemyManager.RemoveSpawner(this);//makes spawner unactive in enemyManager
    }

    private IEnumerator waveCoroutine(int waveCount)
    {
        int enemiesSpawned = 0;
        while(enemiesSpawned < waveCount){
            Debug.Log("spawning enemy #"+enemiesSpawned);//REMOVE:
            SpawnEnemy(0);//change to random range between 0 and # of enemy types FIXME:
            enemiesSpawned++;
            
            yield return new WaitForSeconds(spawnRate);
        }
        
    }
}
