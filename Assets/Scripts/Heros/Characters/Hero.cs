using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Hero : MonoBehaviour
{
    [SerializeField] private float maximumHealth;

    private HealthBar healthBar;

    private float currentHealth;

    // Input
    private bool isL1InUse;
    private bool isL2InUse;
    private bool isR1InUse;
    private bool isR2InUse;

    [Header("Weapons")]
    [SerializeField] protected Collider meleeWeapon;
    [SerializeField] protected Collider rangeWeapon;

    protected abstract void RangeAttack();
    protected abstract void MeleeAttack();
    protected abstract void RegularAbility();
    protected abstract void UltimateAbility();

    // Start is called before the first frame update
    void Start()
    {
        healthBar = GetComponent<HealthBar>();
        healthBar.MaximumHealth = maximumHealth;

        isL1InUse = false;
        isL2InUse = false;
        isR1InUse = false;
        isR2InUse = false;
    }

    private void Update()
    {
        LaunchAttack();
    }

    private void LaunchAttack()
    {
        if (Input.GetAxis("Melee") != 0)
        {
            if (!isR1InUse)
            {
                Debug.Log("Melee");
                MeleeAttack();
                isR1InUse = true;
            }
        }
        else
            isR1InUse = false;

        if (Input.GetAxis("Range") != 0)
        {
            if (!isL1InUse)
            {
                Debug.Log("Range");
                RangeAttack();
                isL1InUse = true;
            }
        }
        else
            isL1InUse = false;
    }

    protected void Attack(Collider col)
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
