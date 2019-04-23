﻿using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private bool isResetAfterHit;

    public float Damage => damage;
    public float ExtraDamage { get; set; }
    public bool IsAttacking { get; set; }
    public Hero WeaponHolder { get; private set; }

    private void Awake()
    {
        ExtraDamage = 0;

        WeaponHolder = GetComponentInParent<Hero>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (IsAttacking)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Arena") && isResetAfterHit)
            {
                WeaponHolder.ResetWeapon();
            }
            else if (other.gameObject.layer == LayerMask.NameToLayer("Hitbox") && other.gameObject.transform != WeaponHolder.transform)
            {
                float damageApplied = (ExtraDamage != 0) ? damage + ExtraDamage : damage;

                Debug.Log($"I've hit the {other.gameObject.name}");

                if (isResetAfterHit)
                    WeaponHolder.ResetWeapon();

                if (other.gameObject.tag == "Player")
                    other.gameObject.GetComponent<Hero>().TakeDamage(damageApplied);

                else if (other.gameObject.tag == "Decoy")
                    other.gameObject.SendMessage("TakeDamage", new float[] { WeaponHolder.PlayerNumber, damageApplied });

                ExtraDamage = 0;
            }
        }
    }

    public float GetWeaponHolderPlayerNumber()
    {
        return WeaponHolder.PlayerNumber;
    }
}
