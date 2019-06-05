using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    private bool hasBeenInitialized;
    private bool timerStarted;
    private float damage;
    private float damageInterval;
    private float damageDuration;
    private float lastDamage;
    private int attackerNumber;

    private void Awake()
    {
        hasBeenInitialized = false;
        timerStarted = false;
    }

    public void Initialize(float damage, float damageInterval, float damageDuration, int attackerNumber)
    {
        this.damage = damage;
        this.damageInterval = damageInterval;
        this.damageDuration = damageDuration;
        this.attackerNumber = attackerNumber;
        hasBeenInitialized = true;
        lastDamage = 0;
    }

    private void FixedUpdate()
    {
        if (hasBeenInitialized)
        {
            if (!timerStarted)
            {
                StartCoroutine(Timer());
            }

            if (lastDamage >= damageInterval)
            {
                lastDamage = 0;
            }

            lastDamage += Time.fixedDeltaTime;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        int enemyPlayerNumber = 0;
        if (other.GetComponent<Hero>() != null)
        {
            enemyPlayerNumber = other.GetComponent<Hero>().PlayerNumber;
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Hitbox") && enemyPlayerNumber != attackerNumber)
        {
            if (lastDamage >= damageInterval)
            {
                other.gameObject.SendMessage("TakeDamage", new float[] { damage, 0 });
            }
        }
    }

    private IEnumerator Timer()
    {
        float timeElapsed = 0;

        while (timeElapsed < damageDuration)
        {
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
