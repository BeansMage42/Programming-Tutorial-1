using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collectible : MonoBehaviour
{
    [SerializeField] private int pointValue;
    [SerializeField] private int addAmmo;
    [SerializeField] private GameManager manager;
     
    
    private void OnTriggerEnter(Collider other)
    {
       // Debug.Log("touched coin");

        
        
        if (other.transform.tag == "Player")
        {
            if (this.name == "Coin")
            {
                manager.AddScore(pointValue);
                Destroy(gameObject);
            }
            else if(this.name == "AmmoBox")
            {
                manager.Ammo(addAmmo);
                Destroy(gameObject);
            }
        }
    }
}
