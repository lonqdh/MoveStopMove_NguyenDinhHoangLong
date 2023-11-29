using Lean.Pool;
using UnityEngine;
using UnityEngine.UIElements;

public class Bullet : MonoBehaviour
{
    public Rigidbody rb;
    public Character attacker;
    public BoxCollider bulletCollider;
    [SerializeField] private Transform bullet;
    [SerializeField] private float rotatespeed;
    private Vector3 maximumScale = new Vector3(3, 3, 3);

    private Vector3 originalScale;
    private Vector3 originalColliderSize;
    [SerializeField] private float despawnDelay = 2f; // Adjust this value as needed

    //private float currentCharScale;

    private void Start()
    {
        //originalScale = transform.localScale;
        //transform.localScale = Vector3.one;
        //OnInit();
        //rb = GetComponent<Rigidbody>(); Cache kieu 1
        originalColliderSize = GetComponent<BoxCollider>().size;
        bulletCollider = GetComponent<BoxCollider>();
    }

    private void OnEnable()
    {
        //ScaleForBullet();
        Invoke("OnDespawn", despawnDelay); // phai lam onenable vi start chi 1 lan khi lan dau pool spawn, khi pool deactive va active lai ( respawn ) thi se k chay start vi da chay r
    }

    private void Update()
    {
        bullet.Rotate(Vector3.up * rotatespeed * Time.deltaTime, Space.Self);
    }

    public void OnInit(Character attacker)
    {
        this.attacker = attacker;
        ScaleForBullet();
    }

    //public void OnInit()
    //{

    //    ScaleForBullet();
    //}

    //public void OnInit()
    //{

    //}

    public void OnDespawn()
    {
        transform.localScale = Vector3.one; // Reset scale to (1, 1, 1)
        bulletCollider.size = originalColliderSize; // Reset collider size
        LeanPool.Despawn(this);
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
