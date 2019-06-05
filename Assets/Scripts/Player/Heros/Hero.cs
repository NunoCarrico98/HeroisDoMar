using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class Hero : MonoBehaviour
{
    [Header("Player number")]
    [SerializeField] private int pNumber;

    [Header("Abilities UI")]
    [SerializeField] protected Image meleeUICooldownPanel;
    [SerializeField] protected Image rangedUICooldownPanel;
    [SerializeField] protected Image regularAbilityUICooldownPanel;
    [SerializeField] protected Image ultimateUICooldownPanel;

	[Header("Hero Stats")]
	[SerializeField] protected Image healthBar;
	[SerializeField] protected Image shieldBar;

    [Header("Maximum Health and Shield")]
    [SerializeField] protected float maximumHealth;
    [SerializeField] protected float maximumShield;

    [Header("Cooldowns")]
    [SerializeField] protected float basicAbilityCooldown;
    [SerializeField] protected float movementAbilityCooldown;
    [SerializeField] protected float otherAbilityCooldown;
    [SerializeField] protected float ultimateAbilityCooldown;

    [Header("Weapon(s)")]
    [SerializeField] protected Collider weapon1;

	//VFX
	protected VFXManager vfxManager;

	protected UIManager uiManager;

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
	public float CurrentHealth { get; set; }
	public float CurrentShield { get; set; }
	public float DamageMultiplier { get; set; } = 1;

	protected abstract void MovementAbility();
    protected abstract void BasicAbility();
    protected abstract void OtherAbility();
    protected abstract void UltimateAbility();

    public abstract void ResetWeapon();

    // Start is called before the first frame update
    protected void Start()
    {
		vfxManager = FindObjectOfType<VFXManager>();
		uiManager = FindObjectOfType<UIManager>();

        CurrentHealth = maximumHealth;
        CurrentShield = maximumShield;

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
                basicAbility = true;
                lockedBasicAbility = true;
                StartCoroutine(AbilityCooldown("BA", basicAbilityCooldown, meleeUICooldownPanel));
            }
        }
        if (!lockedMovementAbility)
        {
            if (InputManager.GetButtonDown(pNumber, "MA"))
            {
                movementAbility = true;
                lockedMovementAbility = true;
                StartCoroutine(AbilityCooldown("MA", movementAbilityCooldown, rangedUICooldownPanel));
            }
        }
        if (!lockedOtherAbility)
        {
            if (InputManager.GetButtonDown(pNumber, "OA") && !lockedOtherAbility)
            {
                otherAbility = true;
                lockedOtherAbility = true;
                StartCoroutine(AbilityCooldown("OA", otherAbilityCooldown, regularAbilityUICooldownPanel));
            }
        }
        if (!lockedUltimateAbility)
        {
            if (InputManager.GetButtonDown(pNumber, "UA") && !lockedUltimateAbility)
            {
                ultimateAbility = true;
                lockedUltimateAbility = true;
                StartCoroutine(AbilityCooldown("UA", ultimateAbilityCooldown, ultimateUICooldownPanel));
            }
        }
    }

    private IEnumerator AbilityCooldown(string abilityName, float cooldown, Image cooldownPanel)
    {
        for (float i = cooldown; i > 0; i -= 0.1f)
        {
			uiManager.SetYBarSize(cooldownPanel, i, cooldown);
            yield return new WaitForSecondsRealtime(0.1f);
        }

        switch (abilityName)
        {
            case "BA":
                lockedBasicAbility = false;
                break;
            case "MA":
                lockedMovementAbility = false;
                break;
            case "OA":
                lockedOtherAbility = false;
                break;
            case "UA":
                lockedUltimateAbility = false;
                break;
        }
    }

	public void VerifyMaxHealth()
	{
		if (CurrentHealth > maximumHealth) CurrentHealth = maximumHealth;
	}

	public void VerifyMaxShield()
	{
		if (CurrentShield > maximumShield) CurrentShield = maximumShield;
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

		// If hero has shield
        if (CurrentShield > 0)
        {
			// Remove hitpoints from shield
            CurrentShield -= damage;
            if (CurrentShield < 0)
            {
                float damageToHp = CurrentShield;
                CurrentHealth += damageToHp;
                CurrentShield = 0;
            }
        }
		// If hero does not have shields
        else
        {
            CurrentHealth -= damage;
        }

		// Die
        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            Die();
        }

		// Set Health Bar
        uiManager.SetXBarSize(healthBar, CurrentHealth, maximumHealth);
		// Set Shield Bar
        uiManager.SetXBarSize(shieldBar, CurrentShield, maximumShield);
    }
}
