
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;
using static UnityEngine.Rendering.GPUSort;

 public enum tipe { api, air, tanah }

public class Card : MonoBehaviour
{
    public GameObject prefabReference;

    [SerializeField] private SpriteRenderer cardImage;
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text cost;

    public  CardData cardData;
    public Card(CardData cardData)
    {
        this.cardData = cardData;
        Effect = cardData.effect;
        Cost = cardData.cost;
    }

    public Sprite sprite { get => cardData.sprite; }
    public string Title { get => cardData.name; }
    public int Cost { get; set; }
    public string Effect { get; set; }

    private Collider2D col;
    public bool fullss;

    private Vector3 startDragPosition;

    public BoxCollider2D box;
    public BoxCollider2D boxs;
    Card myCard;

    private LeftCardDropArea currntDorpArea;

    public int maxHealth = 100;
    public int curHealth = 100;
    public int attack = 10;
    public float tolerance = -0.05f;
    Collider2D triggerCol;
    public EnemyCard targetCard;
    public int damage = 10;
    public int target = 1;
    public int jarakArea;
    public bool masukJarak;
    public int hitTarget;

    public GameObject player;
    public GameObject enemy;
    
    //tipe cardTipe;
    public Collider2D hitcollider;

    public bool set;
    public bool bisaDropefek;

    public tipe Tipes;
    public tipeSlot TipeSlot;

    public GameObject[] HandPosition;
    [SerializeField] public RightCardDropArea[] HandSlot;
    public Transform[] HandPositionTrans;
    [SerializeField] private int maxHandSize;

    public void Start()
    {
        masukJarak = false;
        box =  GetComponent<BoxCollider2D>();
        curHealth = maxHealth;
        fullss = false;
        set = true;
    }

    void Awake()
    {
        
        triggerCol = GetComponent<Collider2D>();
        col = GetComponent<Collider2D>();
        //mainCamera = Camera.main;
        player = GameObject.FindGameObjectWithTag("Player");
        HandPosition = GameObject.FindGameObjectsWithTag("EnemyHand");

    }

    public void Update()
    {
        CalculateAndMove();
        if (curHealth <= 0)
        {
            Destroy(this.gameObject);
        }
        //if (Input.GetKeyUp(KeyCode.V)) 
       // {
        //    tipeSerangan();
        //}
    }

    void OnMouseDown()
    {
        startDragPosition = transform.position;
        transform.position = GetMousePositionInWorldSpace();
        

        
    }

    private void OnMouseDrag()
    {
        
        
        transform.position = GetMousePositionInWorldSpace();
        if (currntDorpArea != null)
        {
            currntDorpArea.CardLifted();
            currntDorpArea = null;
        }
        Vector2 center = box.transform.TransformPoint(box.offset);
        Vector2 size = Vector2.Scale(box.size, box.transform.lossyScale);
        float angle = transform.eulerAngles.z;
        
        Collider2D hit = Physics2D.OverlapBox(center, size, angle);

        if (hit.gameObject.CompareTag("EnemyHand"))
        {
            EnemyCard other = hit.GetComponent<EnemyCard>();
            if (other != null)
                other.TakeDamage(1);
        }
    }

    public void OnEndDrag(PointerEventData e)
    {
        
    }

    public void setCurrentDrop(LeftCardDropArea leftCardDropArea)
    {
        
        currntDorpArea = leftCardDropArea;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyHand"))
        {
            Debug.Log("kacau men");
            enemy = this.gameObject;
            targetCard = collision.GetComponent<EnemyCard>();
            //targetCard.TakeDamage(1);
            //Destroy(this.gameObject);
        }
    }

    /*public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("EnemyHand"))
        {
            Debug.Log("kacau men");
        }
    }

    /*private void OnTriggerStay2D(Collider2D other)
    {
        // bounds dari trigger dan objek lain
        Bounds t = triggerCol.bounds;
        Bounds o = other.GetComponent<Collider2D>().bounds;

        // cek apakah bottom objek (o.min.y) kira-kira sama dengan top trigger (t.max.y)
        bool verticalMatch = Mathf.Abs(o.min.y - t.max.y) <= tolerance;

        // cek apakah center x objek berada dalam lebar trigger (agar benar-benar "di atas")
        bool horizontalInside = (o.center.x >= t.min.x) && (o.center.x <= t.max.x);

        if (verticalMatch && horizontalInside) 
        {
            
            other.gameObject.GetComponent<EnemyCard>().TakeDamage(100);
        }
    }*/


    private void OnMouseUp()
    {

        if (targetCard != null) 
        {
            if(set == true && masukJarak == true)
            {
                targetCard.GetComponent<EnemyCard>().TakeDamage(100);
                HandManager.Instance?.DiscardInstance(this);
                Destroy(this.gameObject);
            }
            
        }
        col.enabled = false;

        try
        {

            hitcollider = Physics2D.OverlapPoint(new Vector2(transform.position.x, transform.position.y));
            if (hitcollider != null && hitcollider.TryGetComponent(out ICardDropArea cardDropArea) && hitcollider.TryGetComponent(out LeftCardDropArea LeftcardDropArea))
            {
                if (LeftcardDropArea.tipes == TipeSlot)
                {
                    cardDropArea.OnCardDrop(this, true);
                    //this.transform.parent = null;
                }
                else
                {
                    transform.position = startDragPosition;
                }
                bisaDropefek = false;
            }

            else
            {
                bisaDropefek = true;
                transform.position = startDragPosition;



            }
        }





        finally
        {
            col.enabled = true;
        }
    }

    
    public Vector3 GetMousePositionInWorldSpace()
    {
        float dis = 10f;
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dis);
        Vector3 objPos = Camera.main.ScreenToWorldPoint(mousePos);
        return objPos;
    }

    public void attacks(int amount)
    {
        Debug.Log("attacking" + amount);
    }

    public void TakeDamage(int dmg)
    {
        
        curHealth -= dmg;
    }

    public void ActionCard()
    {

        hitTarget = Random.Range(0, target + 1);
       // if (target == 0) { return; }
        for (int i = 0; i <= hitTarget; i++)
        {
            HandPosition[i].GetComponentInChildren<EnemyCard>();
            if (i == hitTarget)
            {
                 HandPosition[hitTarget].GetComponent<EnemyCard>().TakeDamage(damage);
                //Debug.Log("ngasih damage " + damage);
                tipeSerangan();
            }
            else
            {
                
            }
        }
    }


    public void tipeSerangan()
    {
       
        switch (Tipes) 
        {
            case tipe.api:
                Debug.Log("mateng cheff");
                break;

            case tipe.air:
                Debug.Log("basahhh");
                break;

            case tipe.tanah:
                Debug.Log("awww");
                break;
        }

    }

    public void sets()
    {
        set = true;
    }

    public void unsets()
    {
        set = false;
    }

    void CalculateAndMove()
    {
        if (enemy == null) return;

        
        Vector2Int aiGridPos = new Vector2Int(
            Mathf.RoundToInt(player.transform.position.x),
            Mathf.RoundToInt(player.transform.position.y)
        );

        Vector2Int playerGridPos = new Vector2Int(
            Mathf.RoundToInt(enemy.transform.position.x),
            Mathf.RoundToInt(enemy.transform.position.y)
        );

        
        Vector2Int distance = playerGridPos - aiGridPos;

        
        if (distance.sqrMagnitude < (jarakArea * jarakArea))
        {
            masukJarak = true;
        }
        else
        {
            masukJarak = false;
        }
    }

}

