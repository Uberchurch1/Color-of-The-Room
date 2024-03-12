using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
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
        GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(0.766f);
        Application.Quit();
    }
}
