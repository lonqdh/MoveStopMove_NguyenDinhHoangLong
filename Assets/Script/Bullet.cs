using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody rb;
    public Character attacker;
    public BoxCollider bulletCollider;
    [SerializeField] private Transform bullet;
    [SerializeField] private float rotatespeed;
    private Vector3 maximumScale = new Vector3(3,3,3);
    

    private void Start()
    {
        bulletCollider = GetComponent<BoxCollider>();
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

    public void ScaleForBullet(float growthSize)
    {
        if (transform.localScale.x < maximumScale.x)
        {
           bulletCollider.size = new Vector3(bulletCollider.size.x, bulletCollider.size.y * growthSize, bulletCollider.size.z);
        }
        transform.localScale *= growthSize;
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
