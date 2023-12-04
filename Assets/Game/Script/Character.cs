using Lean.Pool;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;

public class Character : MonoBehaviour
{
    //[SerializeField] public Weapon weapon;
    [SerializeField] protected Transform weaponHoldingPos;
    [SerializeField] private Animator anim;
    [SerializeField] private int killCountToGrow = 5;
    [SerializeField] internal WeaponData weaponData;
    [NonSerialized] protected float lastAutoAttackTime;
    [SerializeField] protected float attackRange;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float rotateSpeed;
    
    
    [SerializeField] private float growthFactor = 1.2f;
    
    //private BoxCollider bulletCollider;
    //private WeaponType defaultCurrentWeapon = WeaponType.Hammer;
    protected string currentAnimName = "";
    protected int defaultLayerNumber = 0;
    protected int currentKillCount = 0;
    public float charScale = 1;
    public Bullet bulletPrefab;
    public Transform throwPoint;
    public bool IsDead = false;
    public LayerMask enemyLayer;
    public Collider charCollider;

    protected Weapon weaponInstance;
    

    protected virtual void Start()
    {
        //OnInit();
    }

    protected virtual void Update()
    {
        //if (!IsDead)
        //{
        //    Attack();
        //}
    }

    internal virtual void OnInit()
    {
        if (weaponData != null)
        {
            ChangeWeapon();
        }
        if (weaponData.bullet != null)
        {
            bulletPrefab = weaponData.bullet;
        }
    }

    public void ChangeWeapon()
    {
        weaponData = DataManager.Instance.GetWeaponData(GameManager.Instance.UserData.EquippedWeapon);
        attackRange = weaponData.autoAttackRange;
        if (weaponInstance == null)
        {
            weaponInstance = Instantiate(weaponData.weapon, weaponHoldingPos.position, weaponHoldingPos.rotation);
        }
        else
        {
            Destroy(weaponInstance.gameObject);
            weaponInstance = Instantiate(weaponData.weapon, weaponHoldingPos.position, weaponHoldingPos.rotation);
        }
        weaponInstance.transform.parent = weaponHoldingPos;
    }

    protected virtual void OnHit()
    {
        //gameObject.layer = defaultLayerNumber; // set thanh default layer de character k nham, ban toi nua
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
    }

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

            //weaponData.autoAttackRange *= growthFactor;
            attackRange *= growthFactor;

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

    private void OnDrawGizmosSelected()
    {
        if (weaponData != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, weaponData.autoAttackRange);
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.layer == 3)
    //        Debug.Log("Self hit!");

    //    // Ignore the collision with the player.
    //    Physics.IgnoreCollision(collision.collider, collider);
    //    Physics.IgnoreCollision()
    //}

    //con loi khi bot spawn lai k ban
    //nguyen nhan : do ham resetsize reset attackrange thanh` 0 cho bot

    //sua collider cua bullet hammer ban ra * 1.5 voi moi lan grow

    //task : set attack range cho char dua tren weapondata, kphai thay doi attackrange cua weapondata --> kphai clone new weapondata cho moi char

}


