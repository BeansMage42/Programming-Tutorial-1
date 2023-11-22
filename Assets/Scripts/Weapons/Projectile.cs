using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    
    private float projectileDamage;
    private float lifeTime = 5;

    private static Collider[] maxCollisions = new Collider[15];



    public void SetDamage(float damage)
    {
        projectileDamage = damage;
    }
    private void Update()
    {
        lifeTime -= Time.deltaTime;
        if(lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if(collision.transform.root.TryGetComponent(out IDamagable hitObject))
        {
            Debug.Log("hit");
            hitObject.TakeDamage(projectileDamage);
            Destroy(gameObject);
        }
    }

  /*  private void OnDestroy()
    {
       int n = Physics.OverlapSphereNonAlloc(transform.position, 50, maxCollisions, GameManager.enemyLayers);

        print(n);
        for (int i = 0; 1 < n; i++)
        {
            
            Collider c = maxCollisions[i];
            print(c.name);
            c.GetComponent<Enemy>().ForceTarget(transform.position);
        }

        *//*foreach(Collider c in maxCollisions)
        {
            c.GetComponent<Enemy>().ForceTarget(transform.position);
        }*//*
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 50);
    }*/
}
