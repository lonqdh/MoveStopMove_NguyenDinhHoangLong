using Lean.Pool;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class Character : MonoBehaviour
{
    //[SerializeField] public Weapon weapon;
    [SerializeField] public Bullet bulletPrefab;
    [SerializeField] public Transform throwPoint;
    [SerializeField] public float moveSpeed;
    [SerializeField] public float rotateSpeed;
    [SerializeField] public LayerMask enemyLayer;
    //[SerializeField] public float autoAttackRange = 5f;
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

    [SerializeField] protected WeaponData weaponData;
    [SerializeField] private Transform weaponHoldingPos;
    private WeaponType defaultCurrentWeapon = WeaponType.Axe;



    // Start is called before the first frame update
    protected virtual void Start()
    {
        //OnInit();
    }

    // Update is called once per frame
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
            weaponData = DataManager.Instance.GetWeaponData(defaultCurrentWeapon);
            Debug.Log("Char: " + this.name + " Weapon Type: " + weaponData.weaponType + ", Auto Attack Range: " + weaponData.autoAttackRange);
            Weapon weaponInstance = Instantiate(weaponData.weapon, weaponHoldingPos.position, weaponHoldingPos.rotation);
            weaponInstance.transform.parent = weaponHoldingPos;
        }
        if (weaponData.bullet != null)
        {
            bulletPrefab = weaponData.bullet;
        }
    }


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

            weaponData.autoAttackRange *= growthFactor;

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


