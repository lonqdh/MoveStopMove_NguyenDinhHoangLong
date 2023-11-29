using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bot : Character
{
    private IState<Bot> currentState;

    public NavMeshAgent agent;

    public Vector3 walkPoint;
    public bool walkPointSet;
    public float walkPointRange;

    public bool isAttacking;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        OnInit();
    }

    protected override void Update()
    {
        if (!IsDead)
        {
            if (currentState != null)
            {
                currentState.OnExecute(this);
            }

            DetectEnemies();
        }

        //if (!IsDead)
        //{
        //    DetectEnemies();
        //}
    }

    private void DetectEnemies()
    {
        // Check for enemies within the autoAttackRange
        Collider[] hitColliders = new Collider[10];
        int numEnemies = Physics.OverlapSphereNonAlloc(transform.position, weaponData.autoAttackRange, hitColliders, enemyLayer);

        if (IsDead)
        {
            Debug.Log("Is Dead Cant Detect");
            return;
        }

        if (numEnemies > 0 && !isAttacking)
        {
            Debug.Log("Found enemies!");

            float closestDistance = float.MaxValue;
            Transform nearestEnemy = null;

            for (int i = 0; i < numEnemies; i++)
            {
                float distance = Vector3.Distance(transform.position, hitColliders[i].transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    nearestEnemy = hitColliders[i].transform;
                }
            }

            if (nearestEnemy != null)
            {
                // Stop moving
                agent.isStopped = true;
                agent.SetDestination(transform.position);
                ChangeAnim("IsIdle");

                Vector3 direction = nearestEnemy.position - throwPoint.position;
                direction.Normalize();

                transform.rotation = Quaternion.LookRotation(direction);

                // Trigger attack
                Attack(nearestEnemy);
            }
        }
    }

    public void Attack(Transform target)
    {
        float cooldownDuration = 2f;

        // Check if enough time has passed since the last attack
        if (Time.time - lastAutoAttackTime >= cooldownDuration)
        {
            ChangeAnim("IsAttack");

            Bullet bullet = LeanPool.Spawn(weaponData.bullet, throwPoint.position, throwPoint.rotation);
            //bullet.attacker = this;
            bullet.OnInit(this);
            //bullet.OnInit(growthFactor); // Pass the growth factor of the
            bullet.GetComponent<Rigidbody>().velocity = (target.position - throwPoint.position).normalized * 5f;

            // Set the cooldown timer
            lastAutoAttackTime = Time.time;

            isAttacking = true;

            StartCoroutine(ResumePatrolling());
        }
    }

    private IEnumerator ResumePatrolling()
    {
        yield return new WaitForSeconds(2f);

        // Resume patrolling
        isAttacking = false;
        if (!IsDead)
        {
            agent.isStopped = false;
            ChangeState(new PatrolState());
        }

    }

    internal override void OnInit()
    {
        base.OnInit();
        isAttacking = false;
        IsDead = false;
        gameObject.layer = 7;
        ChangeState(new PatrolState());

    }

    private void OnDespawn()
    {
        LevelManager.Instance.DespawnBots(this);
    }

    protected override void OnHit()
    {
        this.gameObject.layer = 0;
        agent.isStopped = true;
        IsDead = true;
        ChangeAnim("IsDead");
        Invoke(nameof(OnDespawn), 2f);

        Transform spawnPoint = GetRandomSpawnPoint();

        if (spawnPoint != null)
        {
            LevelManager.Instance.SpawnBot(spawnPoint);
        }
        else
        {
            Debug.LogError("No valid spawn points available.");
        }
    }

    private Transform GetRandomSpawnPoint()
    {
        List<Transform> spawnPoints = LevelManager.Instance.botSpawnPointList;

        // If there are available spawn points, use one of them
        if (spawnPoints.Count > 0)
        {
            int randomIndex = Random.Range(0, spawnPoints.Count);
            return spawnPoints[randomIndex];
        }
        else
        {
            return null;
        }
    }

    public void ChangeState(IState<Bot> state)
    {
        if (currentState != null)
        {
            currentState.OnExit(this);
        }

        currentState = state;
        //IsDead = false;
        //isAttacking = false;

        if (currentState != null)
        {
            currentState.OnEnter(this);
        }
    }
}
