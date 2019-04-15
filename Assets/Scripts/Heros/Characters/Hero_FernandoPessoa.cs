using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero_FernandoPessoa : Hero
{
    [Header("Basic Ability")]
    [SerializeField] private float maxBADistance;
    [SerializeField] private float durationBA;
    [Header("Other Ability")]
    [SerializeField] private float movementSpeedIncrease;
    [SerializeField] private float durationOA;
    [SerializeField] private ParticleSystem particlesOA;

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
    }

    // old hat code
    /*
    protected override void BasicAbility()
    {
        if (!attackFlag)
        {
            localRotation = weapon1.transform.localRotation;
            localStartingPosition = weapon1.transform.localPosition;
            startingPosition = weapon1.transform.position;
            targetPos = weapon1.transform.position + weapon1.transform.forward * maxRangeDistance;
            attackFlag = true;
            weapon1.transform.SetParent(null);
        }

        weapon1.transform.position = Vector3.Lerp(startingPosition, targetPos, Mathf.InverseLerp(0, rangedAttackSpeed, timeElapsed));
        timeElapsed += Time.deltaTime;

        if (timeElapsed >= rangedAttackSpeed)
        {
            ResetWeapon();
        }
    } */

    protected override void BasicAbility()
    {
        if (!attackFlag)
        {
            charAnimator.SetBool("Hat Swing", true);
            attackFlag = true;
        }

        timeElapsed += Time.deltaTime;

        if (timeElapsed >= durationBA / 3)
            weapon1.GetComponent<Animator>().Play("Frisbee Flying");

        if (timeElapsed >= durationBA)
        {
            charAnimator.SetBool("Hat Swing", false);
            weapon1.GetComponent<Animator>().Play("Default");
            attackFlag = false;
            basicAbility = false;
            timeElapsed = 0;
        }
    }

    protected override void MovementAbility()
    {
    }

    protected override void OtherAbility()
    {
        otherAbility = false;
        StartCoroutine(OtherAbilityDurationTimer());
    }

    protected override void UltimateAbility()
    {
    }

    private IEnumerator OtherAbilityDurationTimer()
    {
        float temp = charMovement.MovementSpeed;
        charMovement.MovementSpeed += movementSpeedIncrease;
        particlesOA.Play();

        for (float i = durationOA; i > 0; i -= 0.1f)
        {
            yield return new WaitForSecondsRealtime(0.1f);
        }

        charMovement.MovementSpeed = temp;
        particlesOA.Stop();
    }

    // old hat code
    /*
    public override void ResetWeapon()
    {
        basicAbility = false;
        timeElapsed = 0;
        attackFlag = false;
        weapon1.transform.SetParent(transform);
        weapon1.transform.localPosition = localStartingPosition;
        weapon1.transform.localRotation = localRotation;
    }
    */

    public override void ResetWeapon()
    {
        charAnimator.SetBool("Hat Swing", false);
        weapon1.GetComponent<Animator>().Play("Default");
        attackFlag = false;
        basicAbility = false;
        timeElapsed = 0;
    }
}
