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

    protected override void MeleeAttack()
    {
        Attack(meleeWeapon);
        StartCoroutine(SwingSword
            (meleeWeapon.transform.position + new Vector3(0, 0, maxMeleeDistance)));
    }

    protected override void RangeAttack()
    {
        Attack(rangeWeapon);
    }

    protected override void RegularAbility()
    {
        throw new System.NotImplementedException();
    }

    protected override void UltimateAbility()
    {
        throw new System.NotImplementedException();
    }

    // NEED TO FIX DIS SHIT
    IEnumerator SwingSword(Vector3 endPos)
    {
        float elapsedTime = 0;
        Vector3 startingPos = meleeWeapon.transform.position;

        while (elapsedTime < meleeAttackSpeed)
        {
            elapsedTime += Time.deltaTime;
            meleeWeapon.transform.position = Vector3.Lerp(startingPos, endPos, meleeAttackSpeed);
            yield return null;
        }
    }
}
