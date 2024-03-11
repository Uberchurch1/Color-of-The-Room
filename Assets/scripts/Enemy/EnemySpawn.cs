using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawn : MonoBehaviour
{
    public GameObject[] enemyTypes;
    public GameObject enemyBlank;
    public float spawnRange;
    public float spawnRate = 2f;

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
        //if(waveTracker.CheckRequest()){
        //    //REMOVE:Debug.Log("wave checked true");
        //    StartWave(waveTracker.GetWaveCount());
        //}
    }

    //spawn an enemy with type indexed from array enemyTypes
    private void SpawnEnemy(int enType)
    {
        Vector3 randVec = new Vector3(Random.Range(-spawnRange, spawnRange), 0, Random.Range(-spawnRange, spawnRange));
        Instantiate(enemyTypes[enType], transform.position+randVec, Quaternion.identity);
    }

    //spawn an enemy with type indexed from array enemyTypes
    private void SpawnEnemyBlank()
    {
        Instantiate(enemyBlank, transform.position, Quaternion.identity);
    }

    public void StartWave(int waveCount)
    {
        waveTracker.SetOngoing();
        Debug.Log("starting spawn wave #"+waveCount);//REMOVE:
        enemyManager.AddSpawner(this);//makes spawner active in enemyManager
        //spawns an enemy every spawnRate a waveCount amount of times
        StartCoroutine(waveCoroutine(waveCount));
    }

    private IEnumerator waveCoroutine(int waveCount)
    {
        int total = 0;
        Debug.Log("starting coroutine W: "+waveCount+", R: "+enemyManager.GetRoundCount());//REMOVE:
        for (int i = 0; i <= enemyManager.GetRoundCount(); i++)
        {
            Debug.Log("enemy spawning round: "+i);//REMOVE:
            int enemiesSpawned = 0;
            while (enemiesSpawned < waveCount)
            {
                Debug.Log("spawning enemy #" + enemiesSpawned); //REMOVE:
                //REMOVE:Debug.Log("enemyType length = "+enemyTypes.Length);
                int ranType = Random.Range(0, enemyTypes.Length);
                //REMOVE:Debug.Log("ranType = " + ranType); 
                SpawnEnemy(ranType); //change to random range between 0 and # of enemy types FIXME:
                enemiesSpawned++;

                yield return new WaitForSeconds(spawnRate);
            }

            total += enemiesSpawned;
            if (i != enemyManager.GetRoundCount())
            {
                Debug.Log("waiting for next spawn round: "+ (i+1));//REMOVE:
                yield return new WaitForSeconds(spawnRate*5);
            }
        }
        Debug.Log(("ending spawning, total spawned: "+total));//REMOVE:
        enemyManager.RemoveSpawner(this);//makes spawner unactive in enemyManager
    }
}
