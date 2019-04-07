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
    [Header("Regular Ability")]
    [SerializeField] private int damageReductionTime;
    [SerializeField] private float damageReductionValue;
    [SerializeField] private GameObject shield;

    private bool dmgReduction;

    private Vector3 startingPos;
    private Vector3 targetPos;

    private float timeElapsed;
    private bool attackFlag;

    private new void Start()
    {
        base.Start();
        timeElapsed = 0;
        attackFlag = false;
        dmgReduction = false;
    }

    protected override void MeleeAttack()
    {
        if (!attackFlag)
        {
            startingPos = meleeWeapon.transform.localPosition;
            targetPos = meleeWeapon.transform.localPosition + new Vector3(0, 0, maxMeleeDistance);
            attackFlag = true;
        }

        meleeWeapon.transform.localPosition = Vector3.Lerp(startingPos, targetPos, Mathf.InverseLerp(0, meleeAttackSpeed, timeElapsed));
        timeElapsed += Time.deltaTime;

        if (timeElapsed >= meleeAttackSpeed)
        {
            isR1InUse = false;
            timeElapsed = 0;
            attackFlag = false;
            meleeWeapon.transform.localPosition = startingPos;
        }
    }

    protected override void RangeAttack()
    {
        if (!attackFlag)
        {
            startingPos = rangeWeapon.transform.localPosition;
            targetPos = rangeWeapon.transform.localPosition + new Vector3(0, 0, maxRangeDistance);
            attackFlag = true;
        }

        rangeWeapon.transform.localPosition = Vector3.Lerp(startingPos, targetPos, Mathf.InverseLerp(0, rangedAttackSpeed, timeElapsed));
        timeElapsed += Time.deltaTime;

        if (timeElapsed >= rangedAttackSpeed)
        {
            isL1InUse = false;
            timeElapsed = 0;
            attackFlag = false;
            rangeWeapon.transform.localPosition = startingPos;
        }
    }

    protected override void RegularAbility()
    {
        isL2InUse = false;
        dmgReduction = true;
        shield.SetActive(true);
        StartCoroutine(DamageReductionTimer());
    }

    protected override void UltimateAbility()
    {
    }

    protected override void TakeDamage(float damage)
    {
        if (!dmgReduction)
            base.TakeDamage(damage);
        else
            base.TakeDamage(damage / damageReductionValue);
    }

    private IEnumerator DamageReductionTimer()
    {
        for (int i = damageReductionTime; i > 0; i--)
        {
            yield return new WaitForSecondsRealtime(1);
        }
        dmgReduction = false;
        shield.SetActive(false);
    }
}
