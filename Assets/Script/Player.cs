using Lean.Pool;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private FloatingJoystick joystick;
    private Vector3 moveVector;
    private bool hasInit;
    private Rigidbody rigidbody;

    private void Start()
    {
        OnInit();
        rigidbody = GetComponent<Rigidbody>();
    }

    protected override void Update() //sau neu lam update o character thi phai override va goi base.Update neu co chuc nang ca bot va nguoi lam
    {
        if(hasInit && IsDead == false)
        {
            Move();
            if(moveVector == Vector3.zero)
            {
                //base.Update();
                Attack();
            }
        }   
    }

    public void OnInit()
    {
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
        else if (joystick.Horizontal == 0 && joystick.Vertical == 0)
        {
            rigidbody.velocity = Vector3.zero;
            ChangeAnim("IsIdle");
        }
    }

    private void Attack()
    {
        float cooldownDuration = 2f;

        // Check if enough time has passed since the last attack
        if (Time.time - lastAutoAttackTime >= cooldownDuration)
        {
            Collider[] hitColliders = new Collider[10];
            int numEnemies = Physics.OverlapSphereNonAlloc(transform.position, autoAttackRange, hitColliders, enemyLayer);

            if (numEnemies > 0)
            {
                Debug.Log("Found enemies!");

                float closestDistance = float.MaxValue; //set = Max de distance giua player va enemy dau tien detected luon nho hon closestDistance ==> assign new value for closestDistance and keep checking the closest enemy detected to prioritize attacking
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
                    Vector3 direction = nearestEnemy.position - throwPoint.position;
                    direction.Normalize();

                    transform.rotation = Quaternion.LookRotation(direction);

                    ChangeAnim("IsAttack");

                    Bullet bullet = LeanPool.Spawn(bulletPrefab, throwPoint.position, throwPoint.rotation);
                    bullet.attacker = this;
                    bullet.GetComponent<Rigidbody>().velocity = direction.normalized * 5f;

                    // Set the cooldown timer
                    lastAutoAttackTime = Time.time;
                }
            }
        }
    }

}