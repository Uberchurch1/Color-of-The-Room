using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
        public void SwitchScene(string scene)
        {
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
