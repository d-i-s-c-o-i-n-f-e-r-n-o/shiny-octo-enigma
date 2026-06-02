using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;

public class Zombie : MonoBehaviour
{
    public bool isPlot = false; //���-�� ���� �������
    public bool moving = false;
    public bool isDead = false;
    bool isAttack = false;
    bool isActive = false;

    //��������� ��� ���������
    bool inHearZone = false;
    bool inSeeingZone = false;

    bool canHearPlayer = false;
    bool canSeePlayer = false;
    //------------------------

    bool[] viewAnimator = { false, false, false, true}; //up, dowm, left, right

    public int health = 4;
    public int speed = 2;

    [Header("Prefabs")]
    public GameObject money;
    public GameObject blood;
    GameObject currentDistraction = null;

    FPlayer player;
    Animator anim;
    Rigidbody2D body;
    SpriteRenderer spr;

    [Header("Zone Colliders")]
    public CircleCollider2D activeZone;
    public PolygonCollider2D viewZone;
    public CapsuleCollider2D hearZone;

    CapsuleCollider2D bulletZone_low;
    BoxCollider2D bulletZone_medium;
    EdgeCollider2D bulletZone_high;

    Vector2[][] viewPaths = {
        new Vector2[] { //up
            new Vector2(1,-2),
            new Vector2(-1,-2),
            new Vector2(-6,3),
            new Vector2(0,5),
            new Vector2(6,3)
        },
        new Vector2[] { //down
            new Vector2(1,1),
            new Vector2(-1,1),
            new Vector2(-6,-4),
            new Vector2(0,-6),
            new Vector2(6,-4)
        },
        new Vector2[] { //left
            new Vector2(1, -1),
            new Vector2(1, 1),
            new Vector2(-6, 6),
            new Vector2(-9, 0),
            new Vector2(-6, -6)
        },
        new Vector2[] { //right
            new Vector2(-1, -1),
            new Vector2(-1, 1),
            new Vector2(6, 6),
            new Vector2(9, 0),
            new Vector2(6, -6)
        }
    };
    float sightTimer = 0f;
    float hearTimer = 0f;
    public float sightDistance = 0.5f;
    public float sightDelay = 1f; // Time to trigger sight

    public Vector2[] patrolPath;
    int currentPatrolIndex = 0;
    public LayerMask obstacleLayer;

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<FPlayer>();
        anim = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();

        bulletZone_low = GetComponent<CapsuleCollider2D>();
        bulletZone_medium = GetComponent<BoxCollider2D>();
        bulletZone_high = GetComponent<EdgeCollider2D>();

        isAttack = isDead = moving = false;
        anim.SetBool("AlreadyDead", isDead);
    }


    void FixedUpdate()
    {
        Animation();

        canSeePlayer = inSeeingZone && CanISeeIt(player.transform);
        canHearPlayer = inHearZone && CanIHearIt(player.transform);

        if (canSeePlayer)
        {
            moving = HandlerSightZone(player.transform);
            Debug.Log($"I saw something: {sightTimer}");
            return;
        }

        if (canHearPlayer)
        {
            moving = HaldlerHearZone(player.transform);
            Debug.Log($"I heard something: {hearTimer}");
            return;
        }

        if (patrolPath.Length > 0)
        {
            moving = Patrol();
            return;
        }

    }

    bool ChasingPlayer()
    {
        if (sightTimer >= sightDelay && !isAttack)
        {
            Debug.Log("Player detected!");
            body.linearVelocity = (player.transform.position - transform.position).normalized * speed;
            return true;
        }
        else
        {
            body.linearVelocity = Vector2.zero;
            return false;
        }
        
    }

    void Animation()
    {
        if (moving && !isPlot) //��������
        {
            for (int i = 0; i < viewAnimator.Length; i++)
                viewAnimator[i] = false;

            anim.SetFloat("Speed", 1f);
            //-------------------------------------------------------------------
            int whichone;

            if (Mathf.Abs(body.linearVelocity.y) > 0.05f)   //���� �� ��� �����������
                whichone = body.linearVelocity.y > 0 ? 0 : 1;
            else
                whichone = body.linearVelocity.x > 0 ? 3 : 2;

            viewAnimator[whichone] = true;
            viewZone.points = viewPaths[whichone];
            //-------------------------------------------------------------------
            anim.SetBool("Up", viewAnimator[0]);
            anim.SetBool("Down", viewAnimator[1]);
            anim.SetBool("Left", viewAnimator[2]);
            anim.SetBool("Right", viewAnimator[3]);
            //-------------------------------------------------------------------
        }
        else
        {
            anim.SetFloat("Speed", 0f); //���������
            body.linearVelocity = Vector2.zero;
        }
            //-----------------------------------------------------------------------
            //if (playerPos.y < transform.position.y) spr.sortingOrder = -1;
            //else spr.sortingOrder = 1;
    }

    void OnCollisionEnter2D(Collision2D collision) //���� ��������� ����������� � �������, �� �������.
    {
        if (collision.gameObject.CompareTag("Player"))
            StartCoroutine(Attack(player));
    }

    void OnTriggerEnter2D(Collider2D thing)
    {
        if (thing.CompareTag("Player")) //���� � ���� �� ��� ������� �����
        {
            isActive = true;
            inHearZone = hearZone.bounds.Intersects(thing.bounds); //������ �� �� ����
            inSeeingZone = viewZone.bounds.Intersects(thing.bounds); //����� �� �� ����
            //Debug.Log($"I hear you? {canHear} || I see you? {canSee}");
        }

        if (thing.CompareTag("Distraction"))
        {
            currentDistraction = thing.gameObject;
        }

        if (thing.CompareTag("Weapon") && !isDead) //��� ����� ������� ��������� ��, ��
        {
            //������� ����� ������� ����, ��� ����� ������������

            if (bulletZone_low.bounds.Intersects(thing.bounds))
                health -= 1;
            else if (bulletZone_medium.bounds.Intersects(thing.bounds))
                health -= 2;
            else if (bulletZone_high.bounds.Intersects(thing.bounds))
                health -= 3;

            Destroy(thing.gameObject);

            if (health <= 0) StartCoroutine(Death(thing.gameObject));
        }
    }

    bool HandlerSightZone(Transform thing) //��������� ��
    {
        if (thing.CompareTag("Player"))
        {
            sightTimer += Time.deltaTime;
            return ChasingPlayer();
        }
        else sightTimer = 0;
        return false;
    }

    bool HaldlerHearZone(Transform thing)
    {
        Debug.Log("I heard something");
        if (thing.CompareTag("Player") || thing.CompareTag("Distraction"))
        {
            int a = AmIDistracted(thing);
            if (a == 3) hearTimer = 0;
            return AmIDistracted(thing) % 2 == 1; //�� �����
        }
        return false;
    }

    private void OnTriggerExit2D(Collider2D thing)
    {
        if (thing.CompareTag("Player")) //����������� �������
        {
            isActive = activeZone.bounds.Intersects(thing.bounds); //��������� �������
            inHearZone = hearZone.bounds.Intersects(thing.bounds); //������ �� �� ����
            inSeeingZone = viewZone.bounds.Intersects(thing.bounds); //����� �� �� ����

            if (!isActive) //��������������� ���� ����� ������ ������� ������
            {
                Debug.Log("I'm deactivated");
                body.linearVelocity = Vector2.zero;
                moving = false;
            }
        }
    }

    IEnumerator Attack(FPlayer pl)
    {   
        moving = false;
        pl.underAttack = isAttack = true;
        anim.SetBool("Attack", isAttack);
        //-------------------------------------------------------------------
        yield return StaticHolder.wait[5];                  //PlayerPush
        float shift_x = viewAnimator[3] ? 2f : (viewAnimator[2] ? 2f : 0f);
        float shift_y = viewAnimator[0] ? 2f : (viewAnimator[1] ? 2f : 0f);
        Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();
        playerRb.linearVelocity = new Vector2(playerRb.linearVelocity.x + shift_x, playerRb.linearVelocity.y + shift_y);
        //-------------------------------------------------------------------
        yield return StaticHolder.wait[7];                  //AttackReset
        pl.underAttack = isAttack = false;
        anim.SetBool("Attack", isAttack);
        moving = true;
    }

    IEnumerator Death(GameObject bullet)
    {
        isDead = true;
        anim.SetBool("Dead", isDead);

        //-------------------------------------------------------------------
        yield return StaticHolder.wait[1];              // DestroyBullet
        Destroy(bullet);
        //-------------------------------------------------------------------
        yield return new WaitForSeconds(0.65f);             // SpawnBlood
        float shift_x = viewAnimator[3] ? 0.4F : viewAnimator[2] ? - 0.4f : 0f;
        Instantiate(blood, new Vector3(transform.position.x + shift_x, transform.position.y - 0.8f, transform.position.z), transform.rotation);
        //-------------------------------------------------------------------
        yield return new WaitForSeconds(0.75f);             // DeadSpriteSet
        anim.SetBool("AlreadyDead", isDead);
        GetComponent<BoxCollider2D>().enabled = false;
        //-------------------------------------------------------------------
        yield return StaticHolder.wait[5];              //MoneySpawn
        //Instantiate(money, new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z), transform.rotation);
        //Destroy(gameObject);
    }
    
    private bool CanISeeIt(Transform thing)
    {
        if (thing.CompareTag("Player"))
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, (player.transform.localPosition - transform.localPosition).normalized, sightDistance, obstacleLayer);
            //Debug.Log(hit.collider is not TilemapCollider2D);
            return hit.collider is not TilemapCollider2D;
        }
        return false;
    }
    private void OnDrawGizmos()
    {
        // Draw the ray for debugging purposes
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (player.transform.position - transform.position).normalized * sightDistance);
    }

    private bool CanIHearIt(Transform thing)
    {
        if (thing.CompareTag("Player"))
            return player.speed == player.runSpeed;
        else if (thing.CompareTag("Distraction"))
            return true;

        return false;
    }

    private int AmIDistracted(Transform target)
    {
        Debug.Log($"I heard something: {hearTimer}");

        if (Vector2.Distance(transform.position, target.position) > 0.7f)
        {
            body.linearVelocity = (target.position - transform.position).normalized * speed;
            return 0; //���
        }

        body.linearVelocity = Vector2.zero;
        hearTimer += Time.deltaTime;

        if (hearTimer >= 5f) //����� �������
        {
            body.linearVelocity = (patrolPath[currentPatrolIndex] - body.position).normalized * speed;
            return Vector2.Distance(transform.localPosition, patrolPath[currentPatrolIndex]) < 0.1f ? 3 : 2; //��� ������� ��� ��� ������
        }

        return 1; //�����
    }

    bool Patrol() //����������� �� + �������� ���������� moving
    {
        Debug.Log($"I'm patroling: {currentPatrolIndex}");
        if (Vector2.Distance(transform.localPosition, patrolPath[currentPatrolIndex]) > 0.01f)
        {
            body.linearVelocity = (patrolPath[currentPatrolIndex] - body.position).normalized * speed;
            return true;
        }

        body.linearVelocity = Vector2.zero;
        currentPatrolIndex = (currentPatrolIndex + 1) % patrolPath.Length; //���� ���������

        return false;
    }
}