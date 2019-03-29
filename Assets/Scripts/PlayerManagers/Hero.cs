using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    [SerializeField] private float maximumHealth;

    private HealthBar healthBar;

    private float currentHealth;

    [Header("0 = Melee | 1 = Range")]
    [SerializeField] private Collider[] attackWeapon;

    // Start is called before the first frame update
    void Start()
    {
        healthBar = GetComponent<HealthBar>();
        healthBar.MaximumHealth = maximumHealth;
    }

    private void Update()
    {
        LaunchAttack();
    }

    private void LaunchAttack()
    {
        // I = Melee attack
        if (Input.GetKeyDown(KeyCode.I))
            Attack(attackWeapon[0]);
        // O = Ranged attack
        if (Input.GetKeyDown(KeyCode.O))
            Attack(attackWeapon[1]);
    }

    private void Attack(Collider col)
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
                continue;
            // Attack
            else
                ApplyDamage(c);
        }
    }

    private void ApplyDamage(Collider col)
    {
        Debug.Log($"I've hit the {col.name}");
        //Hero enemy = col.gameObject.GetComponent<Hero>();
        //enemy.currentHealth -= 10f;
        //enemy.healthBar.SetHealthbarSize(10f);

        col.SendMessageUpwards("SetHealthbarSize", 10f);
    }
}
