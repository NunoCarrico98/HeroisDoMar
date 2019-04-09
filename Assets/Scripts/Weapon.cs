using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private float damage;

    private Hero weaponHolder;

    private void Awake()
    {
        weaponHolder = GetComponentInParent<Hero>();    
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == 9 && other.transform != transform.root)
        {
            Debug.Log($"I've hit the {other.gameObject.name}");

            if (tag == "RangedWeapon")
                weaponHolder.ResetRangedWeapon();

            if (other.gameObject.tag == "Player")
            {
                other.gameObject.SendMessageUpwards("TakeDamage", damage);
            }
        }
    }
}
