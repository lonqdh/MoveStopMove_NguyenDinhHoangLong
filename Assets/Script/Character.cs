using Lean.Pool;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] public Bullet bulletPrefab;
    [SerializeField] public Transform throwPoint;
    [SerializeField] public float moveSpeed;
    [SerializeField] public float rotateSpeed;
    [SerializeField] public LayerMask enemyLayer;
    [SerializeField] public float autoAttackRange = 5f;
    public float lastAutoAttackTime;
    [SerializeField] protected bool IsDead = false;
    protected string currentAnimName;
    [SerializeField] protected Animator anim;

    //Scaling
    [SerializeField] public float growthFactor = 1.2f;
    [SerializeField] protected int killCountToGrow = 5;
    protected int currentKillCount = 0;
    private BoxCollider bulletCollider;
    public float charScale = 1;

    //protected float originalSize;
    //private float originalAttackRange;

    //[SerializeField] private float minCharRatio = 1;
    //[SerializeField] private float maxCharRatio = 3;
    //private float charRatio;

    //public float CharRatio
    //{
    //    get => charRatio; 
    //    set
    //    {
    //        charRatio = Mathf.Clamp(value,minCharRatio,maxCharRatio);
    //    }
    //}


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //if (!IsDead)
        //{
        //    Attack();
        //}

    }


    //private void Attack()
    //{
    //    float cooldownDuration = 2f;

    //    // Check if enough time has passed since the last attack
    //    if (Time.time - lastAutoAttackTime >= cooldownDuration)
    //    {
    //        Collider[] hitColliders = new Collider[10];
    //        int numEnemies = Physics.OverlapSphereNonAlloc(transform.position, autoAttackRange, hitColliders, enemyLayer);

    //        if (numEnemies > 0)
    //        {
    //            Debug.Log("Found enemies!");

    //            float closestDistance = float.MaxValue; //set = Max de distance giua player va enemy dau tien detected luon nho hon closestDistance ==> assign new value for closestDistance and keep checking the closest enemy detected to prioritize attacking
    //            Transform nearestEnemy = null;

    //            for (int i = 0; i < numEnemies; i++)
    //            {
    //                float distance = Vector3.Distance(transform.position, hitColliders[i].transform.position);

    //                if (distance < closestDistance)
    //                {
    //                    closestDistance = distance;
    //                    nearestEnemy = hitColliders[i].transform;
    //                }
    //            }

    //            if (nearestEnemy != null)
    //            {
    //                Vector3 direction = nearestEnemy.position - throwPoint.position;
    //                direction.Normalize();

    //                transform.rotation = Quaternion.LookRotation(direction);

    //                ChangeAnim("IsAttack");

    //                Bullet bullet = LeanPool.Spawn(bulletPrefab, throwPoint.position, throwPoint.rotation);
    //                bullet.attacker = this; 
    //                bullet.GetComponent<Rigidbody>().velocity = direction.normalized * 5f;

    //                // Set the cooldown timer
    //                lastAutoAttackTime = Time.time;
    //            }
    //            else
    //            {
    //                return;
    //            }
    //        }
    //    }
    //}

    protected virtual void OnHit()
    {
        //gameObject.layer = 0; // set thanh default layer de character k nham, ban toi nua
        //OnDeath();
    }

    protected virtual void OnDeath()
    {
        IsDead = true;
        ChangeAnim("IsDead");
        Invoke(nameof(OnDespawn), 2f);
    }

    protected virtual void OnDespawn()
    {
        //ResetSize();
        this.gameObject.SetActive(false);
        //Destroy(gameObject);
    }

    //protected void ResetSize()
    //{
    //    transform.localScale = originalSize * Vector3.one;
    //    autoAttackRange = originalAttackRange;
    //    ScaleAllChildren(transform, 1.0f);
    //}


    public void ChangeAnim(string animName)
    {
        if (currentAnimName != animName)
        {
            anim.ResetTrigger(currentAnimName);
            currentAnimName = animName;
            anim.SetTrigger(currentAnimName);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            if (other.GetComponent<Bullet>().attacker == this)
            {
                return;
            }

            OnHit();
        }
    }

    public virtual void Grow()
    {
        currentKillCount++;
        // If the character has enough kills to grow
        if (currentKillCount >= killCountToGrow)
        {
            transform.localScale *= growthFactor;

            autoAttackRange *= growthFactor;

            ScaleAllChildren(transform, growthFactor);

            // Reset kill count
            currentKillCount = 0;
        }
    }

    private void ScaleAllChildren(Transform parent, float scale)
    {
        if (charScale < 2f)
        {
            foreach (Transform child in parent)
            {
                child.localScale *= scale;
            }

            charScale *= growthFactor;
        }
        else
        {
            return;
        }
    }


    //con loi khi bot spawn lai k ban
    //nguyen nhan : do ham resetsize reset attackrange thanh` 0 cho bot

    //sua collider cua bullet hammer ban ra * 1.5 voi moi lan grow
}


