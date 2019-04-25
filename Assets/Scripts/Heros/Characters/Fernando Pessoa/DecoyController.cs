using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.AI;
using UnityEngine;

public class DecoyController : MonoBehaviour
{

    [SerializeField] private GameObject explosionEffect;
    [SerializeField] private GameObject vanishEffect;

    private int pNumber;
    private float decoyLifetime;
    private float health;
    private float movementSpeed;
    private float targetRadius;
    private float explosionRadius;
    private float explosionDamage;
    private float secondsForTarget;
    private float timeElapsed;
    private Vector3 lastPosition;
    private Animator charAnimator;
    private NavMeshAgent agent;

    private bool enemyFound;
    private List<GameObject> enemiesList;
    private GameObject nearestEnemy;

    public bool IsMovementAllowed { get; set; }

    private void Awake()
    {
        charAnimator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        timeElapsed = 0;
        IsMovementAllowed = true;
    }

    public void Initialize(int pNumber, float decoyLifetime, float health, float movementSpeed,
        float targetRadius, float explosionRadius, float explosionDamage, float secondsForTarget)
    {
        this.pNumber = pNumber;
        this.decoyLifetime = decoyLifetime;
        this.health = health;
        this.movementSpeed = movementSpeed;
        this.targetRadius = targetRadius;
        this.explosionRadius = explosionRadius;
        this.explosionDamage = explosionDamage;
        this.secondsForTarget = secondsForTarget;

        enemiesList =
            GameObject.FindGameObjectsWithTag("Player").Where(p => p.name != $"Player {pNumber}").ToList();
        agent.speed = movementSpeed;
    }

    private void Update()
    {
        lastPosition = transform.position;
        timeElapsed += Time.deltaTime;

        if (timeElapsed <= secondsForTarget)
        {
            if (IsMovementAllowed)
            {
                PlayRunningAnimation(true);
                agent.Move(transform.forward * movementSpeed * Time.deltaTime);
            }
            else
                PlayRunningAnimation(false);
        }
        else
        {
            if (!enemyFound)
            {
                PlayRunningAnimation(false);
                GameObject tempEnemy = null;

                foreach (GameObject enemy in enemiesList)
                {
                    if (Vector3.Distance(transform.position, enemy.transform.position) <= targetRadius)
                    {
                        if (nearestEnemy == null) nearestEnemy = enemy;
                        tempEnemy = enemy;
                        enemyFound = true;
                    }
                    if (nearestEnemy != null)
                        if (Vector3.Distance(transform.position, tempEnemy.transform.position)
                            < Vector3.Distance(transform.position, nearestEnemy.transform.position))
                            nearestEnemy = tempEnemy;
                }
            }
            else
            {
                if (IsMovementAllowed)
                {
                    PlayRunningAnimation(true);
                    agent.SetDestination(nearestEnemy.transform.position);
                }
                else
                    PlayRunningAnimation(false);
            }
        }

        if (nearestEnemy != null &&
            Vector3.Distance(transform.position, nearestEnemy.transform.position) <= agent.stoppingDistance)
            PlayRunningAnimation(false);

        if (timeElapsed >= decoyLifetime)
            DestroyDecoy(vanishEffect);
    }

    private void PlayRunningAnimation(bool isRunning)
    {
        switch (isRunning)
        {
            case true:
                charAnimator.SetFloat("Velocity", Vector3.Distance(lastPosition, transform.position / Time.deltaTime));
                break;
            case false:
                charAnimator.SetFloat("Velocity", 0);
                break;
        }
    }

    public void TakeDamage(float[] weaponProperties)
    {
        float weaponDamage = weaponProperties[0];
        int weaponHolderNum = (int)weaponProperties[1];

        if (weaponHolderNum == pNumber)
        {
            ApplyAOEDamage();
            DestroyDecoy(explosionEffect);
        }
        else
        {
            health -= weaponDamage;

            if (health <= 0)
            {
                DestroyDecoy(vanishEffect);
            }
        }
    }

    private void ApplyAOEDamage()
    {
        Collider[] affectedEnemies = Physics.OverlapSphere(transform.position, explosionRadius, LayerMask.GetMask("Hitbox"));

        foreach (Collider c in affectedEnemies)
        {
            if (c.name != $"Player {pNumber}" && c.transform != transform)
            {
                c.SendMessageUpwards("TakeDamage", new float[] { explosionDamage, 0 });
            }
        }
    }

    private void DestroyDecoy(GameObject effect)
    {
        Instantiate(effect, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision other)
    {
        Weapon weapon = other.transform.GetComponent<Weapon>();

        if (weapon != null)
        {
            if (weapon.IsAttacking)
            {
                if (weapon.GetWeaponHolderPlayerNumber() == pNumber)
                {
                    Instantiate(explosionEffect, transform.position, transform.rotation);
                    Destroy(gameObject);
                }
                else
                {
                    Instantiate(vanishEffect, transform.position, transform.rotation);
                    Destroy(gameObject);
                }
            }
        }
    }

    public void AllowMovement(bool movement)
    {
        IsMovementAllowed = movement;
    }
}
