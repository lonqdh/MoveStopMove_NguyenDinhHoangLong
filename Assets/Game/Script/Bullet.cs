using Lean.Pool;
using UnityEngine;
using UnityEngine.UIElements;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float despawnDelay = 2f;
    [SerializeField] private float rotatespeed = 180f;
    private Vector3 maximumScale = new Vector3(3, 3, 3);
    private Transform bulletTransform;
    public Rigidbody rb;
    public Character attacker;
    public MeshCollider bulletCollider;
    public Rigidbody bulletRigidbody;


    private void Awake()
    {
        bulletTransform = this.GetComponent<Transform>();
    }

    private void Start()
    {
        //rb = GetComponent<Rigidbody>(); Cache kieu 1
        //bulletTransform = this.GetComponent<Transform>();
        //bulletTransform = this.GetComponent<Transform>();
    }

    private void OnEnable()
    {
        //ScaleForBullet();
        /*Invoke("OnDespawn", despawnDelay);*/ // phai lam onenable vi start chi 1 lan khi lan dau pool spawn, khi pool deactive va active lai ( respawn ) thi se k chay start vi da chay r
        
    }

    private void Update()
    {

        bulletTransform.Rotate(Vector3.up, rotatespeed * Time.deltaTime);

        float distanceFromAttacker = Vector3.Distance(bulletTransform.position, attacker.transform.position);

        if (distanceFromAttacker > attacker.attackRange)
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
            bulletTransform.localScale = Vector3.one; // Reset scale to (1, 1, 1)
            LeanPool.Despawn(this);
        }
    }

    public void ScaleForBullet()
    {
        //bulletTransform.localScale *= attacker.charScale;

        //Debug.Log(attacker.name + ", " + attacker.charScale + " bullet size: " + bulletTransform.localScale);

        if (attacker != null)
        {
            if (bulletTransform != null)
            {
                bulletTransform.localScale *= attacker.charScale;
            }
            else
            {
                Debug.LogError("bulletTransform is null in ScaleForBullet!");
            }
        }
        else
        {
            Debug.LogError("Attacker is null in ScaleForBullet!");
        }
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
