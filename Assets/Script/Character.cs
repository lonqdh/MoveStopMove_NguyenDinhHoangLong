using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] public Bullet bulletPrefab;
    [SerializeField] public Transform throwPoint;
    [SerializeField] public float moveSpeed;
    [SerializeField] public float rotateSpeed;
    [SerializeField] public LayerMask enemyLayer;
    [SerializeField] public float autoAttackRange = 10f;
    public float lastAutoAttackTime;
    public float stopTime;
    protected bool IsDead = false;
    protected string currentAnimName;
    [SerializeField] protected Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        ChangeAnim("IsIdle");
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
        gameObject.layer = 0; // set thanh default layer de character k nham, ban toi nua
        OnDeath();
    }

    protected virtual void OnDeath()
    {
        IsDead = true;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        ChangeAnim("IsDead");
        Invoke(nameof(OnDespawn), 2f);
    }

    protected virtual void OnDespawn()
    {
        this.gameObject.SetActive(false);
        //Destroy(gameObject);
    }


    public void ChangeAnim(string animName)
    {
        if (currentAnimName != animName)
        {
            anim.ResetTrigger(animName);
            currentAnimName = animName;
            anim.SetTrigger(currentAnimName);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            if(other.GetComponent<Bullet>().attacker == this)
            { 
                return;
            }
            
            OnHit();
        }
    }

}
