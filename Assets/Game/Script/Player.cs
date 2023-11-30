using Lean.Pool;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private FloatingJoystick joystick;
    [SerializeField] private Rigidbody rigidbody;
    private Vector3 moveVector;
    private bool hasInit;

    public GameObject attackRangeCircle;

    private void Start()
    {
        OnInit();
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

    private void DetectEnemies()
    {
        
        // Check for enemies within the autoAttackRange
        Collider[] hitColliders = new Collider[10];
        int numEnemies = Physics.OverlapSphereNonAlloc(transform.position, weaponData.autoAttackRange, hitColliders, enemyLayer);

        if (IsDead)
        {
            return;
        }

        //if (numEnemies > 0)
        //{
        //    float closestDistance = 100f;
        //    Transform nearestEnemy = null;


        //    for (int i = 0; i < numEnemies; i++)
        //    {
        //        float distance = Vector3.Distance(transform.position, hitColliders[i].transform.position);
        //        hitColliders[i].GetComponent<Bot>().targetCircle.SetActive(true);

        //        if (distance < closestDistance)
        //        {
        //            closestDistance = distance;
        //            nearestEnemy = hitColliders[i].transform;
        //        }
        //    }

        //    if (nearestEnemy != null)
        //    {
        //        //Debug.Log("Chay Detect");
        //        //ChangeAnim("IsIdle");

        //        //Vector3 direction = nearestEnemy.position - throwPoint.position;
        //        //direction.Normalize();

        //        //transform.rotation = Quaternion.LookRotation(direction);

        //        //nearestEnemy.GetComponent<Bot>().targetCircle.SetActive(true);

        //        Attack(nearestEnemy);

        //    }
        //}

        if (numEnemies > 0)
        {
            float closestDistance = 100f;
            Transform nearestEnemy = null;

            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider != null)
                {
                    float distance = Vector3.Distance(transform.position, hitCollider.transform.position);

                    // Check if the enemy is within the player's attack range
                    if (distance < weaponData.autoAttackRange)
                    {
                        hitCollider.GetComponent<Bot>().targetCircle.SetActive(true);
                    }
                    else
                    {
                        hitCollider.GetComponent<Bot>().targetCircle.SetActive(false);
                    }

                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        nearestEnemy = hitCollider.transform;
                    }
                }
            }

            if (nearestEnemy != null)
            {
                // Attack the nearest enemy
                Attack(nearestEnemy);
            }
        }
        else
        {
            // Turn off target circles for all enemies when no enemies are in range
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider != null)
                {
                    Bot bot = hitCollider.GetComponent<Bot>();
                    if (bot != null)
                    {
                        bot.targetCircle.SetActive(false);
                    }
                }
            }
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
            bullet.GetComponent<Rigidbody>().velocity = direction.normalized * 5f;

            // Set the cooldown timer
            lastAutoAttackTime = Time.time;
        }
    }

    private void ScaleAttackRangeCircle()
    {
        if (attackRangeCircle != null)
        {
            // Set the local scale of the attack range circle based on the player's attack range
            float circleScale = weaponData.autoAttackRange - 1; // Multiplied by 2 to convert radius to diameter
            attackRangeCircle.transform.localScale = new Vector3(circleScale, circleScale, 1f);
        }
    }

}