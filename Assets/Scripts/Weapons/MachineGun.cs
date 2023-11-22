using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : WeaponBase
{
    // Start is called before the first frame update

    [SerializeField] private Rigidbody[] projectile;
    [SerializeField] private float force;
    //[SerializeField] private int maxAmmo, clipSize;

    private Player player;

    void Start()
    {
        player = transform.root.gameObject.GetComponent<Player>();
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    protected override  void Attack(float chargePercent)
    {
        if (player.GetCurrentAmmo() > 0)
        {


            Rigidbody spawnedProjectile = Instantiate(projectile[0], transform.position, transform.rotation);

            //Vector3 dir = Vector3.forward + transform.position;
            // Debug.Log(force * Vector3.forward * chargePercent);
            spawnedProjectile.AddForce(force * transform.forward * chargePercent, ForceMode.Impulse);
            spawnedProjectile.GetComponent<Projectile>().SetDamage(10);
            player.ShotFired(1);
        }
    }
}
