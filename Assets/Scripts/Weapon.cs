using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private float damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9 && other.transform != transform.root)
        {
            Debug.Log($"I've hit the {other.name}");
            other.SendMessageUpwards("TakeDamage", damage);
        }
    }
}
