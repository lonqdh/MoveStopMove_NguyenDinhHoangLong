using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bot : Character
{
    private IState<Bot> currentState;
    private int botLayerNumber = 3;
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

        if (!IsDead && GameManager.Instance.IsState(GameState.Gameplay))
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

    
    private void GetRandomWeapon()
    {
        //int randWeapIndex = Random.Range(0, weaponDataSO.weaponDataList.Count);
        int randWeapIndex = Random.Range(0, DataManager.Instance.weaponDataSO.weaponDataList.Capacity);
        weaponData = DataManager.Instance.weaponDataSO.weaponDataList[randWeapIndex];

        if (weaponInstance == null)
        {
            weaponInstance = Instantiate(weaponData.weapon, weaponHoldingPos.position, weaponHoldingPos.rotation);
            attackRange = weaponData.autoAttackRange;
        }
        else
        {
            Destroy(weaponInstance.gameObject);
            weaponInstance = Instantiate(weaponData.weapon, weaponHoldingPos.position, weaponHoldingPos.rotation);
            attackRange = weaponData.autoAttackRange;
        }

        weaponInstance.transform.parent = weaponHoldingPos;
    }

    private void DetectEnemies()
    {
        // Check for enemies within the autoAttackRange
        Collider[] hitColliders = new Collider[10];

        int numEnemies = Physics.OverlapSphereNonAlloc(transform.position, attackRange, hitColliders, enemyLayer);

        if (IsDead)
        {
            return;
        }

        if (numEnemies > 0 && !isAttacking)
        {
            float closestDistance = attackRange + 1;
            Transform nearestEnemy = null;

            for (int i = 0; i < numEnemies; i++)
            {
                if(charCollider == hitColliders[i])
                {
                    continue;
                }
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

    protected override void Attack(Transform target)
    {
        base.Attack(target);
        StartCoroutine(ResumePatrolling());
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
        //base.OnInit();
        isAttacking = false;
        IsDead = false;
        this.gameObject.layer = botLayerNumber;
        
        if (weaponData != null)
        {
            GetRandomWeapon();
        }
        if (weaponData.bullet != null)
        {
            bulletPrefab = weaponData.bullet;
        }

        ChangeState(new PatrolState());

        
}

private void OnDespawn()
    {
        //LevelManager.Instance.DespawnBot(this);
        //Transform spawnPoint = GetRandomSpawnPoint();

        //if (spawnPoint != null)
        //{
        //    LevelManager.Instance.SpawnSingleBot(spawnPoint);
        //}
        //else
        //{
        //    Debug.LogError("No valid spawn points available.");
        //}

        LevelManager.Instance.DespawnBot(this);
        if(LevelManager.Instance.totalPlayers > 25)
        {
            LevelManager.Instance.SpawnSingleBot();
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

    }

    //private Transform GetRandomSpawnPoint()
    //{
    //    List<Transform> spawnPoints = LevelManager.Instance.botSpawnPointList;

    //    if (spawnPoints.Count > 0)
    //    {
    //        int randomIndex = Random.Range(0, spawnPoints.Count);
    //        return spawnPoints[randomIndex];
    //    }
    //    else
    //    {
    //        return null;
    //    }
    //}

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
