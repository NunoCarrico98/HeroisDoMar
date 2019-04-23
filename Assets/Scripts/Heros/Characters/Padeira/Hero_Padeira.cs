using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero_Padeira : Hero
{
    [Header("Basic Ability")]
    [SerializeField] private GameObject chargeFlamesEffect;
    [SerializeField] private float flamesDuration;
    [SerializeField] private float flamesDamageInterval;
    [SerializeField] private float flamesDamage;
    [SerializeField] private float chargeTimeRequired;
    [SerializeField] private float chargeExtraDamage;
    [Header("Movement Ability")]
    [SerializeField] private GameObject landEffectMA;
    [SerializeField] private float jumpDistance;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float gravity;
    [SerializeField] [Range(20f, 70f)] private float angle;
    [SerializeField] private float damageRadiusMA;
    [SerializeField] private float damageMA;
    [SerializeField] private float slowDownTimeMA;
    [Header("Other Ability")]
    [SerializeField] private float healValue;
    [Header("Ultimate Ability")]
    [SerializeField] private GameObject rollingPin;
    [SerializeField] private float rollDistance;
    [SerializeField] private float rollSpeed;

    // Basic Ability
    private bool attackflagBA;
    private float timeElapsedBA;

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

        attackFlagMA = false;
        attackFlagUA = false;
    }

    protected override void BasicAbility()
    {
        if (!attackflagBA)
        {
            timeElapsedBA = 0;
            attackflagBA = true;
        }

        if (Input.GetButton($"P{PlayerNumber} BA"))
        {
            Debug.Log("got here");
            timeElapsedBA += Time.deltaTime;

            if (timeElapsedBA > chargeTimeRequired)
            {
                weapon1.GetComponent<Weapon>().IsAttacking = true;
                weapon1.GetComponent<Weapon>().ExtraDamage = chargeExtraDamage;
                Debug.Log(weapon1.GetComponent<Weapon>().ExtraDamage);
                charAnimator.SetBool("Basic Ability", true);
                basicAbility = false;
                Debug.Log("CHAARGEEE!");
                timeElapsedBA = 0;
            }
        }
        else
        {
            weapon1.GetComponent<Weapon>().IsAttacking = true;
            charAnimator.SetBool("Basic Ability", true);
            basicAbility = false;
            Debug.Log("BASIC");
        }
    }

    protected override void MovementAbility()
    {
        if (!attackFlagMA)
        {
            attackFlagMA = true;
            charAnimator.SetBool("Movement Ability", true);
            StartCoroutine(LeapTowards());
        }
    }

    protected override void OtherAbility()
    {
        otherAbility = false;

        currentHealth += healValue;
        if (currentHealth > maximumHealth) currentHealth = maximumHealth;

        healthBar.SetHealthBarSize(currentHealth);
    }

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
            attackFlagUA = true;
        }

        if (timeElapsedUA <= rollDuration)
        {
            Debug.Log(timeElapsedUA);
            rollPin.transform.position = Vector3.Lerp(startPosUA, targetPosUA, timeElapsedUA / rollDuration);
            timeElapsedUA += Time.deltaTime;
        }
        else
        {
            attackFlagUA = false;
            ultimateAbility = false;
            Destroy(rollPin);
        }
    }

    private IEnumerator LeapTowards()
    {
        charMovement.IsMovementAllowed = false;
        yield return new WaitForSeconds(0.5f);

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
        GameObject landFX = Instantiate(landEffectMA, transform.position, transform.rotation);
        landFX.transform.localScale = new Vector3(damageRadiusMA * 2, 0.01f, damageRadiusMA * 2);

        yield return new WaitForSeconds(1.1f);
        Destroy(landFX);
        charMovement.IsMovementAllowed = true;
        attackFlagMA = false;
        movementAbility = false;
    }

    private void ApplyAOEDamage()
    {
        Collider[] affectedEnemies = Physics.OverlapSphere(transform.position, damageRadiusMA, LayerMask.GetMask("Hitbox"));

        foreach (Collider c in affectedEnemies)
        {
            if (c.name != $"Player {PlayerNumber}" && c.transform != transform)
            {
                c.SendMessageUpwards("TakeDamage", damageMA);
                StartCoroutine(SlowEnemy(c));
            }
        }
    }

    private IEnumerator SlowEnemy(Collider c)
    {
        float timeElapsed = 0;
        Hero enemy = c.GetComponent<Hero>();
        enemy.CharMovement.IsSlowed = true;

        while (timeElapsed < slowDownTimeMA)
        {
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        enemy.CharMovement.IsSlowed = false;
    }

    public override void ResetWeapon()
    {
        weapon1.GetComponent<Weapon>().IsAttacking = false;
        charAnimator.SetBool("Basic Ability", false);
    }

    public void OnAnimationEnded(int n)
    {
        switch (n)
        {
            case 1:
                charAnimator.SetBool("Basic Ability", false);
                break;
            case 2:
                charAnimator.SetBool("Movement Ability", false);
                break;
        }
    }
}
