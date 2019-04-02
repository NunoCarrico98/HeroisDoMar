using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero_FernandoPessoa : Hero
{
    [Header("Melee")]
    [SerializeField] private float maxMeleeDistance;
    [SerializeField] private float meleeAttackSpeed;
    [Header("Ranged")]
    [SerializeField] private float maxRangeDistance;
    [SerializeField] private float rangedAttackSpeed;

    private Vector3 startingPos;
    private Vector3 targetPos;
    
    private float timeElapsed;
    private bool attackDistanceFlag;

    private void Start()
    {
        timeElapsed = 0;
        attackDistanceFlag = false;
    }

    protected override void MeleeAttack()
    {
        if (!attackDistanceFlag)
        {
            startingPos = meleeWeapon.transform.localPosition;
            targetPos = meleeWeapon.transform.localPosition + new Vector3(0, 0, maxMeleeDistance);
            attackDistanceFlag = true;
        }

        meleeWeapon.transform.localPosition = Vector3.Lerp(startingPos, targetPos, Mathf.InverseLerp(0, meleeAttackSpeed, timeElapsed));
        timeElapsed += Time.deltaTime;

        if (timeElapsed >= meleeAttackSpeed)
        {
            isR1InUse = false;
            timeElapsed = 0;
            attackDistanceFlag = false;
            meleeWeapon.transform.localPosition = startingPos;
        }
    }

    protected override void RangeAttack()
    {
        if (!attackDistanceFlag)
        {
            startingPos = rangeWeapon.transform.localPosition;
            targetPos = rangeWeapon.transform.localPosition + new Vector3(0, 0, maxRangeDistance);
            attackDistanceFlag = true;
        }

        rangeWeapon.transform.localPosition = Vector3.Lerp(startingPos, targetPos, Mathf.InverseLerp(0, rangedAttackSpeed, timeElapsed));
        timeElapsed += Time.deltaTime;

        if (timeElapsed >= rangedAttackSpeed)
        {
            isL1InUse = false;
            timeElapsed = 0;
            attackDistanceFlag = false;
            rangeWeapon.transform.localPosition = startingPos;
        }
    }

    protected override void RegularAbility()
    {
    }

    protected override void UltimateAbility()
    {
    }
}
