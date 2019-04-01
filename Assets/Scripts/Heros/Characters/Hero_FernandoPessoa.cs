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

    private float timeElapsed;

    private void Start()
    {
        timeElapsed = 0;
    }

    protected override void MeleeAttack()
    {
        Vector3 startingPos = meleeWeapon.transform.localPosition;

        meleeWeapon.transform.localPosition 
            = Vector3.Lerp(startingPos, startingPos += new Vector3(0, 0, maxMeleeDistance), meleeAttackSpeed * Time.deltaTime);

        timeElapsed += Time.deltaTime;
        if (timeElapsed >= meleeAttackSpeed)
        {
            isR1InUse = false;
            timeElapsed = 0;
        }

        //StartCoroutine(SwingSword());
    }

    protected override void RangeAttack()
    {
    }

    protected override void RegularAbility()
    {
    }

    protected override void UltimateAbility()
    {
    }

    // NEED TO FIX DIS SHIT
    IEnumerator SwingSword()
    {
        float elapsedTime = 0;
        Vector3 startingPos = meleeWeapon.transform.localPosition;

        while (elapsedTime < meleeAttackSpeed)
        {
            elapsedTime += Time.deltaTime;
            //meleeWeapon.transform.Translate(new Vector3(0, 0, maxMeleeDistance) * meleeAttackSpeed * Time.deltaTime);
            meleeWeapon.transform.localPosition = Vector3.Lerp(startingPos, startingPos += new Vector3(0, 0, maxMeleeDistance), meleeAttackSpeed);
            yield return new WaitForEndOfFrame();
        }
    }
}
