using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
     [SerializeField] private WeaponStatsSO weaponStats;
  

    /*[SerializeField] protected float timeBetweenAttacks;
    [SerializeField] protected float chargeUpTime;
    [SerializeField, Range(0, 1)] protected float minChargePercent;
    [SerializeField] private bool isFullyAuto;
    private WaitForSeconds myCooldownTimer;*/


    private Coroutine currentFireTimer;

    
    private WaitUntil myWaitFunc;
    private bool isOnCoolDown;

    private float currentChargeTime;




    private void Awake()
    {
       
        myWaitFunc = new WaitUntil(() => !isOnCoolDown);
       // myCooldownTimer = new WaitForSeconds(timeBetweenAttacks);
    }

    public void StartFiring()
    {
        currentFireTimer = StartCoroutine(ReFireTimer());
    }
    public void StopFiring()
    {
       // Debug.Log("STOP");
        float p = (currentChargeTime / weaponStats.chargeUpTime);
       // Debug.Log(p);
        if(p != 0) TryAttack(p);

        StopCoroutine(currentFireTimer);
        
    }

    private void TryAttack(float percent)
    {
        currentChargeTime = 0;
        if (!CanAttack(percent)) return;
            Attack(percent);
        StartCoroutine(CoolDown());

        if (weaponStats.isFullyAuto && percent >= 1) currentFireTimer = StartCoroutine(ReFireTimer());
        
    }

    protected virtual bool CanAttack(float percent)
    {
        return !isOnCoolDown && weaponStats.minChargePercent <= percent;
    }

    protected abstract void Attack(float chargePercent);


    private IEnumerator ReFireTimer()
    {
        currentChargeTime = 0;
        print("pre cooldown");
        yield return myWaitFunc;
        print("post cooldown");

        

        while(currentChargeTime < weaponStats.chargeUpTime)
        {
            currentChargeTime += Time.deltaTime;

            yield return null;
        }

        TryAttack(1);
        yield return null;
    }

    private IEnumerator CoolDown()
    {
        Debug.Log("onCoolDown");
        isOnCoolDown = true;
        yield return weaponStats.myCooldownTimer;
        isOnCoolDown = false;
    }

}
