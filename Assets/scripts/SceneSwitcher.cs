using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
        private AudioManager audioMan;
        private PlayerHealth _playerHealth;
        private void Start()
        {
                _playerHealth = FindObjectOfType<PlayerHealth>();
                audioMan = FindObjectOfType<AudioManager>();
                if (CheckCurrentScene("Start"))
                {
                        audioMan.PlaySong(0);
                }
        }

        public void SwitchScene(string scene)
        {
                if (scene == "TheRoom")
                {
                        if (_playerHealth.roundCount == _playerHealth.bossRound)
                        {
                                audioMan.PlaySong(4);
                        }
                        else
                        {
                                audioMan.PlaySong(1);
                        }
                }
                else if (scene == "Shop")
                {
                        if (_playerHealth.roundCount >= _playerHealth.bossRound)
                        {
                                audioMan.PlaySong(5);
                        }
                        else
                        {
                                audioMan.PlaySong(2);
                        }
                }
                else if (scene == "Death")
                {
                        audioMan.PlaySong(0);
                }
                SceneManager.LoadScene(scene);
        }

        public bool CheckCurrentScene(String scene)
        {
                if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName(scene))
                {
                        return true;
                }
                else
                {
                        return false;
                }
        }
}
