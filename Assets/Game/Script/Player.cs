using Lean.Pool;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.UIElements;

public class Player : Character
{
    [SerializeField] private FloatingJoystick joystick;
    [SerializeField] private Rigidbody rigidbody;
    private Vector3 moveVector;
    private bool hasInit;
    //private List<Collider> hitColliders = new List<Collider>();
    public GameObject attackRangeCircle;

    //public Transform nearestEnemy;

    private void Start()
    {
        if (GameManager.Instance.IsState(GameState.Gameplay))
        {
            OnInit();
        }

        //rigidbody = GetComponent<Rigidbody>();
    }

    protected override void Update() //sau neu lam update o character thi phai override va goi base.Update neu co chuc nang ca bot va nguoi lam
    {
        if (hasInit && IsDead == false)
        {
            //base.Update();
            Move();
            DetectEnemies();
        }
    }

    internal override void OnInit()
    {
        base.OnInit();
        ScaleAttackRangeCircle();
        joystick = LevelManager.Instance.joystick;
        CameraFollow.Instance.target = transform;
        hasInit = true;
    }

    private void Move()
    {
        float checkZ = joystick.Vertical * moveSpeed;

        moveVector = Vector3.zero;
        moveVector.x = joystick.Horizontal * rotateSpeed;
        moveVector.z = joystick.Vertical * moveSpeed;


        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
        {
            Vector3 direction = Vector3.RotateTowards(transform.forward, moveVector, rotateSpeed * Time.deltaTime, 0.0f);
            transform.rotation = Quaternion.LookRotation(direction);

            // Allow forward movement
            rigidbody.velocity = new Vector3(moveVector.x, rigidbody.velocity.y, moveVector.z);
            ChangeAnim("Run");

        }
        else if (moveVector == Vector3.zero)
        {
            rigidbody.velocity = Vector3.zero;
            ChangeAnim("IsIdle");
        }
    }

    //private void DetectEnemies()
    //{

    //    // Check for enemies within the autoAttackRange
    //    Collider[] hitColliders = new Collider[10];
    //    int numEnemies = Physics.OverlapSphereNonAlloc(transform.position, weaponData.autoAttackRange, hitColliders, enemyLayer);

    //    if (this.IsDead)
    //    {
    //        Debug.Log("Dead cant detect");
    //        return;
    //    }

    //    if (numEnemies > 0)
    //    {
    //        float closestDistance = 100f;

    //        for (int i = 0; i < numEnemies; i++)
    //        {
    //            if (charCollider != hitColliders[i])
    //            {
    //                float distance = Vector3.Distance(transform.position, hitColliders[i].transform.position);
    //                //hitColliders[i].GetComponent<Bot>().targetCircle.SetActive(true);

    //                if (distance < closestDistance)
    //                {
    //                    closestDistance = distance;
    //                    nearestEnemy = hitColliders[i].transform;
    //                }
    //                else
    //                {
    //                    nearestEnemy = null;
    //                }
    //            }
    //        }

    //        if (nearestEnemy != null)
    //        {
    //            //Debug.Log("Chay Detect");
    //            //ChangeAnim("IsIdle");

    //            //Vector3 direction = nearestEnemy.position - throwPoint.position;
    //            //direction.Normalize();

    //            //transform.rotation = Quaternion.LookRotation(direction);

    //            //nearestEnemy.GetComponent<Bot>().targetCircle.SetActive(true);

    //            Attack(nearestEnemy);
    //        }
    //    }
    //}


    private void DetectEnemies()
    {
        if (IsDead)
        {
            Debug.Log("Dead cant detect");
            return;
        }

        float closestDistance = attackRange + 1; // Initialize with a value greater than attack range
        Transform nearestEnemy = null;

        foreach (Bot bot in LevelManager.Instance.bots)
        {
            if (bot != null && !bot.IsDead)
            {
                float distance = Vector3.Distance(transform.position, bot.transform.position);


                if (distance < attackRange)
                {
                    bot.targetCircle.SetActive(true);
                }
                else
                {
                    bot.targetCircle.SetActive(false);
                }

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    nearestEnemy = bot.transform;
                }
            }
        }

        if (nearestEnemy != null && moveVector == Vector3.zero)
        {
            Debug.Log("Found a nearest enemy!");
            Attack(nearestEnemy);
        }
    }

    public void Attack(Transform target)
    {
        float cooldownDuration = 2f;

        // Check if enough time has passed since the last attack
        if (Time.time - lastAutoAttackTime >= cooldownDuration)
        {
            Vector3 direction = target.position - throwPoint.position;

            transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));

            ChangeAnim("IsAttack");

            Bullet bullet = LeanPool.Spawn(weaponData.bullet, throwPoint.position, throwPoint.rotation);
            //bullet.attacker = this;
            bullet.OnInit(this);
            bullet.bulletRigidbody.velocity = direction.normalized * 5f;

            // Set the cooldown timer
            lastAutoAttackTime = Time.time;
        }
    }

    private void ScaleAttackRangeCircle()
    {
        if (attackRangeCircle != null)
        {
            float circleScale = attackRange - 2;
            attackRangeCircle.transform.localScale = new Vector3(circleScale, circleScale, 1f);
        }
    }

}