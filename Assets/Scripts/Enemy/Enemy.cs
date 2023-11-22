using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour, IDamagable
{
    private Animator myAnimator;

    [field: SerializeField] public float Health { get; set; }

    private NavMeshAgent aiController;

    private int currentTargetPath;
    [SerializeField] private Transform[] wayPoints;
    private Vector3 currentTarget;

    [SerializeField] private Transform Player;

    [SerializeField] private float visualRange;

    [SerializeField] private Rigidbody bullet;
    [SerializeField] private Transform shootPoint; 

    private WaitForSeconds CooldownTimer;
    private enum State
    {
        RETURN_TO_PATROL,
        CHASING_PLAYER,
        PATROLING,
        SHOOTING,
        DEAD
    }

   [SerializeField] private State currentState = State.PATROLING;

    private bool activelyShooting = false;
    private void Start()
    {
        aiController = GetComponent<NavMeshAgent>();
        CooldownTimer = new WaitForSeconds(1);
        Patrol();
    }

    private void Awake()
    {
        myAnimator = GetComponent<Animator>();
    }

    public void Die()
    {
        StopAllCoroutines();
        currentState = State.DEAD;
        Rigidbody[] myRbs = GetComponentsInChildren<Rigidbody>();

        foreach(Rigidbody rb in myRbs)
        {
            rb.isKinematic = false;
        }

        myAnimator.enabled = false;
    }

    private void Update()
    {
        if (currentState != State.DEAD)
        {



            switch (currentState)
            {
                case State.PATROLING:
                    //is the player within sight?
                    if (Vector3.Distance(transform.position, Player.position) < visualRange)
                    {
                        currentState = State.CHASING_PLAYER;//start following the player

                        break;
                    }
                    //has the ai reached its target?
                    if (aiController.remainingDistance <= aiController.stoppingDistance)
                    {
                        print("test");
                        Patrol();//set next destination
                    }
                    break;
                //an in between state, move back to last patrolled point unless interrupted
                case State.RETURN_TO_PATROL:

                    //if reached original patrol path, start patrolling again
                    if (aiController.remainingDistance <= aiController.stoppingDistance)
                    {
                        currentState = State.PATROLING;//patrol again
                        break;
                    }
                    //if the player came back in range
                    if (Vector3.Distance(transform.position, Player.position) < visualRange)
                    {
                        currentState = State.CHASING_PLAYER;//start the chase again
                    }
                    break;
                //following the player
                case State.CHASING_PLAYER:
                    //player moved too far away to follow
                    if (Vector3.Distance(transform.position, Player.position) > visualRange * 2)
                    {
                        currentState = State.RETURN_TO_PATROL;
                        SetTarget(wayPoints[currentTargetPath].position);//go back to the last patrolled point
                        break;
                    }
                    //if the player is wuthin visual range but not close enough to attack, keep moving closer
                    if (Vector3.Distance(transform.position, Player.position) > aiController.stoppingDistance)
                    {
                        SetTarget(Player.position);//move towards the player
                        break;
                    }
                    //if the player is within attack range, stop moving and attack
                    if (Vector3.Distance(transform.position, Player.position) <= aiController.stoppingDistance)
                    {
                        aiController.ResetPath();//stops moving
                        currentState = State.SHOOTING;//starts shooting

                    }

                    break;
                //the attack script
                case State.SHOOTING:
                    //if the player moves out of range again, start chasing again
                    if (Vector3.Distance(transform.position, Player.position) > aiController.stoppingDistance)
                    {
                        currentState = State.CHASING_PLAYER;// chasing state
                                                            //if the enemy was shooting, stop the coroutines so they dont loop infinitely
                        if (activelyShooting)
                        {
                            activelyShooting = false;
                            StopAllCoroutines();
                        }
                        break;
                    }
                    if (!activelyShooting)//if they arent already shooting, start the shooting loop
                    {
                        activelyShooting = true;
                        Shoot();

                    }
                    break;

            }
        }
    }

    private void Patrol()
    {
        currentTargetPath = ++currentTargetPath % wayPoints.Length;

        currentTarget = wayPoints[currentTargetPath].position;
        SetTarget(wayPoints[currentTargetPath].position);
        currentTargetPath++;

       
    }
    public void SetTarget(Vector3 position)
    {
        aiController.SetDestination(position);
    }


    //simple shooting loop with a cooldown
    private void Shoot()
    {
        print("pewpew");

        Rigidbody spawnedProjectile = Instantiate(bullet, shootPoint.position, shootPoint.rotation);

        
        spawnedProjectile.AddForce(transform.forward * 5, ForceMode.Impulse);
        StartCoroutine(ShootCoolDown());
    }

    private IEnumerator ShootCoolDown()
    {
        yield return CooldownTimer;
        Shoot();
    }

}
