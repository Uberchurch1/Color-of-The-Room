using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyManager : MonoBehaviour
{

    public List<Enemy> enemiesInTrigger = new List<Enemy>();//enemies in gun hitbox
    public List<Enemy> enemiesAlive = new List<Enemy>();//enemies currently in world
        [FormerlySerializedAs("bossAlive")]
    public List<Enemy> bossList = new List<Enemy>();//boss list
    public Enemy boss;
    private List<Enemy> enemiesInMelee = new List<Enemy>();//enemies in melee hitbox
    private WaveTracker waveTracker;
    private List<EnemySpawn> activeSpawners = new List<EnemySpawn>();
    public HudManager hudMan;
    private PlayerHealth player;
    private TextMeshProUGUI turnAround;
    private int roundCount;
    public int bossRound = 3;
    public Door door;
    
    private void Start() {
        waveTracker = GetComponent<WaveTracker>();
        player = FindObjectOfType<PlayerHealth>();
        roundCount = player.roundCount;
        StartCoroutine(CheckActiveCoroutine());
        StartCoroutine(StartGameCoroutine());
        turnAround = HudManager.Instance.turnAround;
    }

    private void Update() 
    {
        
    }

    //ends wave if there are no enemies alive and no active spawners
    public void CheckEndWave(bool isStart)
    {
        //REMOVE:Debug.Log("checking end wave");
        if(enemiesAlive.Count == 0 && activeSpawners.Count == 0){
            waveTracker.waveOngoing = false;
            //REMOVE:Debug.Log("end wave = true #"+waveTracker.GetWaveCount());
            if (roundCount != bossRound)
            {
                if (roundCount == -1 && waveTracker.GetWaveCount() == 1)
                {
                    //REMOVE:Debug.Log("ending round #"+roundCount);
                    roundCount += 1;
                    player.roundCount += 1;
                    StartCoroutine(RoundEndCoroutine());
                }
                else if (!isStart && ((waveTracker.GetWaveCount() % 4) == 0))
                {
                    //REMOVE:Debug.Log("ending round #"+roundCount);
                    roundCount += 1;
                    player.roundCount += 1;
                    StartCoroutine(RoundEndCoroutine());
                }
                else
                {
                    //REMOVE:Debug.Log("starting next wave");
                    StartCoroutine(waveTracker.EndWave());
                }
            }
            else
            {
                if (isStart)
                {
                    StartCoroutine(waveTracker.EndWave());
                    boss.gameObject.SetActive(true);
                }
                else
                {
                    StartCoroutine(RoundEndCoroutine());
                }
            }
        }
    }

    public int GetRoundCount()
    {
        return roundCount;
    }

    //add/remove enemies from gun hitbox
    public void AddEnemy(Enemy enemy)
    {
        enemiesInTrigger.Add(enemy);
    }
    
    public void RemoveEnemy(Enemy enemy)
    {
        enemiesInTrigger.Remove(enemy);
    }
    public void AddBoss(Enemy enemy)
    {
        bossList.Add(enemy);
    }
    
    public void RemoveBoss(Enemy enemy)
    {
        bossList.Remove(enemy);
    }


    public bool CheckBossAlive()
    {
        if (bossList.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    //add/remove enemies from melee hitbox
    public void AddMeleeEnemy(Enemy enemy)
    {
        enemiesInMelee.Add(enemy);
        hudMan.ShowGrab(true);
    }
    
    public void RemoveMeleeEnemy(Enemy enemy)
    {
        enemiesInMelee.Remove(enemy);
        hudMan.HideGrab();
    }
    
    public int EnemyMeleeCount()
    {
        return enemiesInMelee.Count;
    }

    public void MeleeDamge(float damage, int heal)
    {
        hudMan.GrabTriggerCoroutine();
        foreach (var enemy in enemiesInMelee)
        {
            enemy.TakeDamage(damage, true);
            player.GiveHealth(heal);
        }
    }

    //add/remove enemies from world list
    public void AddLiveEnemy(Enemy enemy)
    {
        enemiesAlive.Add(enemy);
    }
    
    public void RemoveLiveEnemy(Enemy enemy)
    {
        enemiesAlive.Remove(enemy);
    }

    //add/remove enemy spawners
    public void AddSpawner(EnemySpawn spawner)
    {
        //REMOVE:Debug.Log("added spawner, count: "+activeSpawners.Count);
        activeSpawners.Add(spawner);
    }
    
    public void RemoveSpawner(EnemySpawn spawner)
    {
        //REMOVE:Debug.Log("removed spawner, count: "+activeSpawners.Count);
        activeSpawners.Remove(spawner);
    }

    private IEnumerator CheckActiveCoroutine()
    {
        while (true)
        {
            enemiesInTrigger.RemoveAll(enemy => enemy == null);
            

            enemiesInMelee.RemoveAll(enemy => enemy == null);
            

            enemiesAlive.RemoveAll(enemy => enemy == null);
            

            yield return new WaitForSeconds(0.1f);
        }
    }

    public void DestroyInactive()
    {
        foreach (var enemy in enemiesInTrigger)
        {
            if (!enemy.isActiveAndEnabled)
            {
                RemoveEnemy(enemy);
                Destroy(enemy);
            }
        }
        foreach (var enemy in enemiesInMelee)
        {
            if (!enemy.isActiveAndEnabled)
            {
                RemoveMeleeEnemy(enemy);
                Destroy(enemy);
            }
        }
        foreach (var enemy in enemiesAlive)
        {
            if (!enemy.isActiveAndEnabled)
            {
                RemoveLiveEnemy(enemy);
                Destroy(enemy);
            }
        }
    }

    private IEnumerator StartGameCoroutine()
    {
        if (roundCount == -2)
        {
            HudManager.Instance.turnAround.alpha = 0f;
            yield return new WaitForSeconds(0.25f);
            while (HudManager.Instance.turnAround.alpha < 1)
            {
                HudManager.Instance.turnAround.alpha += 0.2f;
                yield return new WaitForSeconds(0.4f);
            }

            yield return new WaitForSeconds(0.5f);
        }
        HudManager.Instance.turnAround.alpha = 0f;
        CheckEndWave(true);
    }

    private IEnumerator RoundEndCoroutine()
    {
        door.unlocked = true;
        waveTracker.ResetWaveCount();
        //REMOVE:Debug.Log("round ending, next round: "+roundCount);
        turnAround.text = "YOU";
        turnAround.alpha = 1f;
        yield return new WaitForSeconds(1f);
        turnAround.text = "MAY";
        yield return new WaitForSeconds(1f);
        turnAround.text = "LEAVE";
        yield return new WaitForSeconds(1f);
        turnAround.text = "NOW!";
        yield return new WaitForSeconds(1f);
        while (turnAround.alpha > 0)
        {
            turnAround.alpha -= 0.2f;
            yield return new WaitForSeconds(0.2f);
        }
        turnAround.text = "TURN AROUND";
    }
}
