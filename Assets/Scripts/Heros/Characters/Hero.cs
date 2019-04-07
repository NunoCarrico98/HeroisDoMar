using System.Collections;
using TMPro;
using UnityEngine;

public abstract class Hero : MonoBehaviour
{
    [Header("Player Attack Inputs")]
    [Tooltip("Syntax: P(number) Melee")]
    [SerializeField] private string playerMelee;
    [Tooltip("Syntax: P(number) Ranged")]
    [SerializeField] private string playerRanged;
    [Tooltip("Syntax: P(number) Regular Ability")]
    [SerializeField] private string playerRegularAbility;

    [Header("Abilities UI")]
    [SerializeField] private TextMeshProUGUI meleeUIText;
    [SerializeField] private TextMeshProUGUI rangedUIText;
    [SerializeField] private TextMeshProUGUI regularAbilityUIText;
    [SerializeField] private TextMeshProUGUI ultimateUIText;

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
        else if (isL2InUse)
        {
            RegularAbility();
        }
    }

    private void GetInput()
    {
        if (Input.GetButtonDown(playerMelee) && AreAllAttacksDeactivated() && !meleeIsCoolingDown)
        {
            Debug.Log("Melee");
            isR1InUse = true;
            meleeIsCoolingDown = true;
            StartCoroutine(AbilityCooldown("melee", meleeCooldown, meleeUIText));
        }

        else if (Input.GetButtonDown(playerRanged) && AreAllAttacksDeactivated() && !rangedIsCoolingDown)
        {
            Debug.Log("Ranged");
            isL1InUse = true;
            rangedIsCoolingDown = true;
            StartCoroutine(AbilityCooldown("ranged", rangedCooldown, rangedUIText));
        }

        else if (Input.GetButtonDown(playerRegularAbility) && AreAllAttacksDeactivated() && !regularAbilityIsCoolingDown)
        {
            Debug.Log("Regular Ability");
            isL2InUse = true;
            regularAbilityIsCoolingDown = true;
            StartCoroutine(AbilityCooldown("regular", regularAbilityCooldown, regularAbilityUIText));
        }
    }

    private bool AreAllAttacksDeactivated()
    {
        bool returnVar =
            (!isL1InUse && !isL2InUse && !isR1InUse && !isR2InUse) ? true : false;

        return returnVar;
    }

    protected virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthBar.SetHealthbarSize(damage);
    }

    private IEnumerator AbilityCooldown(string abilityName, int cooldown, TextMeshProUGUI abilityUI)
    {
        string temp = abilityUI.text;

        for (int i = cooldown; i > 0; i--)
        {
            abilityUI.text = $"{i}";
            yield return new WaitForSecondsRealtime(1);
        }

        switch (abilityName)
        {
            case "melee":
                meleeIsCoolingDown = false;
                abilityUI.text = temp;
                break;
            case "ranged":
                rangedIsCoolingDown = false;
                abilityUI.text = temp;
                break;
            case "regular":
                regularAbilityIsCoolingDown = false;
                abilityUI.text = temp;
                break;
            case "ultimate":
                ultimateAbilityIsCoolingDown = false;
                abilityUI.text = temp;
                break;
        }
    }
}
