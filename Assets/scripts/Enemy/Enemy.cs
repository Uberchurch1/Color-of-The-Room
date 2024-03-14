using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    public float maxHealth = 100f;
    private float enemyHealth;
    public float atkRange = 1.5f;
    public float enemyDmg = 5f;
    public float atkSpeed = 1f;
    public float moveSpeed = 5f;
    public float forceMag = 100f;
    public bool respawnable;
    public GameObject enemyPrefab;
    public GameObject onHitEffect;
    public GameObject onDeathDrop;
    public int maxDropAmt = 4;
    public int minDropAmt = 0;
    public string enemyTypeS;
    
    
    private int enemyTypeI;
    private EnemyAI enemyAI;
    private EnemyAttack _enemyAttack;
    private int dropAmount;
    private RoomManager roomMan;
    private EnemyManager enemyManager;
    private WaveTracker waveTracker;
    private Collider enemyCollider;
    private Rigidbody rigidBody;
    private PlayerHealth player;
    private Transform playerTransform;
    private AudioSource walkSource;
    private SpriteRenderer _spriteRenderer;
    //private Animator spriteAnim; not used

    // Start is called before the first frame update
    void Awake()
    {
        player = FindObjectOfType<PlayerHealth>();
        enemyManager = FindObjectOfType<EnemyManager>();
        waveTracker = FindObjectOfType<WaveTracker>();
        enemyHealth = maxHealth;
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _enemyAttack = GetComponentInChildren<EnemyAttack>();
        if (enemyTypeS != "boss")
        {
            enemyManager.AddLiveEnemy(this); //adds enemy to world list when spawned
            walkSource = GetComponent<AudioSource>();
            rigidBody = GetComponent<Rigidbody>();
            playerTransform = player.transform;
            roomMan = FindObjectOfType<RoomManager>();
            enemyTypeI = Array.IndexOf(roomMan.GetRoomList(), enemyTypeS);
            enemyCollider = GetComponent<CapsuleCollider>();
            //REMOVE:Debug.Log("spawned type: "+enemyTypeI);

            enemyAI = GetComponent<EnemyAI>();
            float speed = (float)(Math.Pow((enemyManager.GetRoundCount() * 2), 2) * 0.05) + moveSpeed;
            enemyAI.ChangeSpeed(Math.Clamp(speed, 5f, 16.5f));
            maxHealth += enemyManager.GetRoundCount() * 15;
        }
        else
        {
            if (!enemyManager.bossList.Contains(this))
            {
                enemyManager.AddBoss(this);
            }
        }
    }

    private void OnEnable()
    {
        player = FindObjectOfType<PlayerHealth>();
        enemyManager = FindObjectOfType<EnemyManager>();
        waveTracker = FindObjectOfType<WaveTracker>();
        enemyHealth = maxHealth;
        if (enemyTypeS == "boss")
        {
            if (!enemyManager.bossList.Contains(this))
            {
                enemyManager.AddBoss(this);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyTypeS != "boss")
        {
            if (IsInRoom())
            {

                _spriteRenderer.color =
                    new Color(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, 1f);
                _spriteRenderer.enabled = true;
                enemyCollider.enabled = true;
                walkSource.enabled = true;
            }
            else
            {
                if (player.seeking)
                {
                    _spriteRenderer.enabled = true;
                    _spriteRenderer.color = new Color(1, 1, 1, 0.5f);
                }
                else
                {
                    _spriteRenderer.enabled = false;
                }

                enemyCollider.enabled = false;
                walkSource.enabled = false;
            }

            if (player.seeking)
            {
                walkSource.pitch = Time.timeScale;
            }
            else
            {
                walkSource.pitch = 1;
            }
        }
    }

    //applies damage to enemies
    public void TakeDamage(float damage, bool melee)
    {
        //spawns blood particles based on amount of damage taken
        for(int i = 0; i <= damage; i += 15){
        Instantiate(onHitEffect, transform.position, transform.rotation);
        }
        enemyHealth -= damage;
        StartCoroutine(DmgFlashCoroutine());
        //checks if enemy has any health left
        if(enemyHealth <= 0)
        {
            Debug.Log(("health zero"));//REMOVE:
            //calculates spore drop amount
            dropAmount = Random.Range(minDropAmt,maxDropAmt);
            //destroys object and removes enemy from list if enemy dies
            enemyManager.RemoveEnemy(this);
            enemyManager.RemoveBoss(this);
            enemyManager.RemoveMeleeEnemy(this);
            enemyManager.RemoveLiveEnemy(this);
            //respawns enemy if allowed
            if(respawnable){
                Respawn();
            }
            //REMOVE:Debug.Log("destroying");
            enemyManager.CheckEndWave(false);
            //drops a random amount of spores 0-5 set from line 18
            if(dropAmount != 0){
                onDeathDrop.GetComponent<ItemPickup>().amount = dropAmount;
                Instantiate(onDeathDrop, transform.position, transform.rotation);
            }
            _enemyAttack.KillEnemy();
            gameObject.SetActive(false);
            enemyManager.DestroyInactive();
            //REMOVE:Debug.Log("destroyed");
        }

        //if (melee)
        //{
        //    Debug.Log("melee true");//REMOVE:
        //    rigidBody.AddForce(-transform.forward * forceMag, ForceMode.Force);
        //}
    }

    private IEnumerator DmgFlashCoroutine()
    {
        _spriteRenderer.color = new Color(1, .3f, .3f);
        yield return new WaitForSeconds(0.2f);
        while (_spriteRenderer.color != Color.white)
        {
            _spriteRenderer.color += new Color(0, .1f, .1f);
            yield return new WaitForSeconds(0.05f);
        }
    }

    //respawn itself MUST BE CALLED BEFORE DESTROY
    private void Respawn()
    {
        Instantiate(enemyPrefab, transform.position, transform.rotation);
    }
    
    public int GetTypeI()
    {
        return enemyTypeI;
    }

    public bool IsInRoom()
    {
        if (roomMan.GetRoomTypeI() == enemyTypeI)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void OnDisable()
    {
        Destroy(gameObject);
    }

    public float CheckEnemyHealth()
    {
        return enemyHealth;
    }
}
