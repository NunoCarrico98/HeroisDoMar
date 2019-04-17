using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.AI;
using UnityEngine;

public class DecoyController : MonoBehaviour
{
    private int pNumber;
    private float decoyLifetime;
    private float maximumHp;
    private float movementSpeed;
    private float targetRadius;
    private float secondsForTarget;
    private float timeElapsed;
    private Vector3 lastPosition;
    private Animator charAnimator;
    private NavMeshAgent agent;

    private bool enemyFound;
    private List<GameObject> enemiesList;
    private GameObject nearestEnemy;

    private void Awake()
    {
        charAnimator = GetComponentInChildren<Animator>();
        agent = GetComponent<NavMeshAgent>();
        timeElapsed = 0;
    }

    public void Initialize(int pNumber, float decoyLifetime, float maximumHp, float movementSpeed, float targetRadius, float secondsForTarget)
    {
        this.pNumber = pNumber;
        this.decoyLifetime = decoyLifetime;
        this.maximumHp = maximumHp;
        this.movementSpeed = movementSpeed;
        this.targetRadius = targetRadius;
        this.secondsForTarget = secondsForTarget;

        enemiesList =
            GameObject.FindGameObjectsWithTag("Player").Where(p => p.name != $"Player {pNumber}").ToList();
        agent.speed = movementSpeed;
    }

    private void FixedUpdate()
    {
        lastPosition = transform.position;
        timeElapsed += Time.deltaTime;

        if (timeElapsed <= secondsForTarget)
        {
            PlayRunningAnimation(true);
            agent.Move(transform.forward * movementSpeed * Time.fixedDeltaTime);
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
                PlayRunningAnimation(true);
                agent.SetDestination(nearestEnemy.transform.position);
            }
        }

        if (nearestEnemy != null &&
            Vector3.Distance(transform.position, nearestEnemy.transform.position) <= agent.stoppingDistance)
            PlayRunningAnimation(false);
    }

    private void PlayRunningAnimation(bool isRunning)
    {
        switch (isRunning)
        {
            case true:
                charAnimator.SetFloat("Velocity", Vector3.Distance(lastPosition, transform.position / Time.fixedDeltaTime));
                break;
            case false:
                charAnimator.SetFloat("Velocity", 0);
                break;
        }
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
                    Debug.Log("Exploded");
                }
                else
                {
                    Debug.Log("Simply vanished!");
                }
            }
        }
    }
}
