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
    [SerializeField] internal PantData pantData;
    [SerializeField] internal HatData hatData;
    [NonSerialized] protected float lastAutoAttackTime;
    public float attackRange;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float rotateSpeed;
    [SerializeField] protected float attackSpeed;


    [SerializeField] private float growthFactor = 1.2f;
    
    //private BoxCollider bulletCollider;
    protected string currentAnimName = "";
    protected int charLayerNumber = 3;
    protected int defaultLayerNumber = 0;
    public int currentKillCount = 0;
    public Transform hatPos;
    public float charScale = 1;
    public Bullet bulletPrefab;
    public Transform throwPoint;
    public bool IsDead = false;
    public LayerMask enemyLayer;
    public Collider charCollider; //collider giup cho overlapsphere bo qua detect ban than
    protected Weapon weaponInstance;
    public GameObject hatInstance;
    public SkinnedMeshRenderer pantInstance;


    public GameObject attackRangeCircle;

    protected virtual void Update()
    {
        //if (!IsDead)
        //{
        //    Attack();
        //}
    }

    internal virtual void OnInit()
    {
        IsDead = false;
        this.gameObject.layer = charLayerNumber;
        ChangeAnim(Constant.ANIM_IDLE);

        if (weaponData != null)
        {
            ChangeWeapon();
        }
        if (weaponData.bullet != null)
        {
            bulletPrefab = weaponData.bullet;
        }
        if(hatData != null)
        {
            ChangeHat();
        }
        if(pantData != null)
        {
            ChangePant();
        }

        ScaleAttackRangeCircle();
    }

   

    public void ChangeWeapon()
    {
        weaponData = DataManager.Instance.GetWeaponData(GameManager.Instance.UserData.EquippedWeapon);
        //attackRange = weaponData.autoAttackRange;
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
        SetAttackRange();
        SetAttackSpeed();
    }

    public void ChangeHat()
    {
        hatData = DataManager.Instance.GetHatData((HatType)GameManager.Instance.UserData.EquippedHat);
        if (hatInstance == null)
        {
            hatInstance = Instantiate(hatData.hatPrefab, hatPos);
        }
        else
        {
            Destroy(hatInstance.gameObject);
            hatInstance = Instantiate(hatData.hatPrefab, hatPos);
            hatInstance.SetActive(true);
        }
        SetAttackRange();
    }

    public void ChangePant()
    {
        pantData = DataManager.Instance.GetPantData((PantType)GameManager.Instance.UserData.EquippedPant);
        pantInstance.material = pantData.PantMaterial;
        SetCharMoveSpeed();
    }

    private void ResetScale()
    {
        transform.localScale = Vector3.one;
    }
    
    protected void SetCharMoveSpeed()
    {
        moveSpeed = pantData.Speed;
    }

    public void SetAttackRange()
    {
        attackRange = weaponData.autoAttackRange + hatData.range;
    }

    public void SetAttackSpeed()
    {
        attackSpeed = weaponData.attackSpeed;
    }

    protected virtual void OnHit()
    {
        gameObject.layer = defaultLayerNumber; // set thanh default layer de character k nham, ban toi nua
        OnDeath();
    }

    protected virtual void OnDeath()
    {
        IsDead = true;
        Debug.Log(IsDead);
        ChangeAnim(Constant.ANIM_DIE);
        Invoke(nameof(OnDespawn), 2f);
    }

    protected virtual void OnDespawn()
    {
        ResetScale();
        LevelManager.Instance.finishedLevel = false;
        LevelManager.Instance.OnFinish();
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

    protected virtual void Attack(Transform target)
    {
        float cooldownDuration = 2f;

        // Check if enough time has passed since the last attack
        if (Time.time - lastAutoAttackTime >= cooldownDuration)
        {
            Vector3 direction = target.position - throwPoint.position;

            transform.LookAt(new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z));

            ChangeAnim(Constant.ANIM_ATTACK);

            Bullet bullet = LeanPool.Spawn(weaponData.bullet, throwPoint.position, throwPoint.rotation);
            //bullet.attacker = this;
            bullet.OnInit(this);
            //bullet.bulletRigidbody.velocity = direction.normalized * 5f;
            bullet.bulletRigidbody.velocity = direction.normalized * attackSpeed;


            // Set the cooldown timer
            lastAutoAttackTime = Time.time;
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
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }

    public void ScaleAttackRangeCircle()
    {
        if (attackRangeCircle != null)
        {
            float circleScale = attackRange - 2;
            attackRangeCircle.transform.localScale = new Vector3(circleScale, circleScale, 1f);
        }
    }



    //sua collider cua bullet hammer ban ra * 1.5 voi moi lan grow

    //task : set attack range cho char dua tren weapondata, kphai thay doi attackrange cua weapondata --> kphai clone new weapondata cho moi char
    //done

}


