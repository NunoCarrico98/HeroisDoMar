using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DecoyController : MonoBehaviour
{
    private int pNumber;
    private float maximumHp;
    private float movementSpeed;
    private float turnSpeed;
    private float secondsForTarget;
    private float timeElapsed;
    private Vector3 lastPosition;
    private Animator charAnimator;
    private CharacterController charController;

    private float nearestEnemyDistance;
    private List<GameObject> enemiesList;
    private GameObject nearestEnemy;
    private Vector3 targetDirection;

    public void Initialize(int pNumber, float maximumHp, float movementSpeed, float turnSpeed, float secondsForTarget)
    {
        this.pNumber = pNumber;
        this.maximumHp = maximumHp;
        this.movementSpeed = movementSpeed;
        this.turnSpeed = turnSpeed;
        this.secondsForTarget = secondsForTarget;

        enemiesList =
            GameObject.FindGameObjectsWithTag("Player").Where(p => p.name != $"Player {pNumber}").ToList();
        nearestEnemy = enemiesList[0];
        nearestEnemyDistance = Vector3.Distance(transform.position, nearestEnemy.transform.position);
    }

    private void Awake()
    {
        charAnimator = GetComponentInChildren<Animator>();
        charController = GetComponent<CharacterController>();
        timeElapsed = 0;
    }

    private void FixedUpdate()
    {
        lastPosition = transform.position;
        timeElapsed += Time.deltaTime;

        if (timeElapsed <= secondsForTarget)
        {
            charController.Move(transform.forward * movementSpeed * Time.fixedDeltaTime);

            if (enemiesList.Count > 1)
                foreach (GameObject go in enemiesList)
                {
                    if (Vector3.Distance(transform.position, go.transform.position) 
                        <= Vector3.Distance(transform.position, nearestEnemy.transform.position))
                    {
                        nearestEnemy = go;
                    }
                }

            targetDirection = nearestEnemy.transform.position - transform.position;
            targetDirection.y = 0f;
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targetDirection), Time.fixedDeltaTime * turnSpeed);
            transform.position = Vector3.MoveTowards(transform.position, nearestEnemy.transform.position, Time.fixedDeltaTime * movementSpeed);
        }

        charAnimator.SetFloat("Velocity", Vector3.Distance(lastPosition, transform.position / Time.fixedDeltaTime));
        Debug.Log(charAnimator.parameters[0]);
    }
}
