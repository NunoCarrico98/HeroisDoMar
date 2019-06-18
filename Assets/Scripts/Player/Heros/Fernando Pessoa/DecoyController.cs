using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.AI;
using UnityEngine;

public class DecoyController : MonoBehaviour
{
    [SerializeField] private GameObject explosionVFX;
    [SerializeField] private float explosionYOffset;
    [SerializeField] private GameObject vanishVFX;
    [SerializeField] private AudioClip explosionSFX;
    [SerializeField] private AudioClip vanishSFX;

    private int pNumber;
    private float decoyLifetime;
    private float health;
    private float movementSpeed;
    private float targetRadius;
    private float explosionRadius;
    private float explosionDamage;
    private float secondsForAction;
    private float numberOfDecoys;
    private float timeElapsed;
    private Vector3 lastPosition;
    private Animator charAnimator;
    private NavMeshAgent agent;

    private bool enemyFound;
    private List<Hero> enemiesList;
    private List<DecoyController> allyDecoys;
    private Hero nearestEnemy;

    private VFXManager vfxManager;

    public DecoyType type;
    public int PlayerNumber => pNumber;
    public bool IsMovementAllowed { get; set; }
    public bool IsSlowed { get; set; }

    private void Awake()
    {
        charAnimator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        timeElapsed = 0;
        IsMovementAllowed = true;
    }

    public void InitializeDecoy(int pNumber, float decoyLifetime, float health, float movementSpeed,
        float explosionRadius, float explosionDamage, VFXManager vfxManager, DecoyType type,
        float secondsForTarget = 0, float targetRadius = 0, int numberOfDecoys = 0)
    {
        this.pNumber = pNumber;
        this.decoyLifetime = decoyLifetime;
        this.health = health;
        this.movementSpeed = movementSpeed;
        this.targetRadius = targetRadius;
        this.explosionRadius = explosionRadius;
        this.explosionDamage = explosionDamage;
        this.secondsForAction = secondsForTarget;
        this.numberOfDecoys = numberOfDecoys;
        this.vfxManager = vfxManager;

        this.type = type;

        if (type == DecoyType.MovementDecoy)
            enemiesList = FindObjectsOfType<Hero>().Where(p => p.PlayerNumber != pNumber).ToList();

        agent.speed = movementSpeed;
    }

    // Gonna need this in the future for Domino Blow Up
    public void FindAllyDecoys()
    {
        allyDecoys = FindObjectsOfType<DecoyController>().
            Where(d => d.PlayerNumber == PlayerNumber && d.transform != transform).ToList();
    }

    private void Update()
    {
        if (type == DecoyType.MovementDecoy)
            MovementAbility();
        else if (type == DecoyType.UltimateDecoy)
            UltimateAbility();
    }

    private void MovementAbility()
    {
        lastPosition = transform.position;
        timeElapsed += Time.deltaTime;

        if (timeElapsed <= secondsForAction)
        {
            if (IsMovementAllowed)
            {
                float movement = (IsSlowed) ? movementSpeed / 2 : movementSpeed;
                PlayRunningAnimation(true);
                agent.Move(transform.forward * movement * Time.deltaTime);
            }
            else
                PlayRunningAnimation(false);
        }
        else
        {
            if (!enemyFound)
            {
                PlayRunningAnimation(false);
                Hero tempEnemy = null;

                foreach (Hero enemy in enemiesList)
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
                    agent.speed = (IsSlowed) ? movementSpeed / 2 : movementSpeed;
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
            DestroyDecoy(vanishVFX, vanishSFX);
    }

    public void WarpTo(Vector3 position)
    {
        agent.Warp(position);
    }

    private void UltimateAbility()
    {
        if (timeElapsed < decoyLifetime)
        {
            if (IsMovementAllowed)
            {
                float movement = (IsSlowed) ? movementSpeed / 2 : movementSpeed;
                PlayRunningAnimation(true);
                agent.Move(transform.forward * movement * Time.deltaTime);
            }
            else
                PlayRunningAnimation(false);
        }
        else
        {
            ApplyAOEDamage();
            DestroyDecoy(explosionVFX, explosionSFX);
        }

        timeElapsed += Time.deltaTime;
    }

    public void TakeDamage(float[] weaponProperties)
    {
        float weaponDamage = weaponProperties[0];
        int weaponHolderNum = (int)weaponProperties[1];

        if (type == DecoyType.MovementDecoy)
        {
            if (weaponHolderNum == pNumber)
            {
                ApplyAOEDamage();
                DestroyDecoy(explosionVFX, explosionSFX);
            }
            else
            {
                health -= weaponDamage;

                if (health <= 0)
                {
                    DestroyDecoy(vanishVFX, vanishSFX);
                }
            }
        }
        else if (type == DecoyType.UltimateDecoy)
        {
            if (weaponHolderNum == pNumber)
            {
                ApplyAOEDamage();
                DestroyDecoy(explosionVFX, explosionSFX);
            }
            else
            {
                health -= weaponDamage;

                if (health <= 0)
                {
                    ApplyAOEDamage();
                    DestroyDecoy(explosionVFX, explosionSFX);
                }
            }
        }
    }

    public void Suicide()
    {
        ApplyAOEDamage();
        DestroyDecoy(explosionVFX, explosionSFX);
    }

    public void ApplyAOEDamage()
    {
        Collider[] affectedEnemies = Physics.OverlapSphere(transform.position, explosionRadius, LayerMask.GetMask("Hitbox"));

        foreach (Collider c in affectedEnemies)
        {
            // THIS ONLY AFFECTS HEROES FOR NOW.
            Hero enemy = c.GetComponent<Hero>();
            if (enemy != null)
                if (enemy.PlayerNumber != pNumber && c.transform != transform)
                {
                    c.SendMessageUpwards("TakeDamage", new float[] { explosionDamage * enemy.DamageMultiplier, 0 });
                }
        }
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

    public void DestroyDecoy(GameObject vfx, AudioClip sfx)
    {
        if (sfx != null)
            SoundManager.Instance.PlaySFX(sfx);
        vfxManager.InstantiateVFXWithYOffset(vfx, transform, 5f, explosionYOffset);
        //Instantiate(vfx, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    public void AllowMovement(bool movement)
    {
        IsMovementAllowed = movement;
    }

    public void SlowDown(bool condition)
    {
        IsSlowed = condition;
    }

    public enum DecoyType
    {
        MovementDecoy,
        UltimateDecoy
    }
}
