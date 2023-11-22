using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper : WeaponBase 
{
    [SerializeField] private Rigidbody[] projectile;
    [SerializeField] private float force;
    private Player player;
    [SerializeField] private Camera cam;
    public float defaultFov = 60;
    private bool zoomed = false;

    private void Start()
    {
        
        player = transform.root.gameObject.GetComponent<Player>();
    }
    protected override void Attack (float chargePercent)
    {

        Rigidbody spawnedProjectile = Instantiate(projectile[0], transform.position, transform.rotation);

        //Vector3 dir = Vector3.forward + transform.position;
        // Debug.Log(force * Vector3.forward * chargePercent);
        spawnedProjectile.AddForce(force * transform.forward * chargePercent, ForceMode.Impulse);
        spawnedProjectile.GetComponent<Projectile>().SetDamage(100);
        player.ShotFired(1);
    }

    public void Zoom()
    {
        Debug.Log("sniper zoom FUNC");
        if (!zoomed)
        {
            Debug.Log("Zooomed");
            zoomed = !zoomed;

            cam.fieldOfView = (defaultFov / 3);
        }
        else
        {
            Debug.Log("Unzoomed");
            zoomed = !zoomed;
            cam.fieldOfView = defaultFov;
        }
    }

}
