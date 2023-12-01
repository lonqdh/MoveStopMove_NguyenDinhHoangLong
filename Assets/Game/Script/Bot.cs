using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bot : Character
{
    private IState<Bot> currentState;
    private int botLayerNumber = 7;
    //private Bullet bullet;
    
    public NavMeshAgent agent;
    public Vector3 walkPoint;
    public bool walkPointSet;
    public float walkPointRange;
    public bool isAttacking;
    public GameObject targetCircle;


    private void Awake()
    {

    }

    private void Start()
    {
        OnInit();
    }

    protected override void Update()
    {

        if (!IsDead)
        {
            //base.Update();
            if (currentState != null)
            {
                currentState.OnExecute(this);
            }

            //thang duoi nay de freeze bot until vao game
            //if (currentState != null && GameManager.Instance.IsState(GameState.Gameplay))
            //{
            //    currentState.OnExecute(this);
            //}

            DetectEnemies();
        }
    }

    private void DetectEnemies()
    {
        // Check for enemies within the autoAttackRange
        Collider[] hitColliders = new Collider[10];
        int numEnemies = Physics.OverlapSphereNonAlloc(transform.position, weaponData.autoAttackRange, hitColliders, enemyLayer);

        if (IsDead)
        {
            return;
        }

        if (numEnemies > 0 && !isAttacking)
        {
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
                //agent.SetDestination(transform.position); // dung cai nay doc lap thi se bi loi bot dung yen lai ban xong di chuyen luon, va dung cai nay se loi khi nvat dung lai se slide di
                ChangeAnim("IsIdle");

                Vector3 direction = nearestEnemy.position - throwPoint.position;
                direction.Normalize();

                transform.rotation = Quaternion.LookRotation(direction);


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
            bullet.bulletRigidbody.velocity = (target.position - throwPoint.position).normalized * 5f;

            // Set the cooldown timer
            lastAutoAttackTime = Time.time;

            isAttacking = true;

            StartCoroutine(ResumePatrolling());
        }
    }

    private IEnumerator ResumePatrolling()
    {
        yield return new WaitForSeconds(2f);

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
        this.gameObject.layer = botLayerNumber;
        ChangeState(new PatrolState());

    }

    private void OnDespawn()
    {
        LevelManager.Instance.DespawnBot(this);
        Transform spawnPoint = GetRandomSpawnPoint();

        if (spawnPoint != null)
        {
            LevelManager.Instance.SpawnSingleBot(spawnPoint);
        }
        else
        {
            Debug.LogError("No valid spawn points available.");
        }
    }

    protected override void OnHit()
    {
        this.targetCircle.SetActive(false);
        this.gameObject.layer = defaultLayerNumber;
        agent.isStopped = true;
        IsDead = true;
        ChangeAnim("IsDead");
        Invoke(nameof(OnDespawn), 2f);

        //Transform spawnPoint = GetRandomSpawnPoint();

        //if (spawnPoint != null)
        //{
        //    LevelManager.Instance.SpawnSingleBot(spawnPoint);
        //}
        //else
        //{
        //    Debug.LogError("No valid spawn points available.");
        //}
    }

    private Transform GetRandomSpawnPoint()
    {
        List<Transform> spawnPoints = LevelManager.Instance.botSpawnPointList;

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
