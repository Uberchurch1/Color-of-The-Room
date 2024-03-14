using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossAttack : MonoBehaviour
{
    public Enemy _enemy;
    public float spAttackSpeed;
    public float atkSpeedVariation;
    public int spAttackDmg;
    
    private float timeToAttack;
    private RoomManager _roomManager;
    private PlayerHealth _playerHealth;

    private void Start()
    {
        _roomManager = FindObjectOfType<RoomManager>();
        _playerHealth = FindObjectOfType<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_enemy.CheckEnemyHealth() <= 2500)
        {
            if (Time.time > timeToAttack)
            {
                StartCoroutine(BossAttackCoroutine());
            }
        }
    }

    private IEnumerator BossAttackCoroutine()
    {
        timeToAttack = Time.time + spAttackSpeed + Random.Range(0,atkSpeedVariation);
        int currentRoom = _roomManager.GetRoomTypeI();
        HudManager.Instance.StartCoroutine(HudManager.Instance.SpAttackCoroutine(currentRoom));
        yield return new WaitForSeconds(10.5f);
        HudManager.Instance.StopCoroutine(HudManager.Instance.SpAttackCoroutine(currentRoom));
        HudManager.Instance.eatBaby.enabled = false;
        if (_roomManager.GetRoomTypeI() == currentRoom)
        {
            _playerHealth.DamagePlayer(spAttackDmg);
        }
    }
}
