using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    public List<Enemy> enemiesInTrigger = new List<Enemy>();//enemies in gun hitbox
    public List<Enemy> enemiesAlive = new List<Enemy>();//enemies currently in world
    private List<Enemy> enemiesInMelee = new List<Enemy>();//enemies in melee hitbox
    private WaveTracker waveTracker;
    private List<EnemySpawn> activeSpawners = new List<EnemySpawn>();
    public HudManager hudMan;
    private PlayerHealth player;
    private TextMeshProUGUI turnAround;
    private int roundCount;
    public Door door;
    
    private void Start() {
        waveTracker = GetComponent<WaveTracker>();
        player = FindObjectOfType<PlayerHealth>();
        StartCoroutine(CheckActiveCoroutine());
        StartCoroutine(StarGameCoroutine());
        turnAround = HudManager.Instance.turnAround;
        roundCount = player.roundCount;
    }

    private void Update() 
    {
        
    }

    //ends wave if there are no enemies alive and no active spawners
    public void CheckEndWave(bool isStart)
    {
        Debug.Log("checking end wave");//REMOVE:
        if(enemiesAlive.Count == 0 && activeSpawners.Count == 0){
            waveTracker.waveOngoing = false;
            Debug.Log("end wave = true #"+waveTracker.GetWaveCount());//REMOVE:
            if (!isStart && ((waveTracker.GetWaveCount() % 5) == 0))
            {
                Debug.Log("ending round #"+roundCount);//REMOVE:
                roundCount += 1;
                player.roundCount += 1;
                StartCoroutine(RoundEndCoroutine());
            }
            else
            {
                Debug.Log("starting next wave");//REMOVE:
                StartCoroutine(waveTracker.EndWave());
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
    
    //add/remove enemies from melee hitbox
    public void AddMeleeEnemy(Enemy enemy)
    {
        enemiesInMelee.Add(enemy);
        hudMan.ShowGrab();
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

    public void MeleeDamge(float damage)
    {
        hudMan.GrabTriggerCoroutine();
        foreach (var enemy in enemiesInMelee)
        {
            enemy.TakeDamage(damage, true);
            player.GiveHealth(1);
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
        Debug.Log("added spawner, count: "+activeSpawners.Count);//REMOVE:
        activeSpawners.Add(spawner);
    }
    
    public void RemoveSpawner(EnemySpawn spawner)
    {
        Debug.Log("removed spawner, count: "+activeSpawners.Count);//REMOVE:
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

    private IEnumerator StarGameCoroutine()
    {
        HudManager.Instance.turnAround.alpha = 0f;
        yield return new WaitForSeconds(1f);
        while (HudManager.Instance.turnAround.alpha < 1)
        {
            HudManager.Instance.turnAround.alpha += 0.2f;
            yield return new WaitForSeconds(0.4f);
        }
        yield return new WaitForSeconds(0.5f);
        HudManager.Instance.turnAround.alpha = 0f;
        CheckEndWave(true);
    }

    private IEnumerator RoundEndCoroutine()
    {
        door.unlocked = true;
        waveTracker.ResetWaveCount();
        Debug.Log("round ending, next round: "+roundCount);//REMOVE:
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
