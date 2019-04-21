using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private float damage;

    public bool IsAttacking { get; set; }
    public float Damage => damage;
    public Hero WeaponHolder { get; private set; }

    private void Awake()
    {
        WeaponHolder = GetComponentInParent<Hero>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (IsAttacking)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Hitbox") && other.gameObject.transform != WeaponHolder.transform)
            {
                Debug.Log($"I've hit the {other.gameObject.name}");

                WeaponHolder.ResetWeapon();

                if (other.gameObject.tag == "Player")
                    other.gameObject.GetComponent<Hero>().TakeDamage(damage);
                else if (other.gameObject.tag == "Decoy")
                    other.gameObject.SendMessage("TakeDamage", new float[] { WeaponHolder.PlayerNumber, damage });
            }
        }
    }

    public float GetWeaponHolderPlayerNumber()
    {
        return WeaponHolder.PlayerNumber;
    }
}
