using Lean.Pool;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Weapon weaponPrefab;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private FloatingJoystick joystick;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float autoAttackRange = 10f;
    private Vector3 moveVector;
    private bool hasInit;
    private Rigidbody rigidbody;
    private float lastAutoAttackTime;
    private Transform target;
    private Vector3 initialEnemyPosition;

    private void Start()
    {
        OnInit();
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Update() //sau neu lam update o character thi phai override va goi base.Update neu co chuc nang ca bot va nguoi lam
    {
        if(hasInit)
        {
            Move();
            if(moveVector == Vector3.zero && Time.time - lastAutoAttackTime >= 1f)
            {
                Attack();
                lastAutoAttackTime = Time.time;
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
        }
        else if (joystick.Horizontal == 0 && joystick.Vertical == 0)
        {
            rigidbody.velocity = Vector3.zero;
            //anim idle
        }
    }

    private void Attack()
    {
        Collider[] hitColliders = new Collider[10];
        int numEnemies = Physics.OverlapSphereNonAlloc(transform.position, autoAttackRange, hitColliders, enemyLayer);

        if (numEnemies > 0)
        {
            float closestDistance = float.MaxValue;

            for (int i = 0; i < numEnemies; i++)
            {
                float distance = Vector3.Distance(transform.position, hitColliders[i].transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    target = hitColliders[i].transform;
                    initialEnemyPosition = target.position;
                }

                Vector3 direction = initialEnemyPosition - transform.position;
                direction.Normalize();

                transform.rotation = Quaternion.LookRotation(direction);

                Weapon weapon = LeanPool.Spawn(weaponPrefab, throwPoint.position, throwPoint.rotation);
                weapon.GetComponent<Rigidbody>().velocity = direction.normalized * 5f;

            }
        }

    }



}
