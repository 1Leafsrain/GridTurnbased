
using System.Collections.Generic;
using System.Xml;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Experimental.GraphView.GraphView;




public enum Targets { player, enemy, Tile }
public enum ResourceType
{
    Ammo,
    Stamina,
    Sanity,
    Health,
    Mana
}
public class Card : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public float jaraks;
    public float[] kumpulanJaraks;
    public GameObject prefabReference;

    public Vector3 TransformCard;
    public Vector3 targetPosition;

    public CardData cardData;

    public GameObject cardObject;

    public Targets targetType;
    public List<ResourceType> resourceType;

    [SerializeField] public TextMeshProUGUI title;
    [SerializeField] public TextMeshProUGUI descs;
    [SerializeField] public TextMeshProUGUI damages;
    [SerializeField] public int damage;
    [SerializeField] public List<int> cost;
    [SerializeField] public TextMeshProUGUI costText;
    [SerializeField] public SpriteRenderer image;
    [SerializeField] public List<PlainEffect> effects;
    [SerializeField] public int Area;

    public float minFontSize = 10f;
    public float maxFontSize = 40f;

    private Model model;

    private Collider2D col;
    public bool fullss;

    private Vector3 startDragPosition;

    public BoxCollider2D box;
    public BoxCollider2D boxs;
    Card myCard;
    public EnemyCard other;
    public PlayersStat otherP;
    public Tile otherT;
    private LeftCardDropArea currntDorpArea;

    public int MaxMana;
    public int curMana;
    public int maxHealth = 100;
    public int curHealth = 100;
    public int attack;
    public float tolerance = -0.05f;
    Collider2D triggerCol;
    public EnemyCard targetCard;
    public GameObject targets;

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


    public tipeSlot TipeSlot;

    public GameObject[] HandPosition;
    [SerializeField] public RightCardDropArea[] HandSlot;
    public Transform[] HandPositionTrans;
    [SerializeField] private int maxHandSize;
    public Camera camera;

    public PlayersStat Players = PlayersStat.instance;

    public void Start()
    {

        camera = Camera.main;
        masukJarak = false;
        //box = GetComponent<BoxCollider2D>();
        curHealth = maxHealth;
        curMana = MaxMana;

        fullss = false;
        set = true;
    }
    public void Fit()
    {
        float size = maxFontSize;
        costText.enableAutoSizing = true;
        costText.fontSizeMin = minFontSize;
        costText.fontSizeMax = maxFontSize;
        costText.fontSize = size;

        // TMP akan menyesuaikan sendiri selama Auto Size aktif
        costText.ForceMeshUpdate();
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

        costText.text = "";
        for (int i = 0; i < model.cost.Count; i++)
        {
            Debug.Log("Cost: " + model.cost[i] + " " + model.ResourceType[i]);
            costText.text += model.cost[i].ToString() + " " + model.ResourceType[i].ToString();
        }
        Fit();
        cost = model.cost;
        Area = model.Area;
        targetType = model.TargetType;
        resourceType = model.ResourceType;
        if (model.desc != null)
        {
            descs.text = model.desc;
        }
        else
        {
            descs.text = "No description";
        }
        damages.text = model.damage.ToString();
        attack = model.damage;
        foreach (PlainEffect effect in model.effects)
        {

            effects = model.effects;
        }
        image.sprite = model.gambar;
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

    public void OnPointerDown(PointerEventData eventData)
    {
        // Check if the button pressed was the Right Mouse Button
        if (eventData.button == PointerEventData.InputButton.Right)
        {

            TransformCard = transform.position;
            targetPosition = transform.localScale;
            transform.position = new Vector3(camera.transform.position.x, camera.transform.position.y, camera.transform.position.z + 1f);
            transform.localScale = new Vector3(3f, 3f, 1.5f);
        }

    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            transform.position = TransformCard;
            transform.localScale = targetPosition;
        }
    }


    void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(1)) return;
        startDragPosition = transform.position;
        transform.position = GetMousePositionInWorldSpace();
    }



    private void OnMouseDrag()
    {
        TransformCard = transform.position;
        targetPosition = transform.localScale;
        transform.localScale = new Vector3(0.6f, 0.6f, 1.5f);
        transform.position = GetMousePositionInWorldSpace();
        if (currntDorpArea != null)
        {
            currntDorpArea.CardLifted();
            currntDorpArea = null;
        }
        Vector2 center = box.transform.TransformPoint(box.offset);
        Vector2 size = Vector2.Scale(box.size, box.transform.lossyScale);
        float angle = transform.eulerAngles.z;

        Collider2D[] hits = Physics2D.OverlapBoxAll(center, size, angle);
        Collider2D hit = null;
        foreach (var h in hits)
        {
            if (h.gameObject != this.gameObject)
            {
                hit = h;
                break;
            }
        }
        other = null;
        otherP = null;
        otherT = null;
        switch (targetType)
        {
            case Targets.player:
                if (hit != null && hit.gameObject.CompareTag("Player"))
                {
                    otherP = hit.GetComponent<PlayersStat>();

                }
                else
                {
                    otherP = null;
                }
                break;
            case Targets.enemy:
                if (hit != null && hit.gameObject.CompareTag("EnemyHand"))
                {
                    other = hit.GetComponent<EnemyCard>();

                }
                else
                {
                    other = null;
                }
                break;
            case Targets.Tile:
                if (hit != null && hit.gameObject.CompareTag("Tile"))
                {
                    otherT = hit.GetComponent<Tile>();
                    targets = hit.gameObject;
                    masukJarak = true;
                    
                }
                else
                {
                    otherT = null;
                    targets = null;
                    masukJarak = false;
                    
                }
                break;
        }


    }

    public void OnEndDrag(PointerEventData e)
    {
        transform.position = TransformCard;
        transform.localScale = targetPosition;
    }

    public void setCurrentDrop(LeftCardDropArea leftCardDropArea)
    {

        currntDorpArea = leftCardDropArea;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        switch (targetType)
        {
            case Targets.player:
                if (collision.gameObject.CompareTag("Player"))
                {
                    targets = player;
                    masukJarak = true;
                    
                }
                else
                {
                    targets = null;
                    masukJarak = false;
                    
                }
                break;

            case Targets.enemy:
                if (collision.gameObject.CompareTag("EnemyHand"))
                {
                    targets = collision.gameObject;
                    enemys = collision.GetComponent<AigridMove>();
                    targetCard = collision.GetComponent<EnemyCard>();
                    
                }
                else
                {
                    
                }
                break;
            case Targets.Tile:
                if (collision.gameObject.CompareTag("Tile"))
                {
                    targets = collision.gameObject;
                    masukJarak = true;
                    
                }
                else
                {
                    targets = null;
                    masukJarak = false;
                    
                }
                break;
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        switch (targetType)
        {
            case Targets.player:
                if (collision.gameObject.CompareTag("Player"))
                {
                    targets = null;
                    targetCard = null;
                    transform.position = TransformCard;
                    transform.localScale = targetPosition;
                }
                break;
            case Targets.enemy:
                if (collision.gameObject.CompareTag("EnemyHand"))
                {

                    targets = null;
                    targetCard = null;
                    transform.position = TransformCard;
                    transform.localScale = targetPosition;
                }
                break;
            case Targets.Tile:
                if (collision.gameObject.CompareTag("Tile"))
                {
                    targets = null;
                    targetCard = null;
                    transform.position = TransformCard;
                    transform.localScale = targetPosition;
                }
                break;
        }
        
        
    }

    

    private void OnMouseUp()
    {
        var playerStats = player.GetComponent<PlayersStat>();
        if (Input.GetMouseButtonDown(1)) return;
        switch (targetType)
        {
            case Targets.player:
                if (otherP != null)
                {
                    if (set == true && masukJarak == true)
                    {
                        if (effects != null)
                        {
                            foreach (PlainEffect effect in effects)
                            {
                                for (int i = 0; i < resourceType.Count; i++)
                                    switch (resourceType[i])
                                    {
                                        case ResourceType.Stamina:
                                            if (playerStats.curStamina < cost[i])
                                            {
                                                Debug.Log("Not enough Stamina to play this card.");
                                                return;
                                            }
                                            break;
                                        case ResourceType.Sanity:
                                            if (playerStats.curSanity < cost[i])
                                            {
                                                Debug.Log("Not enough sanity to play this card.");
                                                return;
                                            }
                                            break;
                                        case ResourceType.Ammo:
                                            if (playerStats.curAmmo < cost[i])
                                            {
                                                Debug.Log("Not enough Ammo to play this card.");
                                                return;
                                            }
                                            break;
                                        case ResourceType.Health:
                                            if (playerStats.curHealth < cost[i])
                                            {
                                                Debug.Log("Not enough Health to play this card.");
                                                return;
                                            }
                                            break;
                                        case ResourceType.Mana:
                                            if (playerStats.curMana < cost[i])
                                            {
                                                Debug.Log("Not enough Mana to play this card.");
                                                return;
                                            }
                                            break;
                                    }
                                Debug.Log("Applying effect: " + effect.GetType().Name);
                                effect.OnBattle(targets, player, attack, UserType.player);
                                for (int i = 0; i < resourceType.Count; i++)
                                    switch (resourceType[i])
                                    {
                                        case ResourceType.Stamina:
                                            playerStats.curStamina -= cost[i]; break;
                                        case ResourceType.Sanity:
                                            playerStats.curSanity -= cost[i]; break;
                                        case ResourceType.Ammo:
                                            playerStats.curAmmo -= cost[i]; break;
                                        case ResourceType.Health:
                                            playerStats.curHealth -= cost[i]; break;
                                        case ResourceType.Mana:
                                            playerStats.curMana -= cost[i]; break;
                                    }
                            }
                        }
                        Destroy(this.gameObject);
                    }
                }
                break;

            case Targets.enemy:
                if (targetCard != null)
                {
                    if (set == true && masukJarak == true)
                    {
                        if (effects != null)
                        {
                            foreach (PlainEffect effect in effects)
                            {

                                for (int i = 0; i < resourceType.Count; i++)
                                    switch (resourceType[i])
                                    {

                                        case ResourceType.Stamina:
                                            if (playerStats.curStamina < cost[i])
                                            {
                                                Debug.Log("Not enough Stamina to play this card.");
                                                return;
                                            }
                                            break;
                                        case ResourceType.Sanity:
                                            if (playerStats.curSanity < cost[i])
                                            {
                                                Debug.Log("Not enough sanity to play this card.");
                                                return;
                                            }
                                            break;
                                        case ResourceType.Ammo:
                                            if (playerStats.curAmmo < cost[i])
                                            {
                                                Debug.Log("Not enough Ammo to play this card.");
                                                return;
                                            }
                                            break;
                                        case ResourceType.Health:
                                            if (playerStats.curHealth < cost[i])
                                            {
                                                Debug.Log("Not enough Health to play this card.");
                                                return;
                                            }
                                            break;
                                        case ResourceType.Mana:
                                            if (playerStats.curMana < cost[i])
                                            {
                                                Debug.Log("Not enough Mana to play this card.");
                                                return;
                                            }
                                            break;

                                    }
                                Debug.Log("Applying effect: " + effect.GetType().Name);
                                effect.OnBattle(targets, player, attack, UserType.player);
                                for (int i = 0; i < resourceType.Count; i++)
                                    switch (resourceType[i])
                                    {
                                        case ResourceType.Stamina:
                                            playerStats.curStamina -= cost[i]; break;
                                        case ResourceType.Sanity:
                                            playerStats.curSanity -= cost[i]; break;
                                        case ResourceType.Ammo:
                                            playerStats.curAmmo -= cost[i]; break;
                                        case ResourceType.Health:
                                            playerStats.curHealth -= cost[i]; break;
                                        case ResourceType.Mana:
                                            playerStats.curMana -= cost[i]; break;
                                    }
                            }
                        }
                        Destroy(this.gameObject);
                        return;
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
                break;
            case Targets.Tile:
                if (otherT != null)
                {
                    if (set == true)
                    {
                        if (effects != null)
                        {
                            foreach (PlainEffect effect in effects)
                            {
                                for (int i = 0; i < resourceType.Count; i++)
                                    switch (resourceType[i])
                                    {
                                        case ResourceType.Stamina:
                                            if (playerStats.curStamina < cost[i])
                                            {
                                                Debug.Log("Not enough Stamina to play this card.");
                                                return;
                                            }
                                            break;
                                        case ResourceType.Sanity:
                                            if (playerStats.curSanity < cost[i])
                                            {
                                                Debug.Log("Not enough sanity to play this card.");
                                                return;
                                            }
                                            break;
                                        case ResourceType.Ammo:
                                            if (playerStats.curAmmo < cost[i])
                                            {
                                                Debug.Log("Not enough Ammo to play this card.");
                                                return;
                                            }
                                            break;
                                        case ResourceType.Health:
                                            if (playerStats.curHealth < cost[i])
                                            {
                                                Debug.Log("Not enough Health to play this card.");
                                                return;
                                            }
                                            break;
                                        case ResourceType.Mana:
                                            if (playerStats.curMana < cost[i])
                                            {
                                                Debug.Log("Not enough Mana to play this card.");
                                                return;
                                            }
                                            break;
                                    }
                                Debug.Log("Applying effect: " + effect.GetType().Name);
                                effect.OnBattle(targets, player, attack, UserType.player);
                                for (int i = 0; i < resourceType.Count; i++)
                                    switch (resourceType[i])
                                    {
                                        case ResourceType.Stamina:
                                            playerStats.curStamina -= cost[i]; break;
                                        case ResourceType.Sanity:
                                            playerStats.curSanity -= cost[i]; break;
                                        case ResourceType.Ammo:
                                            playerStats.curAmmo -= cost[i]; break;
                                        case ResourceType.Health:
                                            playerStats.curHealth -= cost[i]; break;
                                        case ResourceType.Mana:
                                            playerStats.curMana -= cost[i]; break;
                                    }
                            }
                        }
                        Destroy(this.gameObject);
                        return;
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
                break;
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
        //playerk.curHealth = curHealth;
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

            }
            else
            {

            }
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
        if (enemys != null && targetType == Targets.enemy)
        {

            bool anyInRange = false;


            float dist = Vector2.Distance(player.transform.position, enemys.transform.position);


            if (dist <= Area)
                anyInRange = true;

            masukJarak = anyInRange;
        }


    }

}

