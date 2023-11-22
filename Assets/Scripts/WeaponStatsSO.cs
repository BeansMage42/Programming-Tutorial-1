using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(menuName = "ShootDemo/WeaponSO", fileName = "WeaponStats", order = 1)]
public class WeaponStatsSO : ScriptableObject
{
    [SerializeField] protected float timeBetweenAttacks;
    [field: SerializeField] public float chargeUpTime { get; private set; }
    [field: SerializeField, Range(0, 1)] public float minChargePercent { get; private set; }
    [field: SerializeField] public bool isFullyAuto{ get; private set; }
    public WaitForSeconds myCooldownTimer { get; private set; }

    private void OnEnable()
    {
        myCooldownTimer = new WaitForSeconds(timeBetweenAttacks);
    }

}
