using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero_FernandoPessoa : Hero
{
    [Header("Basic Ability")]
    [SerializeField] private float distanceBA;
    [SerializeField] private float durationForwardBA;
    [Header("Movement Ability")]
    [SerializeField] private GameObject decoyMA;
    [SerializeField] private GameObject switchPositionVFX;
    [SerializeField] private float decoyLifetimeMA;
    [SerializeField] private float decoyHealthMA;
    [SerializeField] private float targetRadius;
    [SerializeField] private float explosionRadiusMA;
    [SerializeField] private float explosionDamageMA;
    [SerializeField] private float secondsUntilSeekingTargetMA;
    [SerializeField] private float delayForSwitch;
    [Header("Other Ability")]
    [SerializeField] private GameObject speedBuffVFX;
    [SerializeField] private float moveSpeedIncreaseOA;
    [SerializeField] private float durationOA;
    [Header("Ultimate Ability")]
    [SerializeField] private GameObject decoyUA;
    [SerializeField] private float secondsForExplosion;
    [SerializeField] private float decoyLifetimeUA;
    [SerializeField] private float decoyHealthUA;
    [SerializeField] private float explosionRadiusUA;
    [SerializeField] private float explosionDamageUA;

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

    // Ultimate Ability


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

        if (isComingBack && Vector3.Distance(boomerang.transform.position, weapon1.transform.position) < 0.2f)
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
            decoy.Initialize(PlayerNumber, decoyLifetimeMA, decoyHealthMA, charMovement.MovementSpeed, 
                targetRadius, explosionRadiusMA, explosionDamageMA, secondsUntilSeekingTargetMA, vfxManager);
            attackFlagMA = true;
        }
        timeElapsedMA += Time.deltaTime;

        if (decoy != null)
            if (Vector3.Distance(transform.position, decoy.transform.position) > decoyCollider.radius * 2)
                decoyCollider.enabled = true;

        if (decoy != null)
        {
            if (InputManager.GetButtonDown(PlayerNumber, "MA") && !positionSwitched && timeElapsedMA > delayForSwitch)
            {
                Debug.Log("entered here");

                positionSwitched = true;
                Vector3 tempPos = decoy.transform.position;

                Instantiate(switchPositionVFX, transform.position, transform.rotation);
                Instantiate(switchPositionVFX, decoy.transform.position, decoy.transform.rotation);

                decoy.AllowMovement(false);
                AllowMovement(false);

                decoy.transform.position = transform.position;
                transform.position = tempPos;
            }
            else if (positionSwitched)
            {
                decoy.AllowMovement(true);
                AllowMovement(true);
            }
        }
        if (timeElapsedMA >= decoyLifetimeMA)
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
        float timeElapsed = 0;

        float temp = charMovement.MovementSpeed;
        charMovement.MovementSpeed += moveSpeedIncreaseOA;

		GameObject speedEffect = vfxManager.InstantiateVFX(speedBuffVFX, transform, durationOA);
        speedEffect.transform.SetParent(transform);

        while (timeElapsed < durationOA)
        {
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        charMovement.MovementSpeed = temp;
        Destroy(speedEffect.gameObject);
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
