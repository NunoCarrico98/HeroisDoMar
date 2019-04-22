using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero_Padeira : Hero
{
    [Header("Basic Ability")]
    [SerializeField] private float chargeTimeRequired;
    [SerializeField] private float chargeExtraDamage;
    [Header("Movement Ability")]
    [SerializeField] private float jumpDistance;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float gravity;
    [SerializeField] [Range(20f, 70f)] private float angle;
    [Header("Other Ability")]
    [SerializeField] private float healValue;

    // Basic Ability
    private bool attackflagBA;
    private float timeElapsedBA;

    // Movement Ability
    private bool attackFlagMA;

    new void Start()
    {
        base.Start();

        attackFlagMA = false;
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
        throw new System.NotImplementedException();
    }

    private IEnumerator LeapTowards()
    {
        charMovement.IsMovementAllowed = false;
        yield return new WaitForSeconds(0.5f);

        float jumpVelocity = jumpDistance / (Mathf.Sin(2 * angle * Mathf.Deg2Rad) / gravity);

        float vX = Mathf.Sqrt(jumpVelocity) * Mathf.Cos(angle * Mathf.Deg2Rad);
        float vY = Mathf.Sqrt(jumpVelocity) * Mathf.Sin(angle * Mathf.Deg2Rad);
        float vZ = Mathf.Sqrt(jumpVelocity) * Mathf.Tan(angle * Mathf.Deg2Rad);

        float flightDuration = jumpDistance / vX;

        float elapsedTime = 0;

        while (elapsedTime < flightDuration)
        {
            transform.Translate(0, (vY - (gravity * elapsedTime)) * jumpSpeed * Time.deltaTime, vX * Time.deltaTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);

        yield return new WaitForSeconds(1.1f);
        charMovement.IsMovementAllowed = true;
        attackFlagMA = false;
        movementAbility = false;
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
