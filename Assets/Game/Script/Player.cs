using Lean.Pool;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.UIElements;

public class Player : Character
{
    [SerializeField] public FloatingJoystick joystick;
    [SerializeField] private Rigidbody rigidbody;
    private Vector3 moveVector;
    private bool hasInit;

    private void Start()
    {
        if (GameManager.Instance.IsState(GameState.Gameplay))
        {
            OnInit();
        }
    }

    protected override void Update() //sau neu lam update o character thi phai override va goi base.Update neu co chuc nang ca bot va nguoi lam
    {
        if (hasInit && IsDead == false && GameManager.Instance.IsState(GameState.Gameplay))
        {
            //base.Update();
            Move();
            DetectEnemies();
        }
    }

    internal override void OnInit()
    {
        base.OnInit();
        //ScaleAttackRangeCircle();
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
            ChangeAnim(Constant.ANIM_RUN);

        }
        else if (moveVector == Vector3.zero)
        {
            rigidbody.velocity = Vector3.zero;
            ChangeAnim(Constant.ANIM_IDLE);
        }
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

        if (numEnemies > 0)
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

                if (distance < attackRange)
                {
                    hitColliders[i].GetComponent<Bot>().targetCircle.SetActive(true);
                }
                else
                {
                    hitColliders[i].GetComponent<Bot>().targetCircle.SetActive(false);
                }

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    nearestEnemy = hitColliders[i].transform;
                }
            }

            if (nearestEnemy != null && moveVector == Vector3.zero)
            {
                Attack(nearestEnemy);

            }
        }
    }
    

    //da fix loi k detect duoc enemy. SOLUTION: bat freeze constraint Y cho bot
    //nguyen nhan : ?
}