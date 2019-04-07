using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Hero : MonoBehaviour
{
    [Header("Player Attack Axis")]
    [Tooltip("Syntax: P(number) Melee")]
    [SerializeField] private string playerMelee;
    [Tooltip("Syntax: P(number) Ranged")]
    [SerializeField] private string playerRanged;

    [Header("Maximum Health")]
    [SerializeField] private float maximumHealth;

    [Header("Cooldowns")]
    [SerializeField] private int meleeCooldown;
    [SerializeField] private int rangedCooldown;
    [SerializeField] private int regularAbilityCooldown;
    [SerializeField] private int ultimateAbilityCooldown;

    private HealthBar healthBar;

    private float currentHealth;

    // Cooldowns
    private bool meleeIsCoolingDown;
    private bool rangedIsCoolingDown;
    private bool regularAbilityIsCoolingDown;
    private bool ultimateAbilityIsCoolingDown;


    // Input
    protected bool isL1InUse;
    protected bool isL2InUse;
    protected bool isR1InUse;
    protected bool isR2InUse;

    [Header("Weapons")]
    [SerializeField] protected Collider meleeWeapon;
    [SerializeField] protected Collider rangeWeapon;

    protected abstract void RangeAttack();
    protected abstract void MeleeAttack();
    protected abstract void RegularAbility();
    protected abstract void UltimateAbility();

    // Start is called before the first frame update
    protected void Start()
    {
        healthBar = GetComponent<HealthBar>();
        healthBar.MaximumHealth = maximumHealth;
        healthBar.Health = maximumHealth;

        isL1InUse = false;
        isL2InUse = false;
        isR1InUse = false;
        isR2InUse = false;

        meleeIsCoolingDown = false;
        rangedIsCoolingDown = false;
        regularAbilityIsCoolingDown = false;
        ultimateAbilityIsCoolingDown = false;
    }

    private void Update()
    {
        GetInput();
    }

    private void FixedUpdate()
    {
        if (isR1InUse)
        {
            MeleeAttack();
        }
        else if (isL1InUse)
        {
            RangeAttack();
        }
    }

    private void GetInput()
    {
        if (Input.GetButtonDown(playerMelee) && AreAllAttacksDeactivated() && !meleeIsCoolingDown)
        {
            Debug.Log("Melee");
            isR1InUse = true;
            meleeIsCoolingDown = true;
            StartCoroutine(AbilityCooldown("melee", meleeCooldown));
        }

        else if (Input.GetButtonDown(playerRanged) && AreAllAttacksDeactivated() && !rangedIsCoolingDown)
        {
            Debug.Log("Ranged");
            isL1InUse = true;
            rangedIsCoolingDown = true;
            StartCoroutine(AbilityCooldown("ranged", rangedCooldown));
        }
    }

    private bool AreAllAttacksDeactivated()
    {
        bool returnVar =
            (!isL1InUse && !isL2InUse && !isR1InUse && !isR2InUse) ? true : false;

        return returnVar;
    }

    private void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthBar.SetHealthbarSize(damage);
    }

    private IEnumerator AbilityCooldown(string ability, int cooldown)
    {
        for (int i = cooldown; i > 0; i--)
        {
            yield return new WaitForSecondsRealtime(1);
        }

        switch (ability)
        {
            case "melee":
                meleeIsCoolingDown = false;
                break;
            case "ranged":
                rangedIsCoolingDown = false;
                break;
            case "regular":
                regularAbilityIsCoolingDown = false;
                break;
            case "ultimate":
                ultimateAbilityIsCoolingDown = false;
                break;
        }
    }
}
