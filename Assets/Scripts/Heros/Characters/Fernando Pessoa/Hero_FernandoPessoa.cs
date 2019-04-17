﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero_FernandoPessoa : Hero
{
    [Header("Basic Ability")]
    [SerializeField] private float distanceBA;
    [SerializeField] private float durationForwardBA;
    [SerializeField] private float durationBackwardBA;
    [Header("Movement Ability")]
    [SerializeField] private GameObject decoyMA;
    [SerializeField] private float decoyLifetime;
    [SerializeField] private float decoyHealthMA;
    [SerializeField] private float targetRadius;
    [SerializeField] private float secondsUntilSeekingTargetMA;
    [Header("Other Ability")]
    [SerializeField] private float moveSpeedIncreaseOA;
    [SerializeField] private float durationOA;
    [SerializeField] private ParticleSystem particlesOA;

    // Basic Ability - Boomerang
    private Collider boomerang;
    private Vector3 startingPosition;
    private Vector3 targetPos;
    private float timeElapsed;
    private bool attackFlagBA;
    private bool isComingBack;

    // Movement Ability
    private DecoyController decoy;
    private CapsuleCollider decoyCollider;
    private bool attackFlagMA;

    private new void Start()
    {
        base.Start();
        timeElapsed = 0;
        attackFlagBA = false;
        attackFlagMA = false;
    }

    protected override void BasicAbility()
    {
        if (!attackFlagBA)
        {
            isComingBack = false;

            boomerang = Instantiate(weapon1, weapon1.transform.position, weapon1.transform.rotation, transform);
            boomerang.GetComponent<Weapon>().IsAttacking = true;
            boomerang.transform.SetParent(null);

            weapon1.GetComponent<MeshRenderer>().enabled = false;

            startingPosition = boomerang.transform.position;
            targetPos = boomerang.transform.position + boomerang.transform.forward * distanceBA;

            attackFlagBA = true;
        }

        if (timeElapsed >= durationForwardBA && !isComingBack)
        {
            isComingBack = true;
            timeElapsed = 0;
            startingPosition = boomerang.transform.position;
        }

        if (timeElapsed >= durationBackwardBA && isComingBack)
        {
            ResetWeapon();
        }

        timeElapsed += Time.deltaTime;

        if (!isComingBack)
            boomerang.transform.position =
                Vector3.Lerp(startingPosition, targetPos, Mathf.InverseLerp(0, durationForwardBA, timeElapsed));
        else
            boomerang.transform.position =
                Vector3.Lerp(startingPosition, weapon1.transform.position, Mathf.InverseLerp(0, durationBackwardBA, timeElapsed));
    }

    protected override void MovementAbility()
    {
        if (!attackFlagMA)
        {
            decoy = Instantiate(decoyMA, transform.position, transform.rotation).GetComponent<DecoyController>();
            decoyCollider = decoy.GetComponent<CapsuleCollider>();
            decoy.Initialize(PlayerNumber, decoyLifetime, maximumHealth, charMovement.MovementSpeed, targetRadius, secondsUntilSeekingTargetMA);
            attackFlagMA = true;
        }
        if (Vector3.Distance(transform.position, decoy.transform.position) > decoyCollider.radius * 2)
        {
            decoyCollider.enabled = true;
            movementAbility = false;
        }
    }

    protected override void OtherAbility()
    {
        otherAbility = false;
        StartCoroutine(TimerOA());
    }

    protected override void UltimateAbility()
    {
    }

    private IEnumerator TimerOA()
    {
        float temp = charMovement.MovementSpeed;
        charMovement.MovementSpeed += moveSpeedIncreaseOA;
        particlesOA.Play();

        for (float i = durationOA; i > 0; i -= 0.1f)
        {
            yield return new WaitForSecondsRealtime(0.1f);
        }

        charMovement.MovementSpeed = temp;
        particlesOA.Stop();
    }

    public override void ResetWeapon()
    {
        weapon1.GetComponent<MeshRenderer>().enabled = true;
        if (boomerang != null)
            Destroy(boomerang.gameObject);

        isComingBack = false;
        basicAbility = false;
        timeElapsed = 0;
        attackFlagBA = false;
    }
}
