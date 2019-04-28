using System.Collections;
using TMPro;
using UnityEngine;
using System;

public abstract class Hero : MonoBehaviour
{
    [Header("Player number")]
    [SerializeField]
    private int pNumber;

    [Header("Abilities UI")]
    [SerializeField] protected TextMeshProUGUI meleeUIText;
    [SerializeField] protected TextMeshProUGUI rangedUIText;
    [SerializeField] protected TextMeshProUGUI regularAbilityUIText;
    [SerializeField] protected TextMeshProUGUI ultimateUIText;

    [Header("Maximum Health and Shield")]
    [SerializeField] protected float maximumHealth;
    [SerializeField] private float maximumShield;

    [Header("Cooldowns")]
    [SerializeField] protected float basicAbilityCooldown;
    [SerializeField] protected float movementAbilityCooldown;
    [SerializeField] protected float otherAbilityCooldown;
    [SerializeField] protected float ultimateAbilityCooldown;

    [Header("Weapon(s)")]
    [SerializeField] protected Collider weapon1;

	//VFX
	protected VFXManager vfxManager;

    protected AttributeBars attributeBar;

    protected float currentHealth;
    protected float currentShield;

    // Cooldowns
    private bool lockedBasicAbility;
    private bool lockedMovementAbility;
    private bool lockedOtherAbility;
    private bool lockedUltimateAbility;

    // Input
    protected bool basicAbility;
    protected bool movementAbility;
    protected bool otherAbility;
    protected bool ultimateAbility;

    protected Animator charAnimator;
    protected CharacterMovement charMovement;

    public CharacterMovement CharMovement => charMovement;
    public int PlayerNumber => pNumber;

    protected abstract void MovementAbility();
    protected abstract void BasicAbility();
    protected abstract void OtherAbility();
    protected abstract void UltimateAbility();

    public abstract void ResetWeapon();

    // Start is called before the first frame update
    protected void Start()
    {
		vfxManager = FindObjectOfType<VFXManager>();

        attributeBar = GetComponent<AttributeBars>();
        attributeBar.MaximumHealth = maximumHealth;
        attributeBar.MaximumShield = maximumShield;
        currentHealth = maximumHealth;
        currentShield = maximumShield;

        charMovement = GetComponent<CharacterMovement>();
        charAnimator = GetComponent<Animator>();

        basicAbility = false;
        movementAbility = false;
        otherAbility = false;
        ultimateAbility = false;

        lockedBasicAbility = false;
        lockedMovementAbility = false;
        lockedOtherAbility = false;
        lockedUltimateAbility = false;
    }

    private void Update()
    {
        charMovement.Move();

        ManageInput();
        if (basicAbility) BasicAbility();
        if (movementAbility) MovementAbility();
        if (otherAbility) OtherAbility();
        if (ultimateAbility) UltimateAbility();
    }

    private void ManageInput()
    {
        if (!lockedBasicAbility)
        {
            if (InputManager.GetButtonDown(pNumber, "BA"))
            {
                Debug.Log("Basic Ability");
                basicAbility = true;
                lockedBasicAbility = true;
                StartCoroutine(AbilityCooldown("BA", basicAbilityCooldown, meleeUIText));
            }
        }
        if (!lockedMovementAbility)
        {
            if (InputManager.GetButtonDown(pNumber, "MA"))
            {
                Debug.Log("Movement Ability");
                movementAbility = true;
                lockedMovementAbility = true;
                StartCoroutine(AbilityCooldown("MA", movementAbilityCooldown, rangedUIText));
            }
        }
        if (!lockedOtherAbility)
        {
            if (InputManager.GetButtonDown(pNumber, "OA") && !lockedOtherAbility)
            {
                Debug.Log("Other Ability");
                otherAbility = true;
                lockedOtherAbility = true;
                StartCoroutine(AbilityCooldown("OA", otherAbilityCooldown, regularAbilityUIText));
            }
        }
        if (!lockedUltimateAbility)
        {
            if (InputManager.GetButtonDown(pNumber, "UA") && !lockedUltimateAbility)
            {
                Debug.Log("Ultimate Ability");
                ultimateAbility = true;
                lockedUltimateAbility = true;
                StartCoroutine(AbilityCooldown("UA", ultimateAbilityCooldown, ultimateUIText));
            }
        }
    }

    private IEnumerator AbilityCooldown(string abilityName, float cooldown, TextMeshProUGUI abilityUI)
    {
        string temp = abilityUI.text;

        for (float i = cooldown; i > 0; i -= 0.1f)
        {
            abilityUI.text = $"{i:f}";
            yield return new WaitForSecondsRealtime(0.1f);
        }

        switch (abilityName)
        {
            case "BA":
                lockedBasicAbility = false;
                abilityUI.text = temp;
                break;
            case "MA":
                lockedMovementAbility = false;
                abilityUI.text = temp;
                break;
            case "OA":
                lockedOtherAbility = false;
                abilityUI.text = temp;
                break;
            case "UA":
                lockedUltimateAbility = false;
                abilityUI.text = temp;
                break;
        }
    }

    public void OnAnimationEnded(int n)
    {
        switch (n)
        {
            case 1:
                charAnimator.SetBool("Basic Ability", false);
                SetAttackMode(0);
                break;
            case 2:
                charAnimator.SetBool("Movement Ability", false);
                SetAttackMode(0);
                break;
        }
    }

    public void SetAttackMode(int mode)
    {
        bool condition = Convert.ToBoolean(mode);
        Weapon currentWeapon = weapon1.GetComponent<Weapon>();

        if (currentWeapon != null)
            currentWeapon.IsAttacking = condition;
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    public void AllowMovement(bool movement)
    {
        charMovement.IsMovementAllowed = movement;
    }

    public void SlowDown(bool condition)
    {
        charMovement.IsSlowed = condition;
    }

    public void TakeDamage(float[] weaponProperties)
    {
        float damage = weaponProperties[0];

        if (currentShield > 0)
        {
            currentShield -= damage;
            if (currentShield < 0)
            {
                float damageToHp = currentShield;
                currentHealth += damageToHp;
                currentShield = 0;
            }
        }
        else
        {
            currentHealth -= damage;
        }

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }

        attributeBar.SetHealthBarSize(currentHealth);
        attributeBar.SetShieldBarSize(currentShield);
    }
}
