using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private float damage;

    public bool IsAttacking { get; set; }
    public Hero WeaponHolder { get; private set; }

    private void Awake()
    {
        WeaponHolder = GetComponentInParent<Hero>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == 9 && other.gameObject.transform != WeaponHolder.transform)
        {
            Debug.Log($"I've hit the {other.gameObject.name}");

            if (tag == "RangedWeapon")
            {
                WeaponHolder.ResetWeapon();
            }

            if (other.gameObject.tag == "Player")
            {
                other.gameObject.SendMessageUpwards("TakeDamage", damage);
            }
        }
    }

    public float GetWeaponHolderPlayerNumber()
    {
        return WeaponHolder.PlayerNumber;
    }
}
