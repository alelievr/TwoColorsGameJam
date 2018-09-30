using UnityEngine;
using System.Collections.Generic;
// using static Unity.Mathematics;
public class NoColDebrisManager : MonoBehaviour
{
    public static NoColDebrisManager instance;
    public float innerRadius = 3;
    public float outerRadius = 5;

    CircleCollider2D circleCollider;

    Collider2D[] results = new Collider2D[16];

    List<NoColDebrisController> debrits = new List<NoColDebrisController>();

    // public Squadronleader[] squadArray;
    // public List<Squadronleader> general.squadList;

    public int debritCount;
    bool needsIntegrityCheck;
    NoColDebrisController controller;
    Queue<NoColDebrisController> debritdistancelist = new Queue<NoColDebrisController>();

    [HideInInspector]
    public float DistanceMaxOfAglo = 0;

    int integrity = 0;
    public SquadGeneral general;

    private void Awake()
    {
        instance = this;
        debritCount = 0;
    }

  
    private void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        UpdatePlayerSizeSqr();
        
        // Debug.Log("test 20 = " + my_abs(20));
        // Debug.Log("test -20 = " + my_abs(-20));
        // Debug.Log("test 0 = " + my_abs(0));
        // Debug.Log("test -0 = " + my_abs(-0));
        // Debug.Log("test 800.4545f = " + my_abs(800.4545f));
        // Debug.Log("test -800.4545f = " + my_abs(-800.4545f));
        // Debug.Log("test 4557.4545f = " + my_abs(45742157.4545f));
        // Debug.Log("test -4557.4545f = " + my_abs(-45742157.4545f));
        // Debug.Log("test 1.1f = " + my_abs(1.1f));
        // Debug.Log("test -1.1f = " + my_abs(-1.1f));

    }

    // private void OnCollisionEnter2D(Collision2D other)
    // {
    // 	if (other.gameObject.tag == "debrit")
    // 	{
    // 		AgglomerateDebrit(other.gameObject.GetComponent< NoColDebrisController >());
    // 	}
    // }

    //     float my_abs(float x)
    // {
    //     // return ((float)((int)x & 0x7FFFFFFF));
    //     int i = *(int*)&x;
    //     i = i & 0x7FFFFFFF;
    //     return ((float)i);
    // }

    public GameObject DebritCollisionCheck(Vector3 pos)
    {
        foreach (Squadronleader leader in general.squadList)
        {
            if ((pos - leader.transform.position).sqrMagnitude <= 50f)
            {
                foreach (var debrit in leader.debritList)
                {
                    if ((pos - debrit.transform.position).sqrMagnitude < 5f)
                    {
                        //AgglomerateDebrit(colTarget.GetComponent< NoColDebrisController >());
                        // Debug.Log("debritcoll check return debrit");
                        return debrit.gameObject;
                    }
                }
            }
        }
        if ((pos - transform.position).sqrMagnitude <= 12f)
        {
            // Debug.Log("debritcoll check return ASTEROID");
            return gameObject;
        }
        return null;
    }

    public void resizeCamera()
    {

    }

    void AssignSquadron(NoColDebrisController debrit)
    {
        foreach (Squadronleader leader in general.squadList)
        {
            if ((debrit.transform.position - leader.transform.position).sqrMagnitude <= 50f)
            {
                leader.debritList.Add(debrit);
            }
        }
    }

    public void AgglomerateDebrit(NoColDebrisController debrit)
    {
        float tmp;
        if ((tmp = Vector2.Distance(transform.position, debrit.transform.position)) > DistanceMaxOfAglo)
        {
            debritdistancelist.Enqueue(debrit);
            DistanceMaxOfAglo = tmp;
            resizeCamera();
        }
        AssignSquadron(debrit);
        debrits.Add(debrit);
        debrit.Agglomerate(integrity);
        debrit.onDestroyed += OnDebritDestroyed;
        debrit.onLaserReceived += (a) => { needsIntegrityCheck = true; controller = a; };
        debrit.transform.SetParent(transform, true);

        UpdatePlayerSizeSqr();
    }

    void IntegrityCheck(NoColDebrisController controller)
    {
        int count = Physics2D.OverlapCircleNonAlloc(transform.position, circleCollider.radius + 0.1f, results);

        integrity++;

        for (int i = 0; i < count; i++)
        {
            var debrit = results[i].GetComponent<NoColDebrisController>();

            if (debrit == null)
                continue;

            debrit.CheckIntegryty(integrity);
        }

        // Iterate over each debrits
        foreach (var debrit in debrits)
            if (debrit.integrity != integrity)
            {
                debrit.OnWillBeDestroyed();
                Destroy(debrit.gameObject);
            }

        needsIntegrityCheck = false;
        UpdatePlayerSizeSqr();
    }

    float size = 0;
    void UpdatePlayerSizeSqr()
    {
        size = 0;
        foreach (var debrit in debrits)
        {
            size = Mathf.Max(size, (debrit.transform.position - transform.position).magnitude/*Vector3.Distance(debrit.transform.position, transform.position)*/);
        }
        if (size < 6.25f)
            size = 6.25f;
        GameManager.instance.playerSize = size;
        GameManager.instance.playerSizeSqr = size * size;

    }

    void OnDebritDestroyed(NoColDebrisController controller)
    {
        if (debritdistancelist.Count > 0 && debritdistancelist.Peek() == controller)
        {
            debritdistancelist.Dequeue();
            while (debritdistancelist.Count > 0 && debritdistancelist.Peek() == null)
                debritdistancelist.Dequeue();
            DistanceMaxOfAglo = (debritdistancelist.Count > 0) ?
                Vector2.Distance(transform.position, debritdistancelist.Peek().transform.position) : 0;
            resizeCamera();

        }
        debrits.Remove(controller);
    }

    private void Update()
    {
        if (needsIntegrityCheck)
            IntegrityCheck(controller);
        GameManager.instance.playerPos = transform.position;
        // UpdatePlayerSizeSqr();
        // Debug.Log("player size = " + GameManager.instance.playerSize);
        // Debug.DrawLine(transform.position,new Vector3(transform.position.x + 1, transform.position.y-2, transform.position.z), Color.yellow , Time.deltaTime);
        // Debug.DrawLine(transform.position,new Vector3(transform.position.x + 2, transform.position.y-1 , transform.position.z), Color.yellow , Time.deltaTime);
        // Debug.DrawLine(transform.position,new Vector3(transform.position.x + 3, transform.position.y, transform.position.z), Color.yellow , Time.deltaTime);
        // Debug.DrawLine(transform.position,new Vector3(transform.position.x + 4, transform.position.y + 1, transform.position.z), Color.yellow , Time.deltaTime);
        // Debug.DrawLine(transform.position,new Vector3(transform.position.x + 5, transform.position.y + 2, transform.position.z), Color.yellow , Time.deltaTime);
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, Mathf.Sqrt(size));
    }
}