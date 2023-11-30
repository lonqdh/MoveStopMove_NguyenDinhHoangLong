using Lean.Pool;
using UnityEngine;
using UnityEngine.UIElements;

public class Bullet : MonoBehaviour
{
    public Rigidbody rb;
    public Character attacker;
    public MeshCollider bulletCollider;
    //[SerializeField] private Transform bullet;
    [SerializeField] private float rotatespeed = 180f;
    private Vector3 maximumScale = new Vector3(3, 3, 3);

    [SerializeField] private float despawnDelay = 2f; 

    //private float currentCharScale;

    private void Start()
    {
        //rb = GetComponent<Rigidbody>(); Cache kieu 1

        
    }

    private void OnEnable()
    {
        //ScaleForBullet();
        /*Invoke("OnDespawn", despawnDelay);*/ // phai lam onenable vi start chi 1 lan khi lan dau pool spawn, khi pool deactive va active lai ( respawn ) thi se k chay start vi da chay r
    }

    private void Update()
    {
        //transform.Rotate(Vector3.forward, rotatespeed * Time.deltaTime);

        transform.Rotate(Vector3.up, rotatespeed * Time.deltaTime);

        float distanceFromAttacker = Vector3.Distance(transform.position, attacker.transform.position);

        if (distanceFromAttacker > attacker.weaponData.autoAttackRange)
        {
            OnDespawn();
        }
    }

    public void OnInit(Character attacker)
    {
        this.attacker = attacker;
        ScaleForBullet();
    }

    public void OnDespawn()
    {
        if (this != null)
        {
            transform.localScale = Vector3.one; // Reset scale to (1, 1, 1)
            LeanPool.Despawn(this);
        }
    }

    public void ScaleForBullet()
    {
        //currentCharScale = attacker.charScale;

        transform.localScale *= attacker.charScale;

        Debug.Log(attacker.name + ", " + attacker.charScale + " bullet size: " + transform.localScale);
    }

    private void OnTriggerEnter(Collider collision)
    {
        //Character victim = collision.GetComponent<Character>();
        Character victim = Cache.GetCharacter(collision); //Cache kieu 2
        if (victim == null || victim == attacker)
        {
            return;
        }
        if (collision.CompareTag("Enemy") || collision.CompareTag("Player"))
        {
            attacker.Grow();
            // Handle other logic on collision if needed
            OnDespawn();
        }
    }

    //1 cach toi uu: cache enemy, kieu va cham voi thg enemy nao cho vao 1 list, ve sau check xem thg enemy minh va cham da co trong list chua

    //problem: khi grow thi ban dan k cham  toi other characters

    //task : lam spin bullet

    //task : lam call back
}
