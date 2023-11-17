using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody rb;
    public Character attacker;
    [SerializeField] private Transform bullet;
    [SerializeField] private float rotatespeed;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        bullet.Rotate(Vector3.up * rotatespeed * Time.deltaTime, Space.Self);
    }

    public void OnInit()
    {

    }

    public void OnDespawn()
    {
        LeanPool.Despawn(this);
    }

    private void OnTriggerEnter(Collider collision)
    {
        Character victim = collision.GetComponent<Character>();
        if(victim == null || victim == attacker)
        {
            return;
        }
        if (collision.tag == "Enemy" || collision.tag == "Player")
        {
            attacker.Grow();
            //collision.GetComponent<Character>().OnHit(30f);
            //Instantiate(hitVFX, transform.position, transform.rotation);
            OnDespawn();
        }
    }

}
