using Unity.Mathematics;
using UnityEngine;

public class GridMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Transform movePoint;
    public int action = 3;
    public int curAction;
    public bool freeMove;
    public bool canMove;

    public LayerMask stop;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public DamageText actionText;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Awake()
    {
        actionText = GameObject.FindGameObjectWithTag("ActionText").GetComponent<DamageText>();
    }

    

    void Start()
    {
        curAction = action;
        canMove = false;
        freeMove = false;
        movePoint.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        actionText.getAction(action.ToString());
        if (curAction <= 0)
        {
            if (freeMove == true) 
            { 
                canMove = true;
            }
            else
            {
                canMove = false;
            }
            
        }
        transform.position =Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime); //ngikutin transform point
        /*if(Vector3.Distance(transform.position, movePoint.position) <= 0.5f) // kalo jarak nya pas baru bisa maju lagi biar ngga melesat jauh
        {
           
            
            if (math.abs(Input.GetAxisRaw("Horizontal")) == 1f && curAction > 0) // kalo ada input, kenapa ada math abs, kan ada negatif dan positif nah jadi ngga diskriminasi
            {
                /*penjelasan buat if ini : Physics2D.OverlapCircle = buat cicle imaginer dengan posisi dan ukuran yang udah ditentukan ke layermask stop, 
            * jadi kalo cicle nya kena layer mask gitu, negasi diawal karna kita ngga mau jalan ke layer stop
                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f), 0.2f, stop))
                {
                    curAction -= 1;
                    movePoint.position += new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 0f); // nah ini baru jalan
                }
            }

            if (math.abs(Input.GetAxisRaw("Vertical")) == 1f && curAction > 0)
            {
                if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f), 0.2f, stop))
                {
                    curAction -= 1;
                    movePoint.position += new Vector3(0f, Input.GetAxisRaw("Vertical"), 0f);
                }
                
            }*/
        if (Vector3.Distance(transform.position, movePoint.position) <= 0.5f && curAction > 0)
        {
            // Prioritaskan horizontal dulu supaya tidak double-move diagonal
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                TryMove(new Vector3(1f, 0f, 0f));
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                TryMove(new Vector3(-1f, 0f, 0f));
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                TryMove(new Vector3(0f, 1f, 0f));
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                TryMove(new Vector3(0f, -1f, 0f));
            }
        }


    }


    void TryMove(Vector3 dir)
    {
        if (!Physics2D.OverlapCircle(movePoint.position + dir, 0.2f, stop))
        {
            if(freeMove == false)
            {
                curAction -= 1;
            }else
            {
                curAction = action;
            }

            movePoint.position += dir;
        }
    }

    public void resetAction()
    {
        curAction = action;
    }
}
