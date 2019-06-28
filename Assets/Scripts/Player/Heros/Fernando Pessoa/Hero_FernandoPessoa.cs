using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero_FernandoPessoa : Hero
{
    [Header("Basic Ability")]
    [SerializeField] private float distanceBA;
    [SerializeField] private float durationForwardBA;
    [SerializeField] private AudioClip boomerangForwardSFX;
    [SerializeField] private AudioClip boomerangBackSFX;

    [Header("Movement Ability")]
    [SerializeField] private GameObject decoyMA;
    [SerializeField] private GameObject switchPositionVFX;
    [SerializeField] private float decoyLifetimeMA;
    [SerializeField] private float decoyHealthMA;
    [SerializeField] private float explosionRadiusMA;
    [SerializeField] private float explosionDamageMA;
    [SerializeField] private float secondsUntilSeekingTargetMA;
    [SerializeField] private float targetRadius;
    [SerializeField] private float delayForSwitch;
    [SerializeField] private AudioClip switchPositionSFX;

    [Header("Other Ability")]
    [SerializeField] private GameObject speedBuffVFX;
    [SerializeField] private float moveSpeedIncreaseOA;
    [SerializeField] private float durationOA;
    [SerializeField] private AudioClip speedBoostVFX;

    [Header("Ultimate Ability")]
    [SerializeField] private GameObject decoyUA;
    [SerializeField] private float decoyLifetimeUA;
    [SerializeField] private float decoyHealthUA;
    [SerializeField] private float explosionRadiusUA;
    [SerializeField] private float explosionDamageUA;
    [SerializeField] private int numberOfDecoys;

    [Header("Common to some habilities")]
    [SerializeField] private AudioClip decoySpawnSFX;

    // Basic Ability - Boomerang
    private Collider boomerang;
    private Vector3 startingPosition;
    private Vector3 targetPos;
    private float timeElapsedBA;
    private bool attackFlagBA;
    private bool isComingBack;

    // Movement Ability
    private DecoyController decoy;
    private CapsuleCollider decoyColliderMA;
    private bool attackFlagMA;
    private bool positionSwitched;
    private float timeElapsedMA;

    // Ultimate Ability
    private List<DecoyController> decoyList;
    private CapsuleCollider decoyColliderUA;
    private bool attackFlagUA;
    private float timeElapsedUA;


    private new void Start()
    {
        base.Start();

        timeElapsedBA = 0;
        attackFlagBA = false;

        timeElapsedMA = 0;
        attackFlagMA = false;
        positionSwitched = false;

        timeElapsedUA = 0;
        attackFlagUA = false;
        decoyList = new List<DecoyController>();
    }

    protected override void BasicAbility()
    {
        if (!attackFlagBA)
        {
            if (boomerangForwardSFX != null)
                SoundManager.Instance.PlaySFX(boomerangForwardSFX);
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
            /*
            if (boomerangBackSFX != null)
                SoundManager.Instance.PlaySFX(boomerangBackSFX); */
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
            decoyColliderMA = decoy.GetComponent<CapsuleCollider>();

            decoy.InitializeDecoy(pNumber: PlayerNumber, decoyLifetime: decoyLifetimeMA,
                health: decoyHealthMA, movementSpeed: charMovement.MovementSpeed,
                targetRadius: targetRadius, explosionRadius: explosionRadiusMA,
                explosionDamage: explosionDamageMA, secondsForTarget: secondsUntilSeekingTargetMA,
                vfxManager: vfxManager, type: DecoyController.DecoyType.MovementDecoy);

            if (decoySpawnSFX != null)
                SoundManager.Instance.PlaySFX(decoySpawnSFX);

            attackFlagMA = true;
        }
        timeElapsedMA += Time.deltaTime;

        if (decoy != null)
            if (Vector3.Distance(transform.position, decoy.transform.position) > decoyColliderMA.radius * 2)
                decoyColliderMA.enabled = true;

        if (decoy != null)
        {
            if (InputManager.GetButtonDown(PlayerNumber, "MA") && !positionSwitched && timeElapsedMA > delayForSwitch)
            {
                Debug.Log("Switched positions!");

                positionSwitched = true;
                Vector3 tempPos = decoy.transform.position;

                Instantiate(switchPositionVFX, transform.position, transform.rotation);
                Instantiate(switchPositionVFX, decoy.transform.position, decoy.transform.rotation);

                if (switchPositionSFX != null)
                    SoundManager.Instance.PlaySFX(switchPositionSFX);

                decoy.AllowMovement(false);
                AllowMovement(false);

                decoy.WarpTo(transform.position);
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
        if (speedBoostVFX != null)
            SoundManager.Instance.PlaySFX(speedBoostVFX);
        StartCoroutine(TimerOA());
    }

    protected override void UltimateAbility()
    {
        if (!attackFlagUA)
        {
            float yRotation = 0;
            float yRotationDegrees = 360 / numberOfDecoys;
            decoyList.Clear();

            for (int i = 0; i < numberOfDecoys; i++)
            {
                decoyList.Add(Instantiate(decoyUA, transform.position, Quaternion.Euler(0, yRotation, 0)).GetComponent<DecoyController>());
                yRotation += yRotationDegrees;
            }

            foreach (DecoyController dc in decoyList)
            {
                if (dc != null)
                    dc.InitializeDecoy(pNumber: PlayerNumber, decoyLifetime: decoyLifetimeUA,
                        health: decoyHealthUA, movementSpeed: charMovement.MovementSpeed,
                        explosionRadius: explosionRadiusUA, explosionDamage: explosionDamageUA,
                        vfxManager: vfxManager, type: DecoyController.DecoyType.UltimateDecoy,
                        numberOfDecoys: numberOfDecoys);
            }

            // Gonna need this in the future for Domino Blow Up
            //foreach (DecoyController dc in decoyList)
            //dc.FindAllyDecoys();

            if (decoySpawnSFX != null)
                SoundManager.Instance.PlaySFX(decoySpawnSFX);

            attackFlagUA = true;
        }

        foreach (DecoyController dc in decoyList)
        {
            if (dc != null)
            {
                CapsuleCollider decoyCollider = dc.GetComponent<CapsuleCollider>();
                if (Vector3.Distance(transform.position, dc.transform.position) > decoyCollider.radius * 2)
                    decoyCollider.enabled = true;
            }
        }

        if (InputManager.GetButtonDown(PlayerNumber, "UA") && timeElapsedUA > 0.1f)
        {
            StartCoroutine(DominoBlowUp());
            /*
            foreach (DecoyController dc in decoyList)
                if (dc != null)
                    dc.Suicide();

            ResetUA(); */
        }

        if (timeElapsedUA > decoyLifetimeUA)
        {
            ResetUA();
        }

        timeElapsedUA += Time.deltaTime;
    }

    private void ResetUA()
    {
        ultimateAbility = false;
        attackFlagUA = false;
        timeElapsedUA = 0;
    }

    private IEnumerator DominoBlowUp()
    {
        foreach (DecoyController dc in decoyList)
        {
            if (dc != null)
                dc.Suicide();
            yield return new WaitForSeconds(0.2f);
        }

        ResetUA();
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
