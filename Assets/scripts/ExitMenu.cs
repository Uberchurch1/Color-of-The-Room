using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitMenu : MonoBehaviour
{
    public GameObject main = null;
    public GameObject settings = null;
    
    // Start is called before the first frame update
    void Start()
    {
        if (settings != null)
        {
            settings.SetActive(false);
        }
    }

    private void OnEnable()
    {
        if (settings != null)
        {
            settings.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void QuitGame()
    {
        StartCoroutine(QuitGameCoroutine());
    }
    
    private IEnumerator QuitGameCoroutine()
    {
        //GetComponent<AudioSource>().Play();
        //yield return new WaitForSecondsRealtime(0.766f);
        Application.Quit();
        yield break;
    }

    public void ToggleSettings()
    {
        if (settings.activeSelf)
        {
            main.SetActive(true);
            settings.SetActive(false);
        }
        else
        {
            main.SetActive(false);
            settings.SetActive(true);
        }
    }
}
