using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero_Padeira : Hero
{
    [Header("Basic Ability")]
    [SerializeField] private GameObject burnVFX;
    [SerializeField] private GameObject chargeFlamesVFX;
    [SerializeField] private float flamesDuration;
    [SerializeField] private float flamesDamageInterval;
    [SerializeField] private float flamesDamage;
    [SerializeField] private float chargeTimeRequired;
    [SerializeField] private float chargeExtraDamage;
    [SerializeField] private AudioClip chargeSFX;

    [Header("Movement Ability")]
    [SerializeField] private GameObject MALandVFX;
    [SerializeField] private float jumpDistance;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float gravity;
    [SerializeField] [Range(20f, 70f)] private float angle;
    [SerializeField] private float damageRadiusMA;
    [SerializeField] private float damageMA;
    [SerializeField] private float slowDownTimeMA;
    [SerializeField] private float vfxDuration;
    [SerializeField] private AudioClip leapLandSFX;

    [Header("Other Ability")]
    [SerializeField] private GameObject healVFX;
    [SerializeField] private float healValue;
    [SerializeField] private AudioClip healSFX;

    [Header("Ultimate Ability")]
    [SerializeField] private GameObject rollingPin;
    [SerializeField] private GameObject burningGroundVFX;
    [SerializeField] private GameObject stunVFX;
    [SerializeField] private float stunYOffset;
    [SerializeField] private float rollDistance;
    [SerializeField] private float rollSpeed;
    [SerializeField] private float stunDuration;
    [SerializeField] private float floorDamage;
    [SerializeField] private float floorDamageInterval;
    [SerializeField] private float floorDamageDuration;
    [SerializeField] private AudioClip rollingPinSFX;
    [SerializeField] private AudioClip burningFloorSFX;

    // Basic Ability
    private bool attackflagBA;
    private float timeElapsedBA;
    private bool canUseCharge;
    private bool hasSoundPlayedBA;

    // Movement Ability
    private bool attackFlagMA;

    // Ultimate Ability
    private bool attackFlagUA;
    private float timeElapsedUA;
    private float rollDuration;
    private GameObject rollPin;
    private Vector3 targetPosUA;
    private Vector3 startPosUA;

    new void Start()
    {
        base.Start();

        attackflagBA = false;
        canUseCharge = false;

        attackFlagMA = false;

        attackFlagUA = false;
    }

    // BASIC ABILITY METHODS
    protected override void BasicAbility()
    {
        if (!attackflagBA)
        {
            timeElapsedBA = 0;
            attackflagBA = true;
        }

        if (Input.GetButton($"P{PlayerNumber} BA"))
        {
            timeElapsedBA += Time.deltaTime;

            if (timeElapsedBA > chargeTimeRequired)
            {
                canUseCharge = true;
                if (!hasSoundPlayedBA)
                    if (chargeSFX != null)
                    {
                        SoundManager.Instance.PlaySFX(chargeSFX);
                        hasSoundPlayedBA = true;
                    }
                vfxManager.EnableVFX(chargeFlamesVFX, true);
            }
        }
        if (Input.GetButtonUp($"P{PlayerNumber} BA"))
        {
            if (canUseCharge)
            {
                Weapon currentWeapon = weapon1.GetComponent<Weapon>();
                currentWeapon.Abilities[0] = true;
                charAnimator.SetTrigger("Basic Ability");
                ResetBA();
            }
            else
            {
                charAnimator.SetTrigger("Basic Ability");
                ResetBA();
            }
        }
    }

    private void ResetBA()
    {
        basicAbility = false;
        timeElapsedBA = 0;
        canUseCharge = false;
        hasSoundPlayedBA = false;
    }

    public void ResetChargeFlamesEffect()
    {
        vfxManager.EnableVFX(chargeFlamesVFX, false);
    }

    public void AfterHitEffectBA(Transform other)
    {
        if (other != null)
            other.gameObject.SendMessage("TakeDamage", new float[] { chargeExtraDamage * DamageMultiplier, 0 });

        StartCoroutine(SetOnFlames(other));
    }

    private IEnumerator SetOnFlames(Transform other)
    {
        float timeElapsed = 0;
        float lastDamage = 0;
        GameObject flames = null;

        if (other != null)
        {
            flames = Instantiate(burnVFX, other.transform);
        }
        while (timeElapsed < flamesDuration)
        {
            if (lastDamage >= flamesDamageInterval)
            {
                lastDamage = 0;
                if (other != null)
                    other.gameObject.SendMessage("TakeDamage", new float[] { flamesDamage * DamageMultiplier, 0 });
            }

            lastDamage += Time.deltaTime;
            timeElapsed += Time.deltaTime;

            yield return null;
        }
        Destroy(flames);
    }

    // MOVEMENT ABILITY METHODS

    protected override void MovementAbility()
    {
        if (!attackFlagMA)
        {
            attackFlagMA = true;
            charAnimator.SetBool("Movement Ability", movementAbility);
            StartCoroutine(LeapTowards());
        }
    }

    private IEnumerator LeapTowards()
    {
        charMovement.IsMovementAllowed = false;

        float jumpVelocity = jumpDistance / (Mathf.Sin(2 * angle * Mathf.Deg2Rad) / gravity);

        float vX = Mathf.Sqrt(jumpVelocity) * Mathf.Cos(angle * Mathf.Deg2Rad);
        float vY = Mathf.Sqrt(jumpVelocity) * Mathf.Sin(angle * Mathf.Deg2Rad);

        float flightDuration = jumpDistance / vX;

        float elapsedTime = 0;

        while (elapsedTime < flightDuration)
        {
            transform.Translate(0, (vY - (gravity * elapsedTime)) * jumpSpeed * Time.deltaTime, vX * Time.deltaTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);

        ApplyAOEDamage();
        GameObject landFX = vfxManager.InstantiateVFX(MALandVFX, transform, vfxDuration);
        landFX.transform.localScale = new Vector3(damageRadiusMA * 2, 0.01f, damageRadiusMA * 2);

        if (leapLandSFX != null)
            SoundManager.Instance.PlaySFX(leapLandSFX);

        movementAbility = false;
        charAnimator.SetBool("Movement Ability", movementAbility);
        charMovement.IsMovementAllowed = true;
        yield return new WaitForSeconds(vfxDuration);
        Destroy(landFX);
        attackFlagMA = false;
    }

    private void ApplyAOEDamage()
    {
        Collider[] affectedEnemies = Physics.OverlapSphere(transform.position, damageRadiusMA, LayerMask.GetMask("Hitbox"));

        foreach (Collider c in affectedEnemies)
        {
            if (c.transform != transform)
            {
                c.SendMessage("TakeDamage", new float[] { damageMA * DamageMultiplier, 0 });
                StartCoroutine(SlowEnemy(c));
            }
        }
    }

    private IEnumerator SlowEnemy(Collider c)
    {
        float timeElapsed = 0;

        if (c != null)
            c.SendMessage("SlowDown", true);

        while (timeElapsed < slowDownTimeMA)
        {
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        if (c != null)
            c.SendMessage("SlowDown", false);
    }

    // OTHER ABILITY METHODS

    protected override void OtherAbility()
    {
        otherAbility = false;

        vfxManager.EnableVFX(healVFX, true);
        if (healSFX != null)
            SoundManager.Instance.PlaySFX(healSFX);

        HealHealth(healValue);
    }

    // ULTIMATE ABILITY METHODS

    protected override void UltimateAbility()
    {
        if (!attackFlagUA)
        {
            timeElapsedUA = 0;
            Vector3 rollingPinSpawn = new Vector3(transform.position.x, 0.5f, transform.position.z) + transform.forward * 2;
            rollPin = Instantiate(rollingPin, rollingPinSpawn, transform.rotation, transform);
            rollPin.transform.SetParent(null);

            rollPin.GetComponent<Weapon>().IsAttacking = true;

            targetPosUA = rollPin.transform.position + rollPin.transform.forward * rollDistance;
            startPosUA = rollPin.transform.position;

            rollDuration = (rollDistance / rollSpeed);

            if (rollingPinSFX != null)
                SoundManager.Instance.PlaySFX(rollingPinSFX);
            attackFlagUA = true;
        }

        if (timeElapsedUA <= rollDuration)
        {
            rollPin.GetComponent<Weapon>().Abilities[3] = true;
            rollPin.transform.position = Vector3.Lerp(startPosUA, targetPosUA, timeElapsedUA / rollDuration);
            timeElapsedUA += Time.deltaTime;
        }
        else
        {
            // Create damage zone
            GameObject damagePlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
            damagePlane.transform.rotation = rollPin.transform.rotation;
            damagePlane.transform.position = (startPosUA + targetPosUA) / 2;
            damagePlane.transform.position = new Vector3(damagePlane.transform.position.x, 0.2f, damagePlane.transform.position.z);
            damagePlane.transform.localScale = new Vector3(1, 1, rollDistance / 10f);
            Destroy(damagePlane.GetComponent<Collider>());
            damagePlane.AddComponent<BoxCollider>().isTrigger = true;
            damagePlane.GetComponent<BoxCollider>().size += new Vector3(0, 4, 0);
            damagePlane.AddComponent<Rigidbody>().isKinematic = true;
            damagePlane.AddComponent<DamageZone>().Initialize(floorDamage, floorDamageInterval, floorDamageDuration, PlayerNumber);
            damagePlane.GetComponent<MeshRenderer>().enabled = false;

            vfxManager.InstantiateVFX(burningGroundVFX, damagePlane.transform, floorDamageDuration);

            attackFlagUA = false;
            ultimateAbility = false;
            Destroy(rollPin);
        }
    }

    public void AfterHitEffectUA(Transform other)
    {
        if (other != null)
            StartCoroutine(Stun(other));
    }

    private IEnumerator Stun(Transform other)
    {
        float timeElapsed = 0;

        if (other != null)
            vfxManager.InstantiateVFXWithYOffset(stunVFX, other, stunDuration, stunYOffset);

        while (timeElapsed < stunDuration)
        {
            if (other != null)
                other.gameObject.SendMessage("AllowMovement", false);

            timeElapsed += Time.deltaTime;

            yield return null;
        }

        if (other != null)
            other.gameObject.SendMessage("AllowMovement", true);
    }

    public override void ResetWeapon()
    {
        weapon1.GetComponent<Weapon>().IsAttacking = false;
        charAnimator.SetBool("Basic Ability", false);
    }
}
