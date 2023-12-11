using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bot : Character
{
    private IState<Bot> currentState;
    //private int botLayerNumber = 3;
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
        int randWeapIndex = Random.Range(0, DataManager.Instance.weaponDataSO.weaponDataList.Capacity);
        weaponData = DataManager.Instance.weaponDataSO.weaponDataList[randWeapIndex];

        if (weaponInstance == null)
        {
            weaponInstance = Instantiate(weaponData.weapon, weaponHoldingPos.position, weaponHoldingPos.rotation);
        }
        else
        {
            Destroy(weaponInstance.gameObject);
            weaponInstance = Instantiate(weaponData.weapon, weaponHoldingPos.position, weaponHoldingPos.rotation);
            attackRange = weaponData.autoAttackRange;
        }
        SetAttackRange();
        SetAttackSpeed();

        weaponInstance.transform.parent = weaponHoldingPos;
    }

    private void GetRandomHat()
    {
        int randHatIndex = Random.Range(0, DataManager.Instance.hatDataSO.hatDataList.Capacity);
        hatData = DataManager.Instance.hatDataSO.hatDataList[randHatIndex];

        if (hatInstance == null)
        {
            hatInstance = Instantiate(hatData.hatPrefab, hatPos);
        }
        else
        {
            Destroy(hatInstance.gameObject);
            hatInstance = Instantiate(hatData.hatPrefab, hatPos);
        }

        SetAttackRange();
        //hatInstance.transform.parent = hatPos;
    }

    private void GetRandomPant()
    {
        int randPantIndex = Random.Range(0, DataManager.Instance.pantDataSO.pantDataList.Capacity);
        pantData = DataManager.Instance.pantDataSO.pantDataList[randPantIndex];

        if (pantInstance == null)
        {
            pantInstance.material = pantData.PantMaterial;
        }
        else
        {
            pantInstance.material = pantData.PantMaterial;
        }
        SetCharMoveSpeed();
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
                if (charCollider == hitColliders[i])
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
                ChangeAnim(Constant.ANIM_IDLE);

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
        isAttacking = false;
        IsDead = false;
        this.gameObject.layer = charLayerNumber;

        if (weaponData != null)
        {
            GetRandomWeapon();
        }
        if (weaponData.bullet != null)
        {
            bulletPrefab = weaponData.bullet;
        }
        if (hatData != null)
        {
            GetRandomHat();
        }
        if (pantData != null)
        {
            GetRandomPant();
        }

        ScaleAttackRangeCircle();

        ChangeState(new PatrolState());


    }

    private void OnDespawn()
    {
        LevelManager.Instance.DespawnBot(this);
        if (LevelManager.Instance.totalPlayers > 25)
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
        ChangeAnim(Constant.ANIM_DIE);
        Invoke(nameof(OnDespawn), 2f);

    }

    public void ChangeState(IState<Bot> state)
    {
        if (currentState != null)
        {
            currentState.OnExit(this);
        }

        currentState = state;

        if (currentState != null)
        {
            currentState.OnEnter(this);
        }
    }
}
