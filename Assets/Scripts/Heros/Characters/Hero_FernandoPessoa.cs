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
    [SerializeField] private float damageReductionTime;
    [SerializeField] private float damageReductionValue;
    [SerializeField] private GameObject shield;

    private bool dmgReduction;

    private Quaternion localRotation;
    private Vector3 localStartingPosition;
    private Vector3 startingPosition;
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
            localStartingPosition = meleeWeapon.transform.localPosition;
            targetPos = meleeWeapon.transform.localPosition + new Vector3(0, 0, maxMeleeDistance);
            attackFlag = true;
        }

        meleeWeapon.transform.localPosition = Vector3.Lerp(localStartingPosition, targetPos, Mathf.InverseLerp(0, meleeAttackSpeed, timeElapsed));
        timeElapsed += Time.deltaTime;

        if (timeElapsed >= meleeAttackSpeed)
        {
            ResetMeleeWeapon();
        }
    }

    protected override void RangeAttack()
    {
        if (!attackFlag)
        {
            localRotation = rangeWeapon.transform.localRotation;
            localStartingPosition = rangeWeapon.transform.localPosition;
            startingPosition = rangeWeapon.transform.position;
            targetPos = rangeWeapon.transform.position + rangeWeapon.transform.forward * maxRangeDistance;
            attackFlag = true;
            rangeWeapon.transform.SetParent(null);
        }

        rangeWeapon.transform.position = Vector3.Lerp(startingPosition, targetPos, Mathf.InverseLerp(0, rangedAttackSpeed, timeElapsed));
        timeElapsed += Time.deltaTime;

        if (timeElapsed >= rangedAttackSpeed)
        {
            ResetRangedWeapon();
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
        for (float i = damageReductionTime; i > 0; i -= 0.1f)
        {
            yield return new WaitForSecondsRealtime(0.1f);
        }
        dmgReduction = false;
        shield.SetActive(false);
    }

    public override void ResetRangedWeapon()
    {
        isL1InUse = false;
        timeElapsed = 0;
        attackFlag = false;
        rangeWeapon.transform.SetParent(transform);
        rangeWeapon.transform.localPosition = localStartingPosition;
        rangeWeapon.transform.localRotation = localRotation;
    }

    public override void ResetMeleeWeapon()
    {
        isR1InUse = false;
        timeElapsed = 0;
        attackFlag = false;
        meleeWeapon.transform.localPosition = localStartingPosition;
    }
}
