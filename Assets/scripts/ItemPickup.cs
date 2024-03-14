using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public int amount;

    public bool bigSpore = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")){
            Destroy(gameObject);
            if (!bigSpore)
            {
                other.GetComponent<PlayerHealth>().GiveSpores(amount);
            }
            else
            {
                other.GetComponent<PlayerHealth>().hasBigSpore = true;
            }
        }
    }
}
