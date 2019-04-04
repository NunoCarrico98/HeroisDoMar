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

    private HealthBar healthBar;

    private float currentHealth;

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
        Debug.Log($"oi {maximumHealth}");
        healthBar = GetComponent<HealthBar>();
        healthBar.MaximumHealth = maximumHealth;
        healthBar.Health = maximumHealth;

        isL1InUse = false;
        isL2InUse = false;
        isR1InUse = false;
        isR2InUse = false;
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
            DetectAttackCollision(meleeWeapon);
        }
        else if (isL1InUse)
        {
            RangeAttack();
            DetectAttackCollision(rangeWeapon);
        }
    }

    private void GetInput()
    {
        //if (Input.GetAxisRaw(playerMelee) != 0)
        if (Input.GetButtonDown(playerMelee))
        {
            if (!isR1InUse)
            {
                Debug.Log("Melee");
                isR1InUse = true;
            }
        }

        //if (Input.GetAxisRaw(playerRanged) != 0)
        if (Input.GetButtonDown(playerRanged))
        {
            if (!isL1InUse)
            {
                Debug.Log("Range");
                isL1InUse = true;
            }
        }
    }

    protected void DetectAttackCollision(Collider col)
    {
        /* Return all colliders that are overlapping our attacking collider
         * and have a HitBox layer mask */
        Collider[] enemyCols =
            Physics.OverlapBox(col.bounds.center, col.bounds.extents,
            col.transform.rotation, LayerMask.GetMask("Hitbox"));

        foreach (Collider c in enemyCols)
        {
            // Ignore our own body
            if (c.transform == transform)
            {
                Debug.Log("ATAKEIII");
                continue;
            }
            // Attack
            else
                ApplyDamage(c);
        }
    }

    private void ApplyDamage(Collider col)
    {
        Debug.Log($"I've hit the {col.name}");
        col.SendMessageUpwards("SetHealthbarSize", 10f);
    }
}
