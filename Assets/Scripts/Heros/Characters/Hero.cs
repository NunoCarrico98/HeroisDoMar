using System.Collections;
using TMPro;
using UnityEngine;

public abstract class Hero : MonoBehaviour
{
    [Header("Player number")]
    [SerializeField] private int pNumber;

    [Header("Abilities UI")]
    [SerializeField] private TextMeshProUGUI meleeUIText;
    [SerializeField] private TextMeshProUGUI rangedUIText;
    [SerializeField] private TextMeshProUGUI regularAbilityUIText;
    [SerializeField] private TextMeshProUGUI ultimateUIText;

    [Header("Maximum Health and Shield")]
    [SerializeField] protected float maximumHealth;
    [SerializeField] private float maximumShield;

    [Header("Cooldowns")]
    [SerializeField] private float basicAbilityCooldown;
    [SerializeField] private float movementAbilityCooldown;
    [SerializeField] private float otherAbilityCooldown;
    [SerializeField] private float ultimateAbilityCooldown;

    [Header("Weapon(s)")]
    [SerializeField] protected Collider weapon1;

    private HealthBar healthBar;

    private float currentHealth;
    private float currentShield;

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

    public int PlayerNumber => pNumber;

    protected abstract void MovementAbility();
    protected abstract void BasicAbility();
    protected abstract void OtherAbility();
    protected abstract void UltimateAbility();

    public abstract void ResetWeapon();

    // Start is called before the first frame update
    protected void Start()
    {
        healthBar = GetComponent<HealthBar>();
        charMovement = GetComponent<CharacterMovement>();
        charAnimator = GetComponentInChildren<Animator>();
        healthBar.MaximumHealth = maximumHealth;
        healthBar.Health = maximumHealth;

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
        ManageInput();
    }

    private void FixedUpdate()
    {
        if (basicAbility) BasicAbility();
        if (movementAbility) MovementAbility();
        if (otherAbility) OtherAbility();
        if (ultimateAbility) UltimateAbility();
    }

    private void ManageInput()
    {
        if (InputManager.GetButtonDown(pNumber, "BA") && !lockedBasicAbility)
        {
            Debug.Log("Basic");
            basicAbility = true;
            lockedBasicAbility = true;
            StartCoroutine(AbilityCooldown("BA", basicAbilityCooldown, meleeUIText));
        }

        else if (InputManager.GetButtonDown(pNumber, "MA") && !lockedMovementAbility)
        {
            Debug.Log("Ranged");
            movementAbility = true;
            lockedMovementAbility = true;
            StartCoroutine(AbilityCooldown("MA", movementAbilityCooldown, rangedUIText));
        }

        else if (InputManager.GetButtonDown(pNumber, "OA") && !lockedOtherAbility)
        {
            Debug.Log("Regular Ability");
            otherAbility = true;
            lockedOtherAbility = true;
            StartCoroutine(AbilityCooldown("OA", otherAbilityCooldown, regularAbilityUIText));
        }
    }

    protected virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthBar.SetHealthbarSize(damage);
    }

    private IEnumerator AbilityCooldown(string abilityName, float cooldown, TextMeshProUGUI abilityUI)
    {
        string temp = abilityUI.text;

        for (float i = cooldown; i > 0; i -= 0.1f)
        {
            abilityUI.text = $"{i}";
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
}
