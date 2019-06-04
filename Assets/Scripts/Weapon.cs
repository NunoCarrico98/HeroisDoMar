using UnityEngine;
using System.Collections.Generic;

public class Weapon : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private bool isResetAfterHit;
    [Header("Hit VFX")]
    [SerializeField] private GameObject hitVFX;
    [SerializeField] private float hitYOffset;

    public bool BasicAbility { get; set; }
    public bool MovementAbility { get; set; }
    public bool OtherAbility { get; set; }
    public bool UltimateAbility { get; set; }

    public bool[] Abilities { get; set; }
    public float Damage => damage;
    public bool IsAttacking { get; set; }
    public Hero WeaponHolder { get; private set; }

    private VFXManager vfxManager;

    private void Awake()
    {
        Abilities = new bool[4];
        WeaponHolder = GetComponentInParent<Hero>();
        vfxManager = FindObjectOfType<VFXManager>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (IsAttacking)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Arena") && isResetAfterHit)
                WeaponHolder.ResetWeapon();

            else if (other.gameObject.layer == LayerMask.NameToLayer("Hitbox") && other.gameObject.transform != WeaponHolder.transform)
            {
                Debug.Log($"I've hit the {other.gameObject.name}");

                if (isResetAfterHit)
                    WeaponHolder.ResetWeapon();

                if (other.gameObject.tag == "Player" || other.gameObject.tag == "Decoy")
                {
					float damageMultiplier = other.gameObject.GetComponent<Hero>().DamageMultiplier;
					other.gameObject.SendMessage("TakeDamage", new float[] { damage * damageMultiplier, WeaponHolder.PlayerNumber });
					vfxManager.InstantiateVFXWithYOffset(hitVFX, other.transform, 2f, hitYOffset);
                }

                //else if (other.gameObject.tag == "Decoy")
                //    other.gameObject.SendMessage("TakeDamage", new float[] { damage, WeaponHolder.PlayerNumber });

                if (Abilities[0]) WeaponHolder.SendMessage("AfterHitEffectBA", other.transform);
                if (Abilities[1]) WeaponHolder.SendMessage("AfterHitEffectMA", other.transform);
                if (Abilities[2]) WeaponHolder.SendMessage("AfterHitEffectOA", other.transform);
                if (Abilities[3]) WeaponHolder.SendMessage("AfterHitEffectUA", other.transform);

                for (int i = 0; i < Abilities.Length; i++)
                {
                    Abilities[i] = false;
                }
            }
        }
    }

    public float GetWeaponHolderPlayerNumber()
    {
        return WeaponHolder.PlayerNumber;
    }
}
