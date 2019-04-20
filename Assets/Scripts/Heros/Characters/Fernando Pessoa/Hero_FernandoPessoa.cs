﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero_FernandoPessoa : Hero
{
    [Header("Basic Ability")]
    [SerializeField]
    private float distanceBA;
    [SerializeField] private float durationForwardBA;
    [Header("Movement Ability")]
    [SerializeField]
    private GameObject decoyMA;
    [SerializeField] private GameObject switchPositionEffect;
    [SerializeField] private float decoyLifetime;
    [SerializeField] private float decoyHealthMA;
    [SerializeField] private float targetRadius;
    [SerializeField] private float explosionRadius;
    [SerializeField] private float explosionDamage;
    [SerializeField] private float secondsUntilSeekingTargetMA;
    [SerializeField] private float timeForSwitch;
    [Header("Other Ability")]
    [SerializeField]
    private float moveSpeedIncreaseOA;
    [SerializeField] private float durationOA;
    [SerializeField] private ParticleSystem particlesOA;

    // Basic Ability - Boomerang
    private Collider boomerang;
    private Vector3 startingPosition;
    private Vector3 targetPos;
    private float timeElapsedBA;
    private bool attackFlagBA;
    private bool isComingBack;

    // Movement Ability
    private DecoyController decoy;
    private CapsuleCollider decoyCollider;
    private bool attackFlagMA;
    private bool positionSwitched;
    private float timeElapsedMA;

    private new void Start()
    {
        base.Start();

        timeElapsedBA = 0;
        attackFlagBA = false;

        timeElapsedMA = 0;
        attackFlagMA = false;
        positionSwitched = false;
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

        if (timeElapsedBA >= durationForwardBA && !isComingBack)
        {
            isComingBack = true;
            timeElapsedBA = 0;
            startingPosition = boomerang.transform.position;
        }

        if (timeElapsedBA >= durationForwardBA * 2 && isComingBack)
        {
            ResetWeapon();
        }

        timeElapsedBA += Time.deltaTime;

        if (!isComingBack)
            boomerang.transform.position =
                Vector3.Lerp(startingPosition, targetPos, Mathf.InverseLerp(0, durationForwardBA, timeElapsedBA));
        else
            boomerang.transform.position =
                Vector3.MoveTowards(boomerang.transform.position, weapon1.transform.position, (distanceBA / durationForwardBA) * Time.deltaTime);
    }

    protected override void MovementAbility()
    {
        if (!attackFlagMA)
        {
            decoy = Instantiate(decoyMA, transform.position, transform.rotation).GetComponent<DecoyController>();
            decoyCollider = decoy.GetComponent<CapsuleCollider>();
            decoy.Initialize(PlayerNumber, decoyLifetime, maximumHealth, charMovement.MovementSpeed, targetRadius, explosionRadius, explosionDamage, secondsUntilSeekingTargetMA);
            attackFlagMA = true;
        }
        timeElapsedMA += Time.deltaTime;

        if (decoy != null)
            if (Vector3.Distance(transform.position, decoy.transform.position) > decoyCollider.radius * 2)
                decoyCollider.enabled = true;

        if (decoy != null)
            if (InputManager.GetButtonDown(PlayerNumber, "MA") && !positionSwitched && timeElapsedMA > timeForSwitch)
            {
                positionSwitched = true;
                Vector3 tempPos = decoy.transform.position;
                Quaternion tempRot = decoy.transform.rotation;

                Instantiate(switchPositionEffect, transform.position, transform.rotation);
                Instantiate(switchPositionEffect, decoy.transform.position, decoy.transform.rotation);

                decoy.transform.position = transform.position;
                transform.position = tempPos;
                decoy.transform.rotation = transform.rotation;
                transform.rotation = tempRot;
            }

        if (timeElapsedMA >= decoyLifetime)
        {
            movementAbility = false;
            attackFlagMA = false;
            positionSwitched = false;
            timeElapsedMA = 0;
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
        timeElapsedBA = 0;
        attackFlagBA = false;
    }
}
