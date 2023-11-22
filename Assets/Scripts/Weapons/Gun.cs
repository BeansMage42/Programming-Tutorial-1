using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : WeaponBase
{
    [SerializeField] private Rigidbody[] projectile;
    [SerializeField] private float force;
    private float bulletNum;
    private float offsetX, offsetY, offsetZ;
    [SerializeField] private int maxSpread;
    private Player player;
    [SerializeField] private int maxAmmo, clipSize;
    protected override void Attack(float chargePercent)
    {
        /*int option = 0;
        switch (chargePercent)
        {
            case < 0.2f:
                
                break;
            case < 0.5f:
                option = 1;
                break;
            default:
                option = 2;
                break;


        }

        Rigidbody spawnedProjectile = Instantiate(projectile[option], transform.position, transform.rotation);

        spawnedProjectile.AddForce(force * transform.forward * chargePercent, ForceMode.Impulse);

        spawnedProjectile.GetComponent<Projectile>().SetDamage(chargePercent * 100);



        print("I attacked: " + chargePercent);*/



        
        //shotgun spray
        bulletNum = 15;
        //Debug.Log(bulletNum);
        for(int i = 0; i <= bulletNum; i++)
        {
            offsetX = Random.Range(-maxSpread, maxSpread) *2;
            offsetY = Random.Range(-maxSpread, maxSpread);
            offsetZ = Random.Range(-maxSpread, maxSpread);
            Vector3 spray = transform.forward + new Vector3(offsetX, offsetY, offsetZ);
            spray = spray.normalized;
            
            // Debug.Log("add projectile");
            Rigidbody spawnedProjectile = Instantiate(projectile[1], transform.position, transform.rotation);

            spawnedProjectile.AddForce(force * spray, ForceMode.Impulse);

            spawnedProjectile.GetComponent<Projectile>().SetDamage(20);

        }

        player.ShotFired(5);
        //Debug.Log(spray);

    }

    // Start is called before the first frame update
    void Start()
    {
        player = transform.root.gameObject.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
