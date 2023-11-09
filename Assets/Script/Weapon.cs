using Lean.Pool;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

public class Weapon : Singleton<Weapon>
{
    public Rigidbody rb;
    //public Vector3 initialDirection;
    //public float range;
    //public Vector3 initialPosition;
    //public Vector3 shootDirection;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        OnInit();
    }

    private void Update()
    {
        //float distance = Vector3.Distance(initialPosition, transform.position);
        //if (distance > range)
        //{
        //    OnDespawn();
        //}
    }

    public void OnInit()
    {
        //rb.velocity = shootDirection.normalized * 5f;
    }

    public void OnDespawn()
    {
        LeanPool.Despawn(this);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Enemy")
        {
            //collision.GetComponent<Character>().OnHit(30f);
            //Instantiate(hitVFX, transform.position, transform.rotation);
            OnDespawn();
        }
    }

}


