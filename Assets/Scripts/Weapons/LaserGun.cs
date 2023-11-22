using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : WeaponBase
{

    private LineRenderer lr;
    private bool laserActive;
    //private bool currentlyAttacking = false;
    [SerializeField]private float damage;
    [SerializeField] private float chargeDamage;
    private float charging = 0;
    private float maxCharge = 3f;
    private Player player;

    private Vector3 origin;
   // private Vector3 endPoint;
    private void Awake()
    {
        player = transform.root.gameObject.GetComponent<Player>();
        lr = this.gameObject.AddComponent<LineRenderer>();
        lr.startWidth = (0.2f);
        lr.endWidth = 0.2f;
        lr.SetPosition(0, transform.position);
        lr.enabled = false;
    }

    protected override void Attack(float chargePercent)
    {

        // Debug.Log(charging);
        if (player.GetCurrentAmmo() > 0)
        {
            if (charging < maxCharge)
            {
                charging += Time.deltaTime / 2;

                lr.startWidth = 0.2f * charging;
                lr.endWidth = 0.2f * charging;
                chargeDamage = damage * charging;
            }

            //Debug.Log("shoot");

            laserActive = true;
            lr.enabled = laserActive;
            lr.SetPosition(0, transform.position);
            origin = lr.GetPosition(0);

            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 10, Color.white);
            RaycastHit hit;
            if (Physics.Raycast(origin, transform.forward * 10, out hit))
            {
                lr.SetPosition(1, hit.point);
                if (hit.collider.transform.root.tag == "Enemy")
                {

                    Damage(hit.collider, chargeDamage);
                }
            }
            else lr.SetPosition(1, origin + transform.forward * 10f);

            player.ShotFired(charging/15);
            if(player.GetCurrentAmmo() <= 0)
            {
                StopLaser();
            }
        }
        
       

        
        //laserActive = false;

    }


    private void Damage(Collider col, float fullDamage)
    {
        Debug.Log("hit");
        col.transform.root.TryGetComponent(out IDamagable hitObject);
        hitObject.TakeDamage(fullDamage);
    }

   

    public void StopLaser()
    {
        lr.enabled = false;
        charging = 0f;
    }
}
