
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;


 public enum tipe { api, air, tanah }

public class Card : MonoBehaviour
{
    public float jaraks;
    public float[] kumpulanJaraks;
    public GameObject prefabReference;

    

    public  CardData cardData;


    [SerializeField] public TextMeshProUGUI title;
    [SerializeField] public TextMeshProUGUI damages;
    [SerializeField] public TextMeshProUGUI cost;
    [SerializeField] public SpriteRenderer image;
    [SerializeField] public List<PlainEffect> effects;
    [SerializeField] public int Area;

    private Model model;

    private Collider2D col;
    public bool fullss;

    private Vector3 startDragPosition;

    public BoxCollider2D box;
    public BoxCollider2D boxs;
    Card myCard;
    public EnemyCard other;
    private LeftCardDropArea currntDorpArea;

    public int maxHealth = 100;
    public int curHealth = 100;
    public int attack;
    public float tolerance = -0.05f;
    Collider2D triggerCol;
    public EnemyCard targetCard;
    public GameObject targets;
    public int damage = 10;
    public int target = 1;
    public int jarakArea;
    public bool masukJarak;
    public int hitTarget;

    public AigridMove enemys;
    public GameObject player;
    public PlayersStat playerk;
    public EnemyCard enemy;
    public GameObject enemyPos;

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
        cekPlayer();
        triggerCol = GetComponent<Collider2D>();
        col = GetComponent<Collider2D>();

    }

    public void setup(Model model)
    {
        this.model = model;
        title.text = model.nama;
        cost.text = model.cost;
        Area = model.Area;
        attack = model.damage;
        foreach (PlainEffect effect in model.effects)
        {
            effects = model.effects;
        }
        //image.sprite = model.gambar;
        //damages.text = model.damage.ToString();
    }
    public void cekPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        HandPosition = GameObject.FindGameObjectsWithTag("EnemyHand");
    }
    public void Update()
    {
        CalculateAndMove();
        

        //CalculateAndMove();
        if (curHealth <= 0)
        {
            Destroy(this.gameObject);
        }
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
        other = null;
        if (hit != null && hit.gameObject.CompareTag("EnemyHand"))
        {
            
            other = hit.GetComponent<EnemyCard>();
            if (other != null)
                other.TakeDamage(1);

        }
        else
        {
            other = null;
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
            targets = collision.gameObject;
            enemys = collision.GetComponent<AigridMove>();
            targetCard = collision.GetComponent<EnemyCard>();
            
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyHand"))
        {
            targets = null;
            targetCard = null;
            
        }
    }

    private void OnMouseUp()
    {
        if (targetCard != null) 
        {
            if(set == true && masukJarak == true)
            {
                if (effects != null)
                {
                    foreach (PlainEffect effect in effects)
                    {
                        Debug.Log("Applying effect: " + effect.GetType().Name);
                        effect.OnBattle(targets, attack);
                    }
                }
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
        for (int i = 0; i <= hitTarget; i++)
        {
            HandPosition[i].GetComponentInChildren<EnemyCard>();
            if (i == hitTarget)
            {
                 HandPosition[hitTarget].GetComponent<EnemyCard>().TakeDamage(damage);
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
        if (enemys != null) 
        {
            
            bool anyInRange = false;
            
            
                float dist = Vector2.Distance(player.transform.position, enemys.transform.position);
                

                if (dist <= Area)
                    anyInRange = true;
            
            masukJarak = anyInRange;
        }

        
    }

}

